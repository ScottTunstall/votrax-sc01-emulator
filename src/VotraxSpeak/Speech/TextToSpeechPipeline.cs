using System.Collections.Generic;
using System.Linq;
using VotraxSpeak.Audio;
using VotraxSpeak.Phonetics;
using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Speech;

/// <summary>
/// Turns text into speech: text to phonemes, phonemes to audio, audio to the chosen output.
/// </summary>
internal sealed class TextToSpeechPipeline
{
    private readonly ITextToPhonemeConverter _converter;
    private readonly ISpeechRenderer _renderer;
    private readonly IAudioOutput _output;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextToSpeechPipeline"/> class.
    /// </summary>
    /// <param name="converter">Works out the phonemes for the text.</param>
    /// <param name="renderer">Produces audio from the phonemes.</param>
    /// <param name="output">Receives the audio.</param>
    public TextToSpeechPipeline(ITextToPhonemeConverter converter, ISpeechRenderer renderer, IAudioOutput output)
    {
        _converter = converter;
        _renderer = renderer;
        _output = output;
    }

    /// <summary>
    /// Speaks <paramref name="text"/> to the output, returning once it has been delivered.
    /// </summary>
    /// <param name="text">The text to speak.</param>
    /// <param name="voice">The voice controls.</param>
    public void Speak(string text, VoiceSettings voice)
    {
        IReadOnlyList<VotraxPhoneme> phonemes = _converter.Convert(text);
        Speak([.. phonemes.Select(phoneme => new PhonemeCue(phoneme, null))], voice);
    }

    /// <summary>
    /// Speaks exactly <paramref name="cues"/> to the output, returning once they have been delivered.
    /// No text conversion or pause tidying is applied.
    /// </summary>
    /// <param name="cues">The phonemes to send to the chip, in order.</param>
    /// <param name="voice">The voice controls.</param>
    public void Speak(IReadOnlyList<PhonemeCue> cues, VoiceSettings voice)
    {
        AudioClip speech = _renderer.Render(cues, voice);
        _output.Deliver(PeakNormalizer.Normalize(speech));
    }
}
