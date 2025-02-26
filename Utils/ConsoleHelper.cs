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
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(new string('=', Console.WindowWidth - 1));
        Console.WriteLine(text.ToUpper());
        Console.WriteLine(new string('=', Console.WindowWidth - 1));
        Console.ResetColor();
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