// Ported from MAME src/devices/sound/votrax.h (m_filt_* registers).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

namespace VotraxSpeak.Synthesis;

/// <summary>
/// The parameter values currently committed to the analog filters (MAME's <c>m_filt_*</c>).
/// </summary>
/// <param name="NoiseVolume">Noise amplitude, 4 bits (<c>fa</c>).</param>
/// <param name="NoiseCutoff">Noise routing into F2 versus the second insertion point, 4 bits (<c>fc</c>).</param>
/// <param name="VoiceVolume">Voice amplitude, 4 bits (<c>va</c>).</param>
/// <param name="F1">First formant, 4 bits.</param>
/// <param name="F2">Second formant, 5 bits.</param>
/// <param name="F2Q">Second formant bandwidth, 4 bits.</param>
/// <param name="F3">Third formant, 4 bits.</param>
internal readonly record struct FilterSettings(
    int NoiseVolume,
    int NoiseCutoff,
    int VoiceVolume,
    int F1,
    int F2,
    int F2Q,
    int F3)
{
    /// <summary>Gets a value indicating whether either sound source is audible.</summary>
    public bool IsSounding => NoiseVolume != 0 || VoiceVolume != 0;
}
