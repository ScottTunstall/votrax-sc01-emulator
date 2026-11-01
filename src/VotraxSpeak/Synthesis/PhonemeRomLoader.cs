using System;
using System.IO;
using System.Linq;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// Loads a phoneme ROM image from disk and checks that it is a known-good dump.
/// </summary>
internal static class PhonemeRomLoader
{
    /// <summary>
    /// Loads and decodes the ROM image at <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The ROM image file.</param>
    /// <returns>The decoded ROM.</returns>
    /// <exception cref="RomNotFoundException">The file does not exist.</exception>
    /// <exception cref="InvalidRomException">The file cannot be read or is not a known SC-01 ROM dump.</exception>
    public static PhonemeRom Load(string path)
    {
        if (!File.Exists(path))
        {
            throw new RomNotFoundException($"The phoneme ROM '{path}' does not exist.");
        }

        byte[] image = ReadImage(path);
        string fingerprint = RomFingerprint.Of(image);

        if (!KnownRomImages.All.Any(known => string.Equals(known.Sha256, fingerprint, StringComparison.Ordinal)))
        {
            throw new InvalidRomException(
                $"'{path}' is not a known Votrax SC-01 phoneme ROM (SHA-256 {fingerprint}). Use MAME's sc01a.bin or sc01.bin.");
        }

        return new PhonemeRom(image);
    }

    private static byte[] ReadImage(string path)
    {
        try
        {
            return File.ReadAllBytes(path);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            throw new InvalidRomException($"The phoneme ROM '{path}' could not be read: {exception.Message}", exception);
        }
    }
}
