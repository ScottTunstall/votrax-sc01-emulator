namespace VotraxSpeak.Speech;

/// <summary>
/// The allowed ranges and defaults of the voice controls.
/// </summary>
internal static class VoiceLimits
{
    /// <summary>The slowest speed, as a percentage of the nominal clock.</summary>
    public const int MinimumSpeedPercent = 25;

    /// <summary>The fastest speed, as a percentage of the nominal clock.</summary>
    public const int MaximumSpeedPercent = 400;

    /// <summary>The nominal speed.</summary>
    public const int DefaultSpeedPercent = 100;

    /// <summary>The lowest inflection (pitch) level.</summary>
    public const int MinimumPitch = 0;

    /// <summary>The highest inflection (pitch) level.</summary>
    public const int MaximumPitch = 3;

    /// <summary>The inflection level the chip resets to.</summary>
    public const int DefaultPitch = 0;

    /// <summary>Speeds are percentages.</summary>
    public const int PercentScale = 100;
}
