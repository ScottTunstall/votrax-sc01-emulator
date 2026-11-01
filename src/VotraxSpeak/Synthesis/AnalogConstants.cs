// Ported from MAME src/devices/sound/votrax.cpp.
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

namespace VotraxSpeak.Synthesis;

/// <summary>
/// Scaling values used by the SC-01's analog signal path.
/// </summary>
internal static class AnalogConstants
{
    /// <summary>Full scale of the 4-bit amplitude and filter parameters.</summary>
    public const double ParameterFullScale = 15.0;

    /// <summary>All four bits of a 4-bit parameter set; used to complement the noise routing.</summary>
    public const int ParameterAllBits = 15;

    /// <summary>Full scale of the 3-bit glottal closure attenuation.</summary>
    public const double ClosureFullScale = 7.0;

    /// <summary>All three bits of the closure level set; the attenuation is the complement of the level.</summary>
    public const int ClosureAllBits = 7;

    /// <summary>The pitch counter holds each glottal wave entry for eight counts (pitch &gt;&gt; 3).</summary>
    public const int GlottalStepShift = 3;

    /// <summary>Noise only passes while this bit of the pitch counter is set.</summary>
    public const int NoisePitchGateBit = 0x40;

    /// <summary>Base intensity of the white noise source, checked against SC-01-A recordings.</summary>
    public const double NoiseAmplitude = 5e3;

    /// <summary>Final output scaling.</summary>
    public const double OutputGain = 0.35;

    /// <summary>
    /// The final low-pass capacitors put its cutoff near 150 Hz, but recordings show it is closer
    /// to 4 kHz, so MAME scales the time constant by this factor.
    /// </summary>
    public const double LowpassCutoffCorrection = 150.0 / 4000.0;
}
