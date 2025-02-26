namespace AzureTableCleaner.Models;

/// <summary>
///     Contains options for deleting records from an Azure Table.
/// </summary>
public class DeleteOptions
{
    /// <summary>
    ///     Connection string for Azure Storage account.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    ///     Optional custom filter expression.
    /// </summary>
    public string? CustomFilter { get; set; }

    /// <summary>
    ///     Optional partition key filter.
    /// </summary>
    public string? PartitionKey { get; set; }

    /// <summary>
    ///     Optional row key filter.
    /// </summary>
    public string? RowKey { get; set; }

    /// <summary>
    ///     Name of the table to delete records from.
    /// </summary>
    public string TableName { get; set; } = string.Empty;
}

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