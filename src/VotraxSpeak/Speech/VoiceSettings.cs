using System;
using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Speech;

/// <summary>
/// The SC-01's two voice controls: its clock speed and its inflection input.
/// </summary>
internal sealed class VoiceSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VoiceSettings"/> class.
    /// </summary>
    /// <param name="speedPercent">
    /// The chip clock as a percentage of the nominal 720 kHz. As on the real chip, a faster clock
    /// raises the pitch along with the speed.
    /// </param>
    /// <param name="pitch">The inflection level, 0 (lowest) to 3 (highest).</param>
    /// <exception cref="ArgumentOutOfRangeException">A value is outside the ranges in <see cref="VoiceLimits"/>.</exception>
    public VoiceSettings(int speedPercent, int pitch)
    {
        if (speedPercent is < VoiceLimits.MinimumSpeedPercent or > VoiceLimits.MaximumSpeedPercent)
        {
            throw new ArgumentOutOfRangeException(nameof(speedPercent), speedPercent, "Speed is outside the supported range.");
        }

        if (pitch is < VoiceLimits.MinimumPitch or > VoiceLimits.MaximumPitch)
        {
            throw new ArgumentOutOfRangeException(nameof(pitch), pitch, "Pitch is outside the supported range.");
        }

        SpeedPercent = speedPercent;
        Pitch = pitch;
    }

    /// <summary>Gets the default settings: nominal clock, lowest inflection.</summary>
    public static VoiceSettings Default { get; } = new(VoiceLimits.DefaultSpeedPercent, VoiceLimits.DefaultPitch);

    /// <summary>Gets the chip clock as a percentage of the nominal clock.</summary>
    public int SpeedPercent { get; }

    /// <summary>Gets the inflection level.</summary>
    public int Pitch { get; }

    /// <summary>Gets the chip clock these settings select, in hertz.</summary>
    public int ClockHz => ChipTiming.NominalClockHz / VoiceLimits.PercentScale * SpeedPercent;
}
