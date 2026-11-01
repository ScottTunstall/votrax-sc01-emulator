// Ported from MAME src/devices/sound/votrax.cpp (inputs of analog_calc).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

namespace VotraxSpeak.Synthesis;

/// <summary>
/// The digital signals that drive the analog path for one sample.
/// </summary>
/// <param name="Pitch">The 8-bit pitch counter, which selects the glottal wave step and gates the noise.</param>
/// <param name="Noise">The current output bit of the noise shift register.</param>
/// <param name="Closure">The glottal closure counter.</param>
/// <param name="Filters">The parameters committed to the filters.</param>
internal readonly record struct AnalogInputs(int Pitch, bool Noise, int Closure, FilterSettings Filters);
