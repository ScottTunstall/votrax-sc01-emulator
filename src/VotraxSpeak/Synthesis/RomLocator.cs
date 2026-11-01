using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// Decides which phoneme ROM file to use.
/// </summary>
internal sealed class RomLocator
{
    private readonly IReadOnlyList<string> _searchDirectories;

    /// <summary>
    /// Initializes a new instance of the <see cref="RomLocator"/> class.
    /// </summary>
    /// <param name="searchDirectories">Directories searched, in order, when no ROM path is given.</param>
    public RomLocator(IReadOnlyList<string> searchDirectories)
    {
        _searchDirectories = searchDirectories;
    }

    /// <summary>
    /// Returns the explicitly requested ROM path, or else the first known ROM file found in the
    /// search directories.
    /// </summary>
    /// <param name="requestedPath">The path the user asked for, if any.</param>
    /// <returns>The path of the ROM to load.</returns>
    /// <exception cref="RomNotFoundException">No path was requested and no known ROM file was found.</exception>
    public string Locate(string? requestedPath)
    {
        if (requestedPath is not null)
        {
            return requestedPath;
        }

        IEnumerable<string> candidates =
            from directory in _searchDirectories
            from image in KnownRomImages.All
            select Path.Combine(directory, image.FileName);

        return candidates.FirstOrDefault(File.Exists)
            ?? throw new RomNotFoundException(
                "No Votrax SC-01 phoneme ROM was found. Copy MAME's sc01a.bin or sc01.bin (from votrsc01a.zip or votrsc01.zip) "
                + "next to the application, or pass its location with --rom. Searched: "
                + string.Join(", ", _searchDirectories.Distinct()));
    }
}
