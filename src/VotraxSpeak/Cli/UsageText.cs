using System.Globalization;
using VotraxSpeak.Speech;

namespace VotraxSpeak.Cli;

/// <summary>
/// The help text shown for --help, and the hint shown after a command-line error.
/// </summary>
internal static class UsageText
{
    /// <summary>The line shown after a command-line error.</summary>
    public const string HelpHint = "Try 'VotraxSpeak --help' for more information.";

    /// <summary>Gets the help text.</summary>
    public static string Text { get; } = string.Format(
        CultureInfo.InvariantCulture,
        """
        Usage: VotraxSpeak [options] [--] <text>

        Speaks <text> through an emulation of the Votrax SC-01 speech chip (ported from MAME).

        Options:
          -f, --file <path>      Write the speech to a WAV file instead of playing it.
          -s, --speed <{0}-{1}>   Chip clock as a percentage of the nominal 720 kHz (default {2}).
                                 As on the real chip, a faster clock also raises the pitch.
          -p, --pitch <{3}-{4}>      Inflection level; higher is higher pitched (default {5}).
          -r, --rom <path>       Phoneme ROM image (default: sc01a.bin, then sc01.bin, next to
                                 the app or in the current directory).
          -h, --help             Show this help.
          --                     Treat everything after this as text, even if it starts with '-'.

        Exit codes: 0 success, 2 invalid arguments, 3 phoneme ROM missing or invalid,
                    4 speech could not be played or written.

        Examples:
          VotraxSpeak "I am a Votrax speech synthesizer."
          VotraxSpeak -s 120 -p 2 Hello there
          VotraxSpeak -f hello.wav "Hello, world!"

        """,
        VoiceLimits.MinimumSpeedPercent,
        VoiceLimits.MaximumSpeedPercent,
        VoiceLimits.DefaultSpeedPercent,
        VoiceLimits.MinimumPitch,
        VoiceLimits.MaximumPitch,
        VoiceLimits.DefaultPitch);
}
