using System;
using System.IO;

namespace VotraxSpeak.Tests;

/// <summary>
/// A new, empty directory that is deleted when disposed.
/// </summary>
internal sealed class TemporaryDirectory : IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemporaryDirectory"/> class.
    /// </summary>
    public TemporaryDirectory()
    {
        Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "VotraxSpeak.Tests", Guid.NewGuid().ToString("N"));
        _ = Directory.CreateDirectory(Path);
    }

    /// <summary>Gets the directory's full path.</summary>
    public string Path { get; }

    /// <summary>
    /// Returns the full path of a file in the directory.
    /// </summary>
    /// <param name="name">The file name.</param>
    /// <returns>The path.</returns>
    public string PathOf(string name) => System.IO.Path.Combine(Path, name);

    /// <inheritdoc/>
    public void Dispose() => Directory.Delete(Path, recursive: true);
}
