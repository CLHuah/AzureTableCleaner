using AzureTableCleaner.Models;
using AzureTableCleaner.Services;
using AzureTableCleaner.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AzureTableCleaner;

/// <summary>
///     Main entry point for the Azure Table Cleaner application.
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        // Setup dependency injection
        var serviceProvider = ConfigureServices();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        var consoleHelper = serviceProvider.GetRequiredService<IConsoleHelper>();
        var inputValidator = serviceProvider.GetRequiredService<IInputValidator>();
        var azureTableService = serviceProvider.GetRequiredService<IAzureTableService>();

        try
        {
            // Application header
            consoleHelper.DisplayHeader("Azure Table Storage Cleaner");

            // Collect user inputs
            var deleteOptions = await CollectUserInputs(consoleHelper, inputValidator);

            // Confirm the deletion operation
            if (await ConfirmDeletion(consoleHelper, deleteOptions))
            {
                // Execute deletion
                var result = await azureTableService.DeleteRecordsAsync(deleteOptions);

                // Display results
                consoleHelper.DisplaySuccess(
                    $"Successfully deleted {result.DeletedCount} records from table '{deleteOptions.TableName}'.");
                if (result.FailedCount > 0)
                    consoleHelper.DisplayWarning($"{result.FailedCount} records failed to delete.");
            }
            else
            {
                consoleHelper.DisplayInfo("Operation cancelled by user.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during execution.");
            consoleHelper.DisplayError($"Error: {ex.Message}");
        }

        consoleHelper.DisplayInfo("Press any key to exit...");
        Console.ReadKey();
    }

    private static async Task<DeleteOptions> CollectUserInputs(IConsoleHelper consoleHelper, IInputValidator validator)
    {
        var options = new DeleteOptions
        {
            // Get connection string
            ConnectionString = await consoleHelper.GetUserInputAsync("Enter Azure Storage Connection String:",
                validator.ValidateConnectionString),

            // Get table name
            TableName = await consoleHelper.GetUserInputAsync("Enter Table Name:", validator.ValidateTableName)
        };

        // Ask for filter type
        consoleHelper.DisplayInfo("Select filter type:");
        consoleHelper.DisplayInfo("1: Filter by Partition Key only");
        consoleHelper.DisplayInfo("2: Filter by Partition Key and Row Key");
        consoleHelper.DisplayInfo("3: Filter by custom query");

        var filterChoice = await consoleHelper.GetUserInputAsync("Enter your choice (1-3):",
            input => validator.ValidateNumberRange(input, 1, 3));

        switch (int.Parse(filterChoice))
        {
            case 1:
                options.PartitionKey = await consoleHelper.GetUserInputAsync(
                    "Enter Partition Key:", validator.ValidatePartitionKey);
                break;
            case 2:
                options.PartitionKey = await consoleHelper.GetUserInputAsync(
                    "Enter Partition Key:", validator.ValidatePartitionKey);
                options.RowKey = await consoleHelper.GetUserInputAsync("Enter Row Key:", validator.ValidateRowKey);
                break;
            case 3:
                options.CustomFilter = await consoleHelper.GetUserInputAsync("Enter custom filter expression:",
                    validator.ValidateCustomFilter);
                break;
        }

        return options;
    }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Register services
        services.AddLogging(builder => { builder.AddConsole(); });

        services.AddSingleton<IConsoleHelper, ConsoleHelper>();
        services.AddSingleton<IInputValidator, InputValidator>();
        services.AddSingleton<IAzureTableService, AzureTableService>();

        return services.BuildServiceProvider();
    }

    /// <summary>
    ///     Displays deletion criteria and prompts user for confirmation before proceeding.
    /// </summary>
    /// <param name="consoleHelper">Helper for console interactions</param>
    /// <param name="options">Delete options containing filter criteria</param>
    /// <returns>True if user confirms deletion, false otherwise</returns>
    private static async Task<bool> ConfirmDeletion(IConsoleHelper consoleHelper, DeleteOptions options)
    {
        // Display warning and deletion criteria details
        consoleHelper.DisplayWarning("You are about to delete records with the following criteria:");
        consoleHelper.DisplayInfo($"Table: {options.TableName}");

        // Show partition key filter if provided
        if (!string.IsNullOrEmpty(options.PartitionKey))
            consoleHelper.DisplayInfo($"Partition Key: {options.PartitionKey}");

        // Show row key filter if provided
        if (!string.IsNullOrEmpty(options.RowKey))
            consoleHelper.DisplayInfo($"Row Key: {options.RowKey}");

        // Show custom filter if provided
        if (!string.IsNullOrEmpty(options.CustomFilter))
            consoleHelper.DisplayInfo($"Custom Filter: {options.CustomFilter}");

        // Get user confirmation
        var confirmation = await consoleHelper.GetUserInputAsync(
            "Are you sure you want to proceed with deletion? (yes/no):",
            input => input.Equals("yes", StringComparison.CurrentCultureIgnoreCase) ||
                     input.Equals("no", StringComparison.CurrentCultureIgnoreCase));

        // Return true only if user explicitly typed "yes"
        return confirmation.Equals("yes", StringComparison.CurrentCultureIgnoreCase);
    }
}