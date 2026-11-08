using System;
using System.IO;

namespace VotraxSpeak.Audio;

/// <summary>
/// Writes speech to an Ogg Vorbis file, replacing any existing file.
/// </summary>
internal sealed class OggFileOutput : IAudioOutput
{
    private readonly string _path;

    /// <summary>
    /// Initializes a new instance of the <see cref="OggFileOutput"/> class.
    /// </summary>
    /// <param name="path">The Ogg file to write.</param>
    /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
    public OggFileOutput(string path)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"><paramref name="clip"/> is <see langword="null"/>.</exception>
    public void Deliver(AudioClip clip) => File.WriteAllBytes(_path, OggVorbisFileEncoder.Encode(clip));
}
