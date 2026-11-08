using System;

namespace VotraxSpeak.Audio;

/// <summary>
/// Completes the output file name the user typed.
/// </summary>
internal static class OutputFilePaths
{
    /// <summary>
    /// Returns <paramref name="path"/>, with <c>.wav</c> added if its file name has no extension. A file
    /// name that already has any extension is returned as it is.
    /// </summary>
    /// <param name="path">The output file path.</param>
    /// <returns>The path to write.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
    public static string WithDefaultExtension(string path)
    {
#if NET
        ArgumentNullException.ThrowIfNull(path);
#else
        if (path is null)
        {
            throw new ArgumentNullException(nameof(path));
        }
#endif

        // Windows ignores trailing dots in file names, so "speech." means "speech".
        string trimmed = path.TrimEnd('.');

        return System.IO.Path.HasExtension(trimmed) || trimmed.Length == 0 ? path : trimmed + AudioFileExtensions.Wav;
    }
}
