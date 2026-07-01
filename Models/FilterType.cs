namespace AzureTableCleaner.Models;

/// <summary>
///     The strategy used to select which records to delete.
/// </summary>
public enum FilterType
{
    /// <summary>Match all records with a given partition key.</summary>
    PartitionKeyOnly = 1,

    /// <summary>Match a single record by partition key and row key.</summary>
    PartitionAndRowKey = 2,

    /// <summary>Match records using a raw OData filter expression.</summary>
    CustomQuery = 3
}
