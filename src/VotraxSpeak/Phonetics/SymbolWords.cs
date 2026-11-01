using System.Collections.Generic;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// How the normalizer reads characters that are neither letters nor digits.
/// </summary>
internal static class SymbolWords
{
    /// <summary>The separator used between words.</summary>
    public const string Space = " ";

    /// <summary>The separator used between words, as a character.</summary>
    public const char WordSeparator = ' ';

    /// <summary>The apostrophe the rules understand.</summary>
    public const string Apostrophe = "'";

    /// <summary>The pause used for clause punctuation such as ; and :.</summary>
    public const string ClausePause = ",";

    /// <summary>The decimal point inside a number.</summary>
    public const char DecimalPoint = '.';

    /// <summary>The thousands separator inside a number.</summary>
    public const char ThousandsSeparator = ',';

    /// <summary>Gets the punctuation the letter-to-sound rules handle themselves.</summary>
    public static string RulePunctuation { get; } = ".,?!-";

    /// <summary>Gets the apostrophe look-alikes that are read as an apostrophe.</summary>
    public static string ApostropheVariants { get; } = "'‘’";

    /// <summary>Gets the punctuation that is read as a comma.</summary>
    public static string ClausePunctuation { get; } = ";:";

    /// <summary>Gets the symbols that are read as words.</summary>
    public static IReadOnlyDictionary<char, string> Spoken { get; } = new Dictionary<char, string>
    {
        ['&'] = "AND",
        ['%'] = "PERCENT",
        ['+'] = "PLUS",
        ['='] = "EQUALS",
        ['@'] = "AT",
    };
}
