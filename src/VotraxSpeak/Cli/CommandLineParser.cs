using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VotraxSpeak.Phonetics;
using VotraxSpeak.Speech;

namespace VotraxSpeak.Cli;

/// <summary>
/// Parses the command line: <c>[options] [@] text…</c>.
/// </summary>
/// <remarks>
/// Text may be one quoted argument or several words, which are joined with spaces. Options may
/// appear anywhere before <c>@</c>, in any order; everything after <c>@</c> is text, so text that starts with
/// '-' can still be spoken. If an option is repeated, the last value wins. <c>--phonemes</c> and <c>--gorf</c>
/// take the phoneme names that follow, quoted as one argument or not, up to the next option.
/// </remarks>
internal static class CommandLineParser
{
    /// <summary>
    /// Parses <paramref name="arguments"/>.
    /// </summary>
    /// <param name="arguments">The command-line arguments, without the program name.</param>
    /// <returns>The options, or an error message for the user.</returns>
    public static CommandLineParseResult Parse(IReadOnlyList<string> arguments)
    {
        try
        {
            return CommandLineParseResult.Success(ParseOptions(arguments));
        }
        catch (CommandLineException exception)
        {
            return CommandLineParseResult.Failure(exception.Message);
        }
    }

    private static CommandLineOptions ParseOptions(IReadOnlyList<string> arguments)
    {
        int separator = arguments.ToList().IndexOf(CommandLineSwitches.EndOfOptions);
        int optionCount = separator < 0 ? arguments.Count : separator;

        var builder = new CommandLineOptionsBuilder();
        var cursor = new ArgumentCursor([.. arguments.Take(optionCount)]);

        while (!cursor.IsAtEnd)
        {
            ParseArgument(cursor, builder);
            cursor.MoveNext();
        }

        foreach (string text in arguments.Skip(optionCount + 1))
        {
            builder.AddText(text);
        }

        return builder.Build();
    }

    private static void ParseArgument(ArgumentCursor cursor, CommandLineOptionsBuilder builder)
    {
        string argument = cursor.Current;

        if (!IsOption(argument))
        {
            builder.AddText(argument);
            return;
        }

        switch (argument)
        {
            case CommandLineSwitches.HelpShort or CommandLineSwitches.HelpLong or CommandLineSwitches.HelpQuestion:
                builder.RequestHelp();
                break;

            case CommandLineSwitches.FileShort or CommandLineSwitches.FileLong:
                cursor.MoveNext();
                builder.SetOutputFile(RequirePath(cursor, argument));
                break;

            case CommandLineSwitches.RomShort or CommandLineSwitches.RomLong:
                cursor.MoveNext();
                builder.SetRomPath(RequirePath(cursor, argument));
                break;

            case CommandLineSwitches.Phonemes:
                cursor.MoveNext();
                builder.SetPhonemes(RequirePhonemes(cursor, argument, PhonemeDialect.Votrax));
                break;

            case CommandLineSwitches.Gorf:
                cursor.MoveNext();
                builder.SetPhonemes(RequirePhonemes(cursor, argument, PhonemeDialect.Gorf));
                break;

            case CommandLineSwitches.SpeedShort or CommandLineSwitches.SpeedLong:
                cursor.MoveNext();
                builder.SetSpeed(RequireInteger(cursor, argument, VoiceLimits.MinimumSpeedPercent, VoiceLimits.MaximumSpeedPercent));
                break;

            case CommandLineSwitches.PitchShort or CommandLineSwitches.PitchLong:
                cursor.MoveNext();
                builder.SetPitch(RequireInteger(cursor, argument, VoiceLimits.MinimumPitch, VoiceLimits.MaximumPitch));
                break;

            default:
                throw new CommandLineException(
                    $"Unknown option '{argument}'.{Suggest(argument)} To speak text that starts with '-', put {CommandLineSwitches.EndOfOptions} before it.");
        }
    }

    private static string Suggest(string argument)
    {
        string longForm = CommandLineSwitches.OptionPrefix + argument;

        return argument.Length > 2 && argument[1] != CommandLineSwitches.OptionPrefix && CommandLineSwitches.LongOptions.Contains(longForm)
            ? $" Did you mean '{longForm}'?"
            : string.Empty;
    }

    private static bool IsOption(string argument) => argument.Length > 1 && argument[0] == CommandLineSwitches.OptionPrefix;

    private static string RequireValue(ArgumentCursor cursor, string option) =>
        cursor.IsAtEnd ? throw new CommandLineException($"Option '{option}' needs a value.") : cursor.Current;

    private static string RequirePath(ArgumentCursor cursor, string option)
    {
        string path = RequireValue(cursor, option);

        if (path.Trim().Length == 0)
        {
            throw new CommandLineException($"Option '{option}' needs a file path.");
        }

        return path;
    }

    private static IReadOnlyList<PhonemeCue> RequirePhonemes(ArgumentCursor cursor, string option, PhonemeDialect dialect)
    {
        string first = RequireValue(cursor, option);

        if (IsOption(first))
        {
            throw new CommandLineException($"Option '{option}' needs a value.");
        }

        // The names may be one quoted argument or several separate ones, up to the next option.
        List<string> names = [first];

        while (cursor.Next is { } next && !IsOption(next))
        {
            cursor.MoveNext();
            names.Add(next);
        }

        string list = string.Join(" ", names);

        try
        {
            return PhonemeListParser.Parse(list, dialect);
        }
        catch (PhonemeListException exception)
        {
            throw new CommandLineException($"Option '{option}': {exception.Message}", exception);
        }
    }

    private static int RequireInteger(ArgumentCursor cursor, string option, int minimum, int maximum)
    {
        string value = RequireValue(cursor, option);
        bool isNumber = int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int number);

        if (!isNumber || number < minimum || number > maximum)
        {
            throw new CommandLineException(string.Format(
                CultureInfo.InvariantCulture,
                "Option '{0}' needs a whole number from {1} to {2}, but was given '{3}'.",
                option,
                minimum,
                maximum,
                value));
        }

        return number;
    }
}
