using System.Collections.Generic;
using System.Linq;
using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// Converts rule phonemes into the Votrax SC-01 phonemes that say them.
/// </summary>
internal static class ArpabetToVotraxMapper
{
    /// <summary>
    /// Returns the chip phonemes for <paramref name="phonemes"/>.
    /// </summary>
    /// <param name="phonemes">The rule phonemes.</param>
    /// <returns>The equivalent chip phonemes, in order.</returns>
    public static IReadOnlyList<VotraxPhoneme> Map(IEnumerable<ArpabetPhoneme> phonemes) =>
        [.. phonemes.SelectMany(phoneme => ArpabetToVotraxTable.Entries[phoneme])];
}
