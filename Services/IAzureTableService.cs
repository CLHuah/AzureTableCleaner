using AzureTableCleaner.Models;

namespace AzureTableCleaner.Services;

/// <summary>
///     Interface for Azure Table Storage operations.
/// </summary>
public interface IAzureTableService
{
    /// <summary>
    ///     Deletes records from Azure Table Storage based on the provided options.
    /// </summary>
    /// <param name="options">Options specifying what records to delete.</param>
    /// <returns>Result of the deletion operation.</returns>
    Task<DeleteResult> DeleteRecordsAsync(DeleteOptions options);
}