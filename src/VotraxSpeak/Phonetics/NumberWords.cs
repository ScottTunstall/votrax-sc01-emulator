using System.Collections.Generic;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// The English words used to read numbers aloud.
/// </summary>
internal static class NumberWords
{
    /// <summary>The word for the hundreds place.</summary>
    public const string Hundred = "HUNDRED";

    /// <summary>The word read for a decimal point.</summary>
    public const string Point = "POINT";

    /// <summary>Numbers are grouped in thousands.</summary>
    public const int GroupSize = 1000;

    /// <summary>Digits in one thousands group.</summary>
    public const int DigitsPerGroup = 3;

    /// <summary>Divides out the hundreds digit of a group.</summary>
    public const int HundredsDivisor = 100;

    /// <summary>Divides out the tens digit.</summary>
    public const int TensDivisor = 10;

    /// <summary>Numbers below this have a word of their own in <see cref="Units"/>.</summary>
    public const int UnitsWithOwnWords = 20;

    /// <summary>Gets the words for 0 to 19.</summary>
    public static IReadOnlyList<string> Units { get; } =
    [
        "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE",
        "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN",
    ];

    /// <summary>Gets the words for the tens, indexed by the tens digit (0 and 1 are unused).</summary>
    public static IReadOnlyList<string> Tens { get; } =
    [
        "", "", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY",
    ];

    /// <summary>Gets the names of the thousands groups, smallest first.</summary>
    public static IReadOnlyList<string> Scales { get; } =
    [
        "", "THOUSAND", "MILLION", "BILLION", "TRILLION",
    ];
}
