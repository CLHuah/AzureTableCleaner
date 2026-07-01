using Azure;
using Azure.Data.Tables;
using AzureTableCleaner.Models;
using Microsoft.Extensions.Logging;

namespace AzureTableCleaner.Services;

/// <summary>
///     Service for Azure Table Storage operations.
/// </summary>
public class AzureTableService(ILogger<AzureTableService> logger) : IAzureTableService
{
    /// <inheritdoc />
    public async Task<DeleteResult> DeleteRecordsAsync(DeleteOptions options)
    {
        logger.LogInformation("Starting delete operation for table {TableName}", options.TableName);

        var result = new DeleteResult();
        var client = new TableClient(options.ConnectionString, options.TableName);

        try
        {
            // Verify the table exists. This is a deletion tool, so we must NOT
            // create the table when it is missing (CreateIfNotExists would mask a
            // mistyped name and silently report "0 records deleted").
            var serviceClient = new TableServiceClient(options.ConnectionString);
            var tableExists = false;
            await foreach (var _ in serviceClient.QueryAsync(t => t.Name == options.TableName))
            {
                tableExists = true;
                break;
            }

            if (!tableExists)
                throw new InvalidOperationException(
                    $"Table '{options.TableName}' does not exist. No records were deleted.");

            // Build the filter string based on options
            var filter = BuildFilterString(options);
            logger.LogInformation("Using filter: {Filter}", filter);

            // Query for entities to delete
            var pageable = client.QueryAsync<TableEntity>(filter);
            var entitiesToDelete = new List<TableEntity>();

            await foreach (var entity in pageable) entitiesToDelete.Add(entity);

            logger.LogInformation("Found {Count} records to delete", entitiesToDelete.Count);

            // Delete entities in batches
            if (entitiesToDelete.Count > 0)
            {
                const int batchSize = 100; // Azure Table Storage has a limit of 100 operations per batch
                for (var i = 0; i < entitiesToDelete.Count; i += batchSize)
                {
                    var batch = entitiesToDelete.Skip(i).Take(batchSize).ToList();
                    var deleteTasks = batch.Select(entity => DeleteEntityAsync(client, entity));

                    var results = await Task.WhenAll(deleteTasks);
                    result.DeletedCount += results.Count(success => success);
                    result.FailedCount += results.Count(success => !success);
                }
            }

            logger.LogInformation("Delete operation completed. {SuccessCount} succeeded, {FailCount} failed",
                result.DeletedCount, result.FailedCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during delete operation");
            throw;
        }

        return result;
    }

    private static string BuildFilterString(DeleteOptions options)
    {
        // If custom filter is provided, use it
        if (!string.IsNullOrEmpty(options.CustomFilter)) return options.CustomFilter;

        // Build filter based on partition key and/or row key.
        // Use CreateQueryFilter so values are safely escaped/quoted (an apostrophe
        // in a key would otherwise break the OData expression).
        if (!string.IsNullOrEmpty(options.PartitionKey) && !string.IsNullOrEmpty(options.RowKey))
            return TableClient.CreateQueryFilter(
                $"PartitionKey eq {options.PartitionKey} and RowKey eq {options.RowKey}");

        if (!string.IsNullOrEmpty(options.PartitionKey))
            return TableClient.CreateQueryFilter($"PartitionKey eq {options.PartitionKey}");

        // Default to empty string which will return all entities (dangerous!)
        return string.Empty;
    }

    private async Task<bool> DeleteEntityAsync(TableClient client, TableEntity entity)
    {
        try
        {
            await client.DeleteEntityAsync(entity.PartitionKey, entity.RowKey, ETag.All);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete entity {PartitionKey}/{RowKey}", entity.PartitionKey, entity.RowKey);
            return false;
        }
    }
}