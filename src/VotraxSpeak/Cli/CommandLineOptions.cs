using VotraxSpeak.Speech;

namespace VotraxSpeak.Cli;

/// <summary>
/// What the user asked for on the command line.
/// </summary>
internal sealed class CommandLineOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineOptions"/> class.
    /// </summary>
    /// <param name="text">The text to speak.</param>
    /// <param name="voice">The voice controls.</param>
    /// <param name="outputFile">The WAV file to write instead of playing, if any.</param>
    /// <param name="romPath">The phoneme ROM to use instead of the default, if any.</param>
    /// <param name="showHelp">Whether the user asked for usage information.</param>
    public CommandLineOptions(string text, VoiceSettings voice, string? outputFile, string? romPath, bool showHelp)
    {
        Text = text;
        Voice = voice;
        OutputFile = outputFile;
        RomPath = romPath;
        ShowHelp = showHelp;
    }

    /// <summary>Gets the text to speak.</summary>
    public string Text { get; }

    /// <summary>Gets the voice controls.</summary>
    public VoiceSettings Voice { get; }

    /// <summary>Gets the WAV file to write instead of playing, or <see langword="null"/> to play.</summary>
    public string? OutputFile { get; }

    /// <summary>Gets the phoneme ROM path, or <see langword="null"/> to search the default locations.</summary>
    public string? RomPath { get; }

    /// <summary>Gets a value indicating whether the user asked for usage information.</summary>
    public bool ShowHelp { get; }
}
