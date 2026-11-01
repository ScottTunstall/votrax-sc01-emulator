// Ported from MAME src/devices/sound/votrax.cpp.
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

namespace VotraxSpeak.Synthesis;

/// <summary>
/// Clock relationships of the SC-01. All delays are counted in main-clock ticks.
/// </summary>
internal static class ChipTiming
{
    /// <summary>The data sheet's nominal main clock, in hertz.</summary>
    public const int NominalClockHz = 720_000;

    /// <summary>The analog output is sampled at main/18 (40 kHz at the nominal clock).</summary>
    public const int MainClockTicksPerSample = 18;

    /// <summary>The switched-capacitor filters are clocked at main/36 (20 kHz at the nominal clock).</summary>
    public const int MainClockTicksPerCapacitorCycle = 36;

    /// <summary>The digital logic updates once every this many output samples (at main/36).</summary>
    public const int SamplesPerChipUpdate = 2;

    /// <summary>Delay between a phoneme write (strobe) and the chip committing to it.</summary>
    public const int PhoneCommitDelayTicks = 72;

    /// <summary>Every phoneme lasts 16 of its duration ticks.</summary>
    public const int TicksPerPhone = 16;

    /// <summary>Duration ticks are measured in units of main/144, i.e. 4 chip updates per unit.</summary>
    public const int ChipUpdatesPerDurationUnit = 4;

    /// <summary>The phone tick comparator runs one chip update late.</summary>
    public const int PhoneTickComparatorDelay = 1;

    /// <summary>Extra main-clock ticks before the end-of-phone request is raised.</summary>
    public const int EndOfPhoneExtraTicks = 2;
}
