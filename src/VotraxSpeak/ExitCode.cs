namespace VotraxSpeak;

/// <summary>
/// The process exit codes.
/// </summary>
internal enum ExitCode
{
    /// <summary>The text was spoken or written.</summary>
    Success = 0,

    /// <summary>The command line was invalid.</summary>
    InvalidArguments = 2,

    /// <summary>The phoneme ROM was missing, unreadable or not a known dump.</summary>
    RomUnavailable = 3,

    /// <summary>The speech could not be played or written to the file.</summary>
    OutputFailed = 4,
}
