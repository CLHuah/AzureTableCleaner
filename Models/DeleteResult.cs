namespace AzureTableCleaner.Models;

/// <summary>
///     Result of a delete operation.
/// </summary>
public class DeleteResult
{
    /// <summary>
    ///     Number of records successfully deleted.
    /// </summary>
    public int DeletedCount { get; set; }

    /// <summary>
    ///     Number of records that failed to delete.
    /// </summary>
    public int FailedCount { get; set; }
}
