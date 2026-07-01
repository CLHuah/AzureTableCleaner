using AzureTableCleaner.Services;
using AzureTableCleaner.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AzureTableCleaner;

/// <summary>
///     Composition root for the Azure Table Cleaner application.
/// </summary>
public class Program
{
    public static async Task Main()
    {
        using var serviceProvider = ConfigureServices();

        var app = serviceProvider.GetRequiredService<ICleanerApplication>();
        await app.RunAsync();

        WaitForExit(serviceProvider.GetRequiredService<IConsoleHelper>());
    }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(builder => builder.AddConsole());

        services.AddSingleton<IConsoleHelper, ConsoleHelper>();
        services.AddSingleton<IInputValidator, InputValidator>();
        services.AddSingleton<IAzureTableService, AzureTableService>();
        services.AddSingleton<ICleanerApplication, CleanerApplication>();

        return services.BuildServiceProvider();
    }

    /// <summary>
    ///     Waits for a key press before exiting, but only when an interactive
    ///     console is attached. <see cref="Console.ReadKey()" /> throws when stdin
    ///     is redirected (common on macOS/Unix pipelines and non-interactive hosts).
    /// </summary>
    private static void WaitForExit(IConsoleHelper console)
    {
        if (Console.IsInputRedirected) return;

        console.DisplayInfo("Press any key to exit...");
        Console.ReadKey();
    }
}
