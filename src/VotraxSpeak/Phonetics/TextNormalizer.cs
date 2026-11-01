using System;
using System.Collections.Generic;
using System.Linq;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// Rewrites free text into the form the letter-to-sound rules expect: upper-case words, numbers and
/// symbols spelled out, only the punctuation the rules understand, and single spaces between words.
/// </summary>
internal static class TextNormalizer
{
    /// <summary>
    /// Returns the normalized form of <paramref name="text"/>.
    /// </summary>
    /// <param name="text">Any text.</param>
    /// <returns>The normalized text; empty if nothing in it can be spoken.</returns>
    public static string Normalize(string text)
    {
        string expanded = string.Concat(ExpandPieces(text));
        return string.Join(SymbolWords.Space, expanded.Split([SymbolWords.WordSeparator], StringSplitOptions.RemoveEmptyEntries));
    }

    private static IEnumerable<string> ExpandPieces(string text)
    {
        int position = 0;

        while (position < text.Length)
        {
            int numberEnd = FindNumberEnd(text, position);

            if (numberEnd > position)
            {
                yield return SymbolWords.Space + SpeakNumber(text.Substring(position, numberEnd - position)) + SymbolWords.Space;
                position = numberEnd;
            }
            else
            {
                yield return SpeakCharacter(text[position]);
                position++;
            }
        }
    }

    /// <summary>
    /// Returns the end of the number starting at <paramref name="start"/>, or <paramref name="start"/>
    /// itself if no number starts there. A number may contain thousands separators and a decimal part.
    /// </summary>
    /// <param name="text">The text being normalized.</param>
    /// <param name="start">Where the number would start.</param>
    /// <returns>The index just past the number.</returns>
    private static int FindNumberEnd(string text, int start)
    {
        int end = SkipDigits(text, start);

        if (end == start)
        {
            return start;
        }

        while (IsThousandsGroup(text, end))
        {
            end += 1 + NumberWords.DigitsPerGroup;
        }

        bool hasFraction = end < text.Length - 1 && text[end] == SymbolWords.DecimalPoint && IsAsciiDigit(text[end + 1]);
        return hasFraction ? SkipDigits(text, end + 1) : end;
    }

    private static bool IsThousandsGroup(string text, int separator)
    {
        int groupEnd = separator + 1 + NumberWords.DigitsPerGroup;

        return groupEnd <= text.Length
            && text[separator] == SymbolWords.ThousandsSeparator
            && SkipDigits(text, separator + 1) == groupEnd;
    }

    private static int SkipDigits(string text, int start)
    {
        int end = start;

        while (end < text.Length && IsAsciiDigit(text[end]))
        {
            end++;
        }

        return end;
    }

    private static string SpeakNumber(string number)
    {
        string[] parts = number.Split(SymbolWords.DecimalPoint);
        string integerDigits = string.Concat(parts[0].Where(IsAsciiDigit));
        string words = NumberToWordsConverter.ConvertInteger(integerDigits);

        return parts.Length == 1
            ? words
            : words + SymbolWords.Space + NumberWords.Point + SymbolWords.Space + NumberToWordsConverter.ConvertDigits(parts[1]);
    }

    private static string SpeakCharacter(char character)
    {
        char upper = char.ToUpperInvariant(character);

        if (LetterClassifier.IsLetter(upper) || SymbolWords.RulePunctuation.IndexOf(upper) >= 0)
        {
            return char.ToString(upper);
        }

        if (SymbolWords.ApostropheVariants.IndexOf(character) >= 0)
        {
            return SymbolWords.Apostrophe;
        }

        if (SymbolWords.ClausePunctuation.IndexOf(character) >= 0)
        {
            return SymbolWords.ClausePause;
        }

        return SymbolWords.Spoken.TryGetValue(character, out string? word)
            ? SymbolWords.Space + word + SymbolWords.Space
            : SymbolWords.Space;
    }

    private static bool IsAsciiDigit(char character) => character is >= '0' and <= '9';
}
