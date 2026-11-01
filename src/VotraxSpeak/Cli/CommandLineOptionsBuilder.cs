using System.Collections.Generic;
using VotraxSpeak.Speech;

namespace VotraxSpeak.Cli;

/// <summary>
/// Collects option values while the command line is parsed.
/// </summary>
internal sealed class CommandLineOptionsBuilder
{
    private readonly List<string> _textParts = [];
    private int _speedPercent = VoiceLimits.DefaultSpeedPercent;
    private int _pitch = VoiceLimits.DefaultPitch;
    private string? _outputFile;
    private string? _romPath;
    private bool _showHelp;

    /// <summary>Adds a word (or quoted phrase) to the text to speak.</summary>
    /// <param name="text">The text.</param>
    public void AddText(string text) => _textParts.Add(text);

    /// <summary>Sets the speed. The last value given wins.</summary>
    /// <param name="speedPercent">The speed, already range-checked.</param>
    public void SetSpeed(int speedPercent) => _speedPercent = speedPercent;

    /// <summary>Sets the pitch. The last value given wins.</summary>
    /// <param name="pitch">The pitch, already range-checked.</param>
    public void SetPitch(int pitch) => _pitch = pitch;

    /// <summary>Sets the WAV file to write. The last value given wins.</summary>
    /// <param name="path">The file path.</param>
    public void SetOutputFile(string path) => _outputFile = path;

    /// <summary>Sets the phoneme ROM path. The last value given wins.</summary>
    /// <param name="path">The file path.</param>
    public void SetRomPath(string path) => _romPath = path;

    /// <summary>Records that usage information was requested.</summary>
    public void RequestHelp() => _showHelp = true;

    /// <summary>
    /// Returns the options collected so far.
    /// </summary>
    /// <returns>The options.</returns>
    /// <exception cref="CommandLineException">No text to speak was given (and help was not requested).</exception>
    public CommandLineOptions Build()
    {
        string text = string.Join(" ", _textParts).Trim();

        if (text.Length == 0 && !_showHelp)
        {
            throw new CommandLineException("No text to speak was given.");
        }

        return new CommandLineOptions(text, new VoiceSettings(_speedPercent, _pitch), _outputFile, _romPath, _showHelp);
    }
}
