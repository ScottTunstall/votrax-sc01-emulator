// Ported from MAME src/devices/sound/votrax.h (m_rom_* values).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

namespace VotraxSpeak.Synthesis;

/// <summary>
/// The parameters the SC-01's internal ROM stores for one phoneme.
/// </summary>
/// <param name="Duration">Length of one of the phoneme's 16 ticks, in units of main/144 (7 bits).</param>
/// <param name="VoiceDelay">
/// Tick at which the noise amplitude and routing start moving and the closure latches (MAME's <c>vd</c>, 4 bits).
/// </param>
/// <param name="ClosureDelay">Tick at which the voice amplitude starts moving (MAME's <c>cld</c>, 4 bits).</param>
/// <param name="NoiseVolume">Target noise amplitude (<c>fa</c>, 4 bits).</param>
/// <param name="NoiseCutoff">Target noise routing (<c>fc</c>, 4 bits).</param>
/// <param name="VoiceVolume">Target voice amplitude (<c>va</c>, 4 bits).</param>
/// <param name="F1">Target first formant (4 bits).</param>
/// <param name="F2">Target second formant (4 bits).</param>
/// <param name="F2Q">Target second formant bandwidth (4 bits).</param>
/// <param name="F3">Target third formant (4 bits).</param>
/// <param name="HasClosure">Whether the phoneme silences the output at its closure point (a stop consonant).</param>
/// <param name="IsPause">Whether this is a pause phoneme; this is wired on the die, not stored in the ROM.</param>
internal sealed record PhonemeParameters(
    int Duration,
    int VoiceDelay,
    int ClosureDelay,
    int NoiseVolume,
    int NoiseCutoff,
    int VoiceVolume,
    int F1,
    int F2,
    int F2Q,
    int F3,
    bool HasClosure,
    bool IsPause);
