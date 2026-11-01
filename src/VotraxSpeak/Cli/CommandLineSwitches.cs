namespace VotraxSpeak.Cli;

/// <summary>
/// The command-line switches the app accepts.
/// </summary>
internal static class CommandLineSwitches
{
    /// <summary>Write the speech to a WAV file instead of playing it.</summary>
    public const string FileShort = "-f";

    /// <summary>Long form of <see cref="FileShort"/>.</summary>
    public const string FileLong = "--file";

    /// <summary>Chip clock as a percentage of the nominal clock.</summary>
    public const string SpeedShort = "-s";

    /// <summary>Long form of <see cref="SpeedShort"/>.</summary>
    public const string SpeedLong = "--speed";

    /// <summary>Inflection (pitch) level.</summary>
    public const string PitchShort = "-p";

    /// <summary>Long form of <see cref="PitchShort"/>.</summary>
    public const string PitchLong = "--pitch";

    /// <summary>Path of the phoneme ROM image.</summary>
    public const string RomShort = "-r";

    /// <summary>Long form of <see cref="RomShort"/>.</summary>
    public const string RomLong = "--rom";

    /// <summary>Speak a list of phoneme names exactly as given, instead of text.</summary>
    public const string Phonemes = "--phonemes";

    /// <summary>Speak a list of phonemes spelled the way the Gorf arcade source spells them, instead of text.</summary>
    public const string Gorf = "--gorf";

    /// <summary>Show usage.</summary>
    public const string HelpShort = "-h";

    /// <summary>Long form of <see cref="HelpShort"/>.</summary>
    public const string HelpLong = "--help";

    /// <summary>Windows-style alternative for <see cref="HelpShort"/>.</summary>
    public const string HelpQuestion = "-?";

    /// <summary>Everything after this is text to speak, even if it starts with '-'.</summary>
    public const string EndOfOptions = "--";

    /// <summary>Arguments starting with this character are options.</summary>
    public const char OptionPrefix = '-';
}
