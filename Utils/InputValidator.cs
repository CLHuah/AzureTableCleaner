using System.Text.RegularExpressions;

namespace AzureTableCleaner.Utils;

/// <summary>
///     Validates user inputs.
/// </summary>
public class InputValidator : IInputValidator
{
    /// <inheritdoc />
    public bool ValidateConnectionString(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString)) return false;

        // Simple check for Azure Storage connection string format
        // In a real-world app, we might try to connect to validate it
        return connectionString.Contains("AccountName=") && connectionString.Contains("AccountKey=");
    }

    /// <inheritdoc />
    public bool ValidateCustomFilter(string filter)
    {
        // Basic validation for ODATA filter syntax
        // In a production app, this would need to be more robust
        if (string.IsNullOrWhiteSpace(filter)) return false;

        // Check for common operators in a valid filter
        string[] validOperators = ["eq", "ne", "gt", "ge", "lt", "le", "and", "or"];
        return validOperators.Any(op => filter.Contains($" {op} "));
    }

    /// <inheritdoc />
    public bool ValidateNumberRange(string input, int min, int max)
    {
        if (!int.TryParse(input, out var value)) return false;

        return value >= min && value <= max;
    }

    /// <inheritdoc />
    public bool ValidatePartitionKey(string partitionKey)
    {
        // Partition key can't be null or empty but can be almost any string
        return !string.IsNullOrWhiteSpace(partitionKey);
    }

    /// <inheritdoc />
    public bool ValidateRowKey(string rowKey)
    {
        // Row key can't be null or empty but can be almost any string
        return !string.IsNullOrWhiteSpace(rowKey);
    }

    /// <inheritdoc />
    public bool ValidateTableName(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName)) return false;

        // Azure Table name rules:
        // 1. Alphanumeric characters
        // 2. Cannot start with a number
        // 3. Case-sensitive
        // 4. Length between 3-63 characters
        var regex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]{2,62}$");
        return regex.IsMatch(tableName);
    }
}