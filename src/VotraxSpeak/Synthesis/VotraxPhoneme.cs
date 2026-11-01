// Ported from MAME src/devices/sound/votrax.cpp (s_phone_table).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

namespace VotraxSpeak.Synthesis;

/// <summary>
/// The 64 phonemes of the Votrax SC-01. Each value is the 6-bit code written to the chip.
/// The example words come from the SC-01 data sheet.
/// </summary>
internal enum VotraxPhoneme
{
    /// <summary>jackEt</summary>
    EH3 = 0x00,

    /// <summary>Enlist</summary>
    EH2 = 0x01,

    /// <summary>hEAvy</summary>
    EH1 = 0x02,

    /// <summary>Short pause (no sound).</summary>
    PA0 = 0x03,

    /// <summary>buTTer</summary>
    DT = 0x04,

    /// <summary>mAde</summary>
    A1 = 0x05,

    /// <summary>mAde</summary>
    A2 = 0x06,

    /// <summary>aZure</summary>
    ZH = 0x07,

    /// <summary>hOnest</summary>
    AH2 = 0x08,

    /// <summary>inhibIt</summary>
    I3 = 0x09,

    /// <summary>Inhibit</summary>
    I2 = 0x0A,

    /// <summary>inhIbit</summary>
    I1 = 0x0B,

    /// <summary>Mat</summary>
    M = 0x0C,

    /// <summary>suN</summary>
    N = 0x0D,

    /// <summary>Bag</summary>
    B = 0x0E,

    /// <summary>Van</summary>
    V = 0x0F,

    /// <summary>CHip</summary>
    CH = 0x10,

    /// <summary>SHop</summary>
    SH = 0x11,

    /// <summary>Zoo</summary>
    Z = 0x12,

    /// <summary>lAWful</summary>
    AW1 = 0x13,

    /// <summary>thiNG</summary>
    NG = 0x14,

    /// <summary>fAther</summary>
    AH1 = 0x15,

    /// <summary>lOOking</summary>
    OO1 = 0x16,

    /// <summary>bOOk</summary>
    OO = 0x17,

    /// <summary>Land</summary>
    L = 0x18,

    /// <summary>triCK</summary>
    K = 0x19,

    /// <summary>Judge</summary>
    J = 0x1A,

    /// <summary>Hello</summary>
    H = 0x1B,

    /// <summary>Get</summary>
    G = 0x1C,

    /// <summary>Fast</summary>
    F = 0x1D,

    /// <summary>paiD</summary>
    D = 0x1E,

    /// <summary>paSS</summary>
    S = 0x1F,

    /// <summary>dAy</summary>
    A = 0x20,

    /// <summary>daY</summary>
    AY = 0x21,

    /// <summary>Yard</summary>
    Y1 = 0x22,

    /// <summary>missIOn</summary>
    UH3 = 0x23,

    /// <summary>mOp</summary>
    AH = 0x24,

    /// <summary>Past</summary>
    P = 0x25,

    /// <summary>cOld</summary>
    O = 0x26,

    /// <summary>pIn</summary>
    I = 0x27,

    /// <summary>mOve</summary>
    U = 0x28,

    /// <summary>anY</summary>
    Y = 0x29,

    /// <summary>Tap</summary>
    T = 0x2A,

    /// <summary>Red</summary>
    R = 0x2B,

    /// <summary>mEEt</summary>
    E = 0x2C,

    /// <summary>Win</summary>
    W = 0x2D,

    /// <summary>dAd</summary>
    AE = 0x2E,

    /// <summary>After</summary>
    AE1 = 0x2F,

    /// <summary>sAlty</summary>
    AW2 = 0x30,

    /// <summary>About</summary>
    UH2 = 0x31,

    /// <summary>Uncle</summary>
    UH1 = 0x32,

    /// <summary>cUp</summary>
    UH = 0x33,

    /// <summary>fOr</summary>
    O2 = 0x34,

    /// <summary>abOArd</summary>
    O1 = 0x35,

    /// <summary>yOU</summary>
    IU = 0x36,

    /// <summary>yOU</summary>
    U1 = 0x37,

    /// <summary>THe</summary>
    THV = 0x38,

    /// <summary>THin</summary>
    TH = 0x39,

    /// <summary>bIRd</summary>
    ER = 0x3A,

    /// <summary>gEt</summary>
    EH = 0x3B,

    /// <summary>bE</summary>
    E1 = 0x3C,

    /// <summary>cAll</summary>
    AW = 0x3D,

    /// <summary>Long pause (no sound).</summary>
    PA1 = 0x3E,

    /// <summary>Stop (no sound). The chip powers up on this phoneme.</summary>
    Stop = 0x3F,
}
