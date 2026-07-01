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
    ///     Name of the table to delete records from.
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    ///     Optional partition key filter.
    /// </summary>
    public string? PartitionKey { get; set; }

    /// <summary>
    ///     Optional row key filter.
    /// </summary>
    public string? RowKey { get; set; }

    /// <summary>
    ///     Optional custom filter expression.
    /// </summary>
    public string? CustomFilter { get; set; }
}
