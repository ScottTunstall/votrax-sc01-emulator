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
        Usage: VotraxSpeak [options] [@] <text>
               VotraxSpeak [options] --phonemes <names>
               VotraxSpeak [options] --gorf <names>

        Speaks <text> through an emulation of the Votrax SC-01 speech chip (ported from MAME).

        Options:
          --phonemes <names>     Speak these Votrax phoneme names exactly, instead of text. Names are
                                 the data-sheet ones (EH3 A1 K AE1 PA1 ...), separated by spaces or
                                 commas. HI or UP after a name sets its inflection bits ($80, $40).
                                 Nothing is added: include the PA1 pauses yourself.
          --gorf <names>         The same, with the names spelled as in the Gorf arcade source (pA2 K0 pAE
                                 ...); case matters there (pA1 and PA1 differ).
          -f, --file <path>      Write the speech to a file instead of playing it: Ogg Vorbis if the name
                                 ends in .ogg, otherwise WAV.
          -s, --speed <{0}-{1}>   Chip clock as a percentage of the nominal 720 kHz (default {2}).
                                 As on the real chip, a faster clock also raises the pitch.
          -p, --pitch <{3}-{4}>      Inflection level; higher is higher pitched (default {5}).
          -r, --rom <path>       Phoneme ROM image (default: sc01a.bin, next to
                                 the app or in the current directory).
          -h, --help             Show this help.
          @                      Treat everything after this as text, even if it starts with '-'.

        Exit codes: 0 success, 2 invalid arguments, 3 phoneme ROM missing or invalid,
                    4 speech could not be played or written.

        Examples:
          VotraxSpeak "I am a Votrax speech synthesizer."
          VotraxSpeak -s 120 -p 2 Hello there
          VotraxSpeak -f hello.wav "Hello, world!"
          VotraxSpeak -f hello.ogg "Hello, world!"
          VotraxSpeak --phonemes "PA1 I N S ER T PA1 K O1 UH3 I3 AY N N PA1"
          VotraxSpeak --gorf "PA1 I0 N S ER T PA1 K0 O1 UH3 I3 AY N N PA1"
          VotraxSpeak --gorf PA1 I0 N S ER T PA1 -f insert.ogg

        """,
        VoiceLimits.MinimumSpeedPercent,
        VoiceLimits.MaximumSpeedPercent,
        VoiceLimits.DefaultSpeedPercent,
        VoiceLimits.MinimumPitch,
        VoiceLimits.MaximumPitch,
        VoiceLimits.DefaultPitch);
}
