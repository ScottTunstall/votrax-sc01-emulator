using System.Collections.Generic;
using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// Tidies the pauses in a phoneme sequence so that it starts with speech, has no stacked pauses,
/// and ends with a long pause that lets the chip's filters ring out.
/// </summary>
internal static class UtteranceShaper
{
    /// <summary>
    /// Returns <paramref name="phonemes"/> with leading pauses removed, each run of pauses reduced to
    /// its longest pause, and a trailing <see cref="VotraxPhoneme.PA1"/>.
    /// </summary>
    /// <param name="phonemes">The phonemes to tidy.</param>
    /// <returns>The tidied phonemes.</returns>
    public static IReadOnlyList<VotraxPhoneme> Shape(IEnumerable<VotraxPhoneme> phonemes)
    {
        var shaped = new List<VotraxPhoneme>();
        VotraxPhoneme? pendingPause = null;

        foreach (VotraxPhoneme phoneme in phonemes)
        {
            if (IsPause(phoneme))
            {
                pendingPause = pendingPause == VotraxPhoneme.PA1 ? VotraxPhoneme.PA1 : phoneme;
                continue;
            }

            if (pendingPause is not null && shaped.Count > 0)
            {
                shaped.Add(pendingPause.Value);
            }

            pendingPause = null;
            shaped.Add(phoneme);
        }

        shaped.Add(VotraxPhoneme.PA1);
        return shaped;
    }

    private static bool IsPause(VotraxPhoneme phoneme) => phoneme is VotraxPhoneme.PA0 or VotraxPhoneme.PA1;
}
