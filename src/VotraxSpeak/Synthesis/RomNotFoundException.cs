using System;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// Thrown when no phoneme ROM was given and none was found in the default locations.
/// </summary>
internal sealed class RomNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RomNotFoundException"/> class.
    /// </summary>
    public RomNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RomNotFoundException"/> class.
    /// </summary>
    /// <param name="message">Where the ROM was looked for.</param>
    public RomNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RomNotFoundException"/> class.
    /// </summary>
    /// <param name="message">Where the ROM was looked for.</param>
    /// <param name="innerException">The underlying failure.</param>
    public RomNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
