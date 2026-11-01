using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// Spells out digit strings as English words.
/// </summary>
internal static class NumberToWordsConverter
{
    /// <summary>
    /// Returns the words for an unsigned whole number written as digits.
    /// </summary>
    /// <param name="digits">One or more ASCII digits.</param>
    /// <returns>
    /// The number in words ("1905" is "ONE THOUSAND NINE HUNDRED FIVE"). Numbers with a leading
    /// zero, or too long to group, are read digit by digit, as people read codes and long numbers.
    /// </returns>
    public static string ConvertInteger(string digits)
    {
        int maximumDigits = NumberWords.Scales.Count * NumberWords.DigitsPerGroup;
        bool readAsDigits = digits.Length > maximumDigits || (digits.Length > 1 && digits[0] == '0');

        return readAsDigits
            ? ConvertDigits(digits)
            : ConvertGrouped(long.Parse(digits, NumberStyles.None, CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Returns each digit as a word ("042" is "ZERO FOUR TWO").
    /// </summary>
    /// <param name="digits">One or more ASCII digits.</param>
    /// <returns>The digits in words.</returns>
    public static string ConvertDigits(string digits) =>
        string.Join(" ", digits.Select(digit => NumberWords.Units[digit - '0']));

    private static string ConvertGrouped(long value)
    {
        if (value == 0)
        {
            return NumberWords.Units[0];
        }

        var groups = new List<string>();

        for (int scale = 0; value > 0; scale++)
        {
            int group = (int)(value % NumberWords.GroupSize);

            if (group != 0)
            {
                string scaleName = NumberWords.Scales[scale];
                groups.Insert(0, scaleName.Length == 0 ? ConvertGroup(group) : ConvertGroup(group) + " " + scaleName);
            }

            value /= NumberWords.GroupSize;
        }

        return string.Join(" ", groups);
    }

    private static string ConvertGroup(int group)
    {
        var words = new List<string>();
        int hundreds = group / NumberWords.HundredsDivisor;
        int remainder = group % NumberWords.HundredsDivisor;

        if (hundreds != 0)
        {
            words.Add(NumberWords.Units[hundreds]);
            words.Add(NumberWords.Hundred);
        }

        if (remainder >= NumberWords.UnitsWithOwnWords)
        {
            words.Add(NumberWords.Tens[remainder / NumberWords.TensDivisor]);
            remainder %= NumberWords.TensDivisor;
        }

        if (remainder != 0)
        {
            words.Add(NumberWords.Units[remainder]);
        }

        return string.Join(" ", words);
    }
}
