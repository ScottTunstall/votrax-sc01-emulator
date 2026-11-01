using System.Collections.Generic;
using VotraxSpeak.Audio;
using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Speech;

/// <summary>
/// Produces the audio for a sequence of Votrax SC-01 phonemes.
/// </summary>
internal interface ISpeechRenderer
{
    /// <summary>
    /// Returns the audio the chip produces when it is fed <paramref name="phonemes"/>.
    /// </summary>
    /// <param name="phonemes">The phonemes, in order.</param>
    /// <param name="voice">The chip's voice controls.</param>
    /// <returns>The rendered audio.</returns>
    AudioClip Render(IReadOnlyList<VotraxPhoneme> phonemes, VoiceSettings voice);
}
