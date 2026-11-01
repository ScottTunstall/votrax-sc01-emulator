using System.Collections.Generic;
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
        AudioClip speech = _renderer.Render(phonemes, voice);
        _output.Deliver(PeakNormalizer.Normalize(speech));
    }
}
