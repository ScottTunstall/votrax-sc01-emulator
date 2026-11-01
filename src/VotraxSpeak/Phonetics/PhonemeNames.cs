using System;
using System.Collections.Generic;
using System.Linq;
using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// Looks up SC-01 phonemes by name, in either spelling convention.
/// </summary>
internal static class PhonemeNames
{
    /// <summary>
    /// The Gorf source's names, indexed by phoneme code (0x00 to 0x3F). They differ from the data-sheet
    /// names where the Forth source could not use them: a <c>p</c> prefix on names that look like numbers
    /// (<c>pA2</c> is <c>A1</c>, <c>pA1</c> is <c>A2</c>) and a <c>0</c> suffix on <c>K0</c>, <c>J0</c> and <c>I0</c>.
    /// </summary>
    private static readonly string[] GorfNamesByCode =
    [
        "EH3", "EH2", "EH1", "PA0", "DT", "pA2", "pA1", "ZH",
        "AH2", "I3", "I2", "I1", "M", "N", "pB", "V",
        "CH", "SH", "Z", "AW1", "NG", "AH1", "OO1", "OO",
        "L", "K0", "J0", "H", "G", "pF", "pD", "S",
        "pA", "AY", "Y1", "UH3", "AH", "P", "O", "I0",
        "U", "Y", "T", "R", "pE", "W", "pAE", "pAE1",
        "AW2", "UH2", "UH1", "UH", "O2", "O1", "IU", "U1",
        "THV", "TH", "ER", "EH", "pE1", "AW", "PA1", "STOP",
    ];

    private static readonly Dictionary<string, VotraxPhoneme> VotraxLookup =
        CreateLookup(code => ((VotraxPhoneme)code).ToString(), StringComparer.OrdinalIgnoreCase);

    private static readonly Dictionary<string, VotraxPhoneme> GorfLookup =
        CreateLookup(code => GorfNamesByCode[code], StringComparer.Ordinal);

    /// <summary>
    /// Looks up a phoneme by name.
    /// </summary>
    /// <param name="name">The name, such as <c>AE1</c> or <c>PA1</c>. Numbers are not names.</param>
    /// <param name="dialect">The spelling convention <paramref name="name"/> is written in.</param>
    /// <param name="phoneme">The phoneme, when the name is known.</param>
    /// <returns><see langword="true"/> if <paramref name="name"/> is a phoneme name in <paramref name="dialect"/>.</returns>
    public static bool TryGetPhoneme(string name, PhonemeDialect dialect, out VotraxPhoneme phoneme) =>
        (dialect == PhonemeDialect.Gorf ? GorfLookup : VotraxLookup).TryGetValue(name, out phoneme);

    private static Dictionary<string, VotraxPhoneme> CreateLookup(Func<int, string> nameOf, StringComparer comparer) =>
        Enumerable
            .Range(0, RomLayout.EntryCount)
            .ToDictionary(nameOf, code => (VotraxPhoneme)code, comparer);
}
