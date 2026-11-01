using System;

namespace VotraxSpeak.Audio;

/// <summary>
/// Plays speech through the default Windows audio device and waits for it to finish.
/// </summary>
internal sealed class SpeakerOutput : IAudioOutput
{
    /// <inheritdoc/>
    /// <exception cref="AudioPlaybackException">The sound could not be played.</exception>
    public void Deliver(AudioClip clip)
    {
        byte[] wave = WaveFileEncoder.Encode(clip);
        bool played;

        try
        {
            played = NativeMethods.PlaySound(wave, IntPtr.Zero, PlaySoundOptions.Memory | PlaySoundOptions.NoDefault);
        }
        catch (Exception exception) when (exception is DllNotFoundException or EntryPointNotFoundException)
        {
            throw new AudioPlaybackException(
                "Playing speech needs Windows audio (winmm.dll). Use -f to write a WAV file instead.",
                exception);
        }

        if (!played)
        {
            throw new AudioPlaybackException("Windows could not play the speech. Check that an audio output device is available.");
        }
    }
}
