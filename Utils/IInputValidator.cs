namespace AzureTableCleaner.Utils;

/// <summary>
///     Interface for validating user inputs.
/// </summary>
public interface IInputValidator
{
    /// <summary>
    ///     Validates a connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    bool ValidateConnectionString(string connectionString);

    /// <summary>
    ///     Validates a custom filter expression.
    /// </summary>
    /// <param name="filter">The filter expression to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    bool ValidateCustomFilter(string filter);

    /// <summary>
    ///     Validates that input is within a numerical range.
    /// </summary>
    /// <param name="input">The input to validate.</param>
    /// <param name="min">Minimum allowed value (inclusive).</param>
    /// <param name="max">Maximum allowed value (inclusive).</param>
    /// <returns>True if valid, false otherwise.</returns>
    bool ValidateNumberRange(string input, int min, int max);

    /// <summary>
    ///     Validates a partition key.
    /// </summary>
    /// <param name="partitionKey">The partition key to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    bool ValidatePartitionKey(string partitionKey);

    /// <summary>
    ///     Validates a row key.
    /// </summary>
    /// <param name="rowKey">The row key to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    bool ValidateRowKey(string rowKey);

    /// <summary>
    ///     Validates a table name.
    /// </summary>
    /// <param name="tableName">The table name to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    bool ValidateTableName(string tableName);
}