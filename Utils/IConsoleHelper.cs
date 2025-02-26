namespace AzureTableCleaner.Utils;

/// <summary>
///     Interface for console interaction helper methods.
/// </summary>
public interface IConsoleHelper
{
    /// <summary>
    ///     Displays an error message.
    /// </summary>
    /// <param name="message">The message to display.</param>
    void DisplayError(string message);

    /// <summary>
    ///     Displays a header in the console.
    /// </summary>
    /// <param name="text">The header text to display.</param>
    void DisplayHeader(string text);

    /// <summary>
    ///     Displays an informational message.
    /// </summary>
    /// <param name="message">The message to display.</param>
    void DisplayInfo(string message);

    /// <summary>
    ///     Displays a success message.
    /// </summary>
    /// <param name="message">The message to display.</param>
    void DisplaySuccess(string message);

    /// <summary>
    ///     Displays a warning message.
    /// </summary>
    /// <param name="message">The message to display.</param>
    void DisplayWarning(string message);

    /// <summary>
    ///     Gets user input with validation.
    /// </summary>
    /// <param name="prompt">The prompt to display to the user.</param>
    /// <param name="validator">Function to validate the input.</param>
    /// <returns>The validated user input.</returns>
    Task<string> GetUserInputAsync(string prompt, Func<string, bool> validator);
}