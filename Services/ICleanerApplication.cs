namespace AzureTableCleaner.Services;

/// <summary>
///     Drives the end-to-end console workflow: gather input, confirm, delete, report.
/// </summary>
public interface ICleanerApplication
{
    /// <summary>
    ///     Runs the cleaner from start to finish. Never throws; failures are logged
    ///     and reported to the user.
    /// </summary>
    Task RunAsync();
}
