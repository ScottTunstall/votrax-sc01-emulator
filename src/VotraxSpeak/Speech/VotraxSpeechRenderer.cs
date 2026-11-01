using System;
using System.Collections.Generic;
using VotraxSpeak.Audio;
using VotraxSpeak.Phonetics;
using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Speech;

/// <summary>
/// Renders phonemes by driving a freshly reset <see cref="VotraxSc01Chip"/> the way a host
/// computer would: write a phoneme, wait for the request line, write the next.
/// </summary>
internal sealed class VotraxSpeechRenderer : ISpeechRenderer
{
    private readonly PhonemeRom _rom;

    /// <summary>
    /// Initializes a new instance of the <see cref="VotraxSpeechRenderer"/> class.
    /// </summary>
    /// <param name="rom">The chip's phoneme ROM.</param>
    public VotraxSpeechRenderer(PhonemeRom rom)
    {
        _rom = rom;
    }

    /// <inheritdoc/>
    public AudioClip Render(IReadOnlyList<PhonemeCue> cues, VoiceSettings voice)
    {
        var chip = new VotraxSc01Chip(_rom, voice.ClockHz);
        var samples = new List<float>();

        foreach (PhonemeCue cue in cues)
        {
            chip.SetInflection(cue.Inflection ?? voice.Pitch);
            chip.Write(cue.Phoneme);
            RenderUntilRequest(chip, samples);
        }

        return new AudioClip((int)Math.Round(chip.SampleRateHz), samples);
    }

    private static void RenderUntilRequest(VotraxSc01Chip chip, List<float> samples)
    {
        do
        {
            chip.Advance();
            samples.Add((float)chip.Output);
        }
        while (!chip.IsRequestAsserted);
    }
}
