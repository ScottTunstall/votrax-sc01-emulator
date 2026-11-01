using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Tests;

/// <summary>
/// Phoneme parameters shared by the synthesis tests.
/// </summary>
internal static class TestPhonemes
{
    /// <summary>The duration used by <see cref="LoudVowel"/>.</summary>
    public const int VowelDuration = 10;

    /// <summary>Gets a loud voiced phoneme with no noise and no closure.</summary>
    public static PhonemeParameters LoudVowel { get; } = new(
        Duration: VowelDuration,
        VoiceDelay: 1,
        ClosureDelay: 1,
        NoiseVolume: 0,
        NoiseCutoff: 0,
        VoiceVolume: 15,
        F1: 5,
        F2: 9,
        F2Q: 3,
        F3: 7,
        HasClosure: false,
        IsPause: false);
}
