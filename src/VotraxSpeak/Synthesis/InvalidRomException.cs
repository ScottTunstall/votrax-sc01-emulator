using System;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// Thrown when a file is not a usable Votrax SC-01 phoneme ROM image.
/// </summary>
internal sealed class InvalidRomException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidRomException"/> class.
    /// </summary>
    public InvalidRomException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidRomException"/> class.
    /// </summary>
    /// <param name="message">Why the ROM was rejected.</param>
    public InvalidRomException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidRomException"/> class.
    /// </summary>
    /// <param name="message">Why the ROM was rejected.</param>
    /// <param name="innerException">The underlying failure.</param>
    public InvalidRomException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
