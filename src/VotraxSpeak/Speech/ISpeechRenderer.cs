using System.Collections.Generic;
using VotraxSpeak.Audio;
using VotraxSpeak.Phonetics;

namespace VotraxSpeak.Speech;

/// <summary>
/// Produces the audio for a sequence of Votrax SC-01 phonemes.
/// </summary>
internal interface ISpeechRenderer
{
    /// <summary>
    /// Returns the audio the chip produces when it is fed <paramref name="cues"/>.
    /// </summary>
    /// <param name="cues">The phonemes, in order, with any per-phoneme inflection.</param>
    /// <param name="voice">The chip's voice controls.</param>
    /// <returns>The rendered audio.</returns>
    AudioClip Render(IReadOnlyList<PhonemeCue> cues, VoiceSettings voice);
}
