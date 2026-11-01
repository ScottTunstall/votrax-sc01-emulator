using System.Globalization;
using System.Linq;
using System.Security.Cryptography;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// Identifies ROM images by their SHA-256 hash.
/// </summary>
internal static class RomFingerprint
{
    /// <summary>
    /// Returns the lower-case hexadecimal SHA-256 hash of <paramref name="image"/>.
    /// </summary>
    /// <param name="image">The ROM image.</param>
    /// <returns>The hash, as 64 hexadecimal digits.</returns>
    public static string Of(byte[] image)
    {
#if NET
        byte[] hash = SHA256.HashData(image);
#else
        using var algorithm = SHA256.Create();
        byte[] hash = algorithm.ComputeHash(image);
#endif
        return string.Concat(hash.Select(b => b.ToString("x2", CultureInfo.InvariantCulture)));
    }
}
