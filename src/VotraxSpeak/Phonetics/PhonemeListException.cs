using System;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// Thrown when a phoneme list cannot be understood; the message is shown to the user.
/// </summary>
internal sealed class PhonemeListException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PhonemeListException"/> class.
    /// </summary>
    public PhonemeListException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PhonemeListException"/> class.
    /// </summary>
    /// <param name="message">What is wrong, phrased for the user.</param>
    public PhonemeListException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PhonemeListException"/> class.
    /// </summary>
    /// <param name="message">What is wrong, phrased for the user.</param>
    /// <param name="innerException">The underlying failure.</param>
    public PhonemeListException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
