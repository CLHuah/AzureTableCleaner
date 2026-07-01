using AzureTableCleaner.Models;
using AzureTableCleaner.Utils;
using Microsoft.Extensions.Logging;

namespace AzureTableCleaner.Services;

/// <summary>
///     Orchestrates the interactive Azure Table cleaning workflow.
/// </summary>
public class CleanerApplication(
    ILogger<CleanerApplication> logger,
    IConsoleHelper console,
    IInputValidator validator,
    IAzureTableService tableService) : ICleanerApplication
{
    /// <inheritdoc />
    public async Task RunAsync()
    {
        try
        {
            console.DisplayHeader("Azure Table Storage Cleaner");

            var options = await CollectDeleteOptionsAsync();

            if (!await ConfirmDeletionAsync(options))
            {
                console.DisplayInfo("Operation cancelled by user.");
                return;
            }

            var result = await tableService.DeleteRecordsAsync(options);
            DisplayResult(options, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during execution.");
            console.DisplayError($"Error: {ex.Message}");
        }
    }

    /// <summary>
    ///     Prompts the user for the connection details and the filter that
    ///     determines which records to delete.
    /// </summary>
    private async Task<DeleteOptions> CollectDeleteOptionsAsync()
    {
        var options = new DeleteOptions
        {
            ConnectionString = await console.GetUserInputAsync(
                "Enter Azure Storage Connection String:", validator.ValidateConnectionString),
            TableName = await console.GetUserInputAsync(
                "Enter Table Name:", validator.ValidateTableName)
        };

        switch (await PromptForFilterTypeAsync())
        {
            case FilterType.PartitionKeyOnly:
                options.PartitionKey = await PromptForPartitionKeyAsync();
                break;

            case FilterType.PartitionAndRowKey:
                options.PartitionKey = await PromptForPartitionKeyAsync();
                options.RowKey = await console.GetUserInputAsync(
                    "Enter Row Key:", validator.ValidateRowKey);
                break;

            case FilterType.CustomQuery:
                options.CustomFilter = await console.GetUserInputAsync(
                    "Enter custom filter expression:", validator.ValidateCustomFilter);
                break;
        }

        return options;
    }

    /// <summary>
    ///     Shows the filter menu and returns the user's validated choice.
    /// </summary>
    private async Task<FilterType> PromptForFilterTypeAsync()
    {
        console.DisplayInfo("Select filter type:");
        console.DisplayInfo("1: Filter by Partition Key only");
        console.DisplayInfo("2: Filter by Partition Key and Row Key");
        console.DisplayInfo("3: Filter by custom query");

        var choice = await console.GetUserInputAsync(
            "Enter your choice (1-3):",
            input => validator.ValidateNumberRange(input, 1, 3));

        return (FilterType)int.Parse(choice);
    }

    private Task<string> PromptForPartitionKeyAsync() =>
        console.GetUserInputAsync("Enter Partition Key:", validator.ValidatePartitionKey);

    /// <summary>
    ///     Displays the deletion criteria and asks the user to confirm.
    /// </summary>
    /// <returns><c>true</c> only if the user explicitly types "yes".</returns>
    private async Task<bool> ConfirmDeletionAsync(DeleteOptions options)
    {
        console.DisplayWarning("You are about to delete records with the following criteria:");
        console.DisplayInfo($"Table: {options.TableName}");

        if (!string.IsNullOrEmpty(options.PartitionKey))
            console.DisplayInfo($"Partition Key: {options.PartitionKey}");

        if (!string.IsNullOrEmpty(options.RowKey))
            console.DisplayInfo($"Row Key: {options.RowKey}");

        if (!string.IsNullOrEmpty(options.CustomFilter))
            console.DisplayInfo($"Custom Filter: {options.CustomFilter}");

        var confirmation = await console.GetUserInputAsync(
            "Are you sure you want to proceed with deletion? (yes/no):",
            input => input.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                     input.Equals("no", StringComparison.OrdinalIgnoreCase));

        return confirmation.Equals("yes", StringComparison.OrdinalIgnoreCase);
    }

    private void DisplayResult(DeleteOptions options, DeleteResult result)
    {
        console.DisplaySuccess(
            $"Successfully deleted {result.DeletedCount} records from table '{options.TableName}'.");

        if (result.FailedCount > 0)
            console.DisplayWarning($"{result.FailedCount} records failed to delete.");
    }
}
