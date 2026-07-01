namespace AzureTableCleaner.Utils;

/// <summary>
///     Helper for console interactions with the user.
/// </summary>
public class ConsoleHelper : IConsoleHelper
{
    /// <inheritdoc />
    public void DisplayError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <inheritdoc />
    public void DisplayHeader(string text)
    {
        var separator = new string('=', GetSafeWidth());

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(separator);
        Console.WriteLine(text.ToUpperInvariant());
        Console.WriteLine(separator);
        Console.ResetColor();
    }

    /// <summary>
    ///     Returns a usable console width for drawing separators.
    ///     On macOS/Unix (and when output is redirected) <see cref="Console.WindowWidth" />
    ///     can throw or report 0, so fall back to a sensible default.
    /// </summary>
    private static int GetSafeWidth()
    {
        try
        {
            var width = Console.WindowWidth - 1;
            return width > 0 ? width : 80;
        }
        catch (IOException)
        {
            return 80;
        }
    }

    /// <inheritdoc />
    public void DisplayInfo(string message)
    {
        Console.WriteLine(message);
    }

    /// <inheritdoc />
    public void DisplaySuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <inheritdoc />
    public void DisplayWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <inheritdoc />
    public async Task<string> GetUserInputAsync(string prompt, Func<string, bool> validator)
    {
        string input;
        bool isValid;

        do
        {
            Console.Write($"{prompt} ");
            input = Console.ReadLine() ?? string.Empty;

            isValid = validator(input);
            if (!isValid) DisplayError("Invalid input. Please try again.");
        } while (!isValid);

        return input;
    }
}