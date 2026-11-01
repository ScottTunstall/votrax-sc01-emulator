using System;
using System.Linq;

namespace VotraxSpeak.Audio;

/// <summary>
/// Scales audio so its loudest sample sits at <see cref="AudioLevels.NormalizedPeak"/>. The
/// emulation's raw output level is arbitrary, so this gives a consistent, comfortable volume.
/// </summary>
internal static class PeakNormalizer
{
    /// <summary>
    /// Returns <paramref name="clip"/> scaled so its peak is <see cref="AudioLevels.NormalizedPeak"/>.
    /// </summary>
    /// <param name="clip">The audio to scale.</param>
    /// <returns>The scaled audio; silence is returned unchanged.</returns>
    public static AudioClip Normalize(AudioClip clip)
    {
        float peak = clip.Samples.Select(Math.Abs).DefaultIfEmpty(0f).Max();

        if (peak == 0f)
        {
            return clip;
        }

        float gain = AudioLevels.NormalizedPeak / peak;
        return new AudioClip(clip.SampleRateHz, clip.Samples.Select(sample => sample * gain));
    }
}
