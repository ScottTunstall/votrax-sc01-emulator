using System;
using System.Collections.Generic;
using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// Reads a list of Votrax SC-01 phoneme names, for example
/// <c>PA1 I N S ER T PA1 K O1 UH3 I3 AY N N PA1</c>.
/// </summary>
/// <remarks>
/// Names are the data-sheet names (as in MAME), separated by spaces or commas, in any case. <c>HI</c> and
/// <c>UP</c> modify the
/// phoneme before them, as in the Gorf arcade source (<c>I1 UP Y UP AE</c>). Nothing is added to or removed from
/// the list, so the chip hears exactly what was written.
/// </remarks>
internal static class PhonemeListParser
{
    private static readonly char[] Separators = [' ', ',', '\t', '\r', '\n'];

    /// <summary>
    /// Parses <paramref name="list"/>.
    /// </summary>
    /// <param name="list">The phoneme list.</param>
    /// <param name="dialect">The spelling convention the names are written in.</param>
    /// <returns>The phonemes, in order, each with its inflection if one was given.</returns>
    /// <exception cref="PhonemeListException">The list is empty, has an unknown name, or starts with a modifier.</exception>
    public static IReadOnlyList<PhonemeCue> Parse(string list, PhonemeDialect dialect)
    {
        var cues = new List<PhonemeCue>();

        foreach (string word in list.Split(Separators, StringSplitOptions.RemoveEmptyEntries))
        {
            if (TryGetInflectionBit(word, out int bit))
            {
                ApplyToPrevious(cues, word, bit);
            }
            else if (PhonemeNames.TryGetPhoneme(word, dialect, out VotraxPhoneme phoneme))
            {
                cues.Add(new PhonemeCue(phoneme, null));
            }
            else
            {
                throw new PhonemeListException($"'{word}' is not a {dialect} phoneme name.");
            }
        }

        return cues.Count > 0 ? cues : throw new PhonemeListException("The phoneme list is empty.");
    }

    private static bool TryGetInflectionBit(string word, out int bit)
    {
        if (string.Equals(word, PhonemeListSyntax.HighModifier, StringComparison.OrdinalIgnoreCase))
        {
            bit = PhonemeListSyntax.HighInflectionBit;
            return true;
        }

        bit = PhonemeListSyntax.UpInflectionBit;
        return string.Equals(word, PhonemeListSyntax.UpModifier, StringComparison.OrdinalIgnoreCase);
    }

    private static void ApplyToPrevious(List<PhonemeCue> cues, string modifier, int bit)
    {
        if (cues.Count == 0)
        {
            throw new PhonemeListException($"'{modifier}' must follow the phoneme it modifies.");
        }

        int last = cues.Count - 1;
        cues[last] = cues[last] with { Inflection = (cues[last].Inflection ?? 0) | bit };
    }
}
