namespace VotraxSpeak.Phonetics;

/// <summary>
/// The phoneme set produced by the NRL letter-to-sound rules (an Arpabet variant), plus pauses.
/// </summary>
internal enum ArpabetPhoneme
{
    /// <summary>bEEt</summary>
    IY = 0,

    /// <summary>bIt</summary>
    IH = 1,

    /// <summary>gAte</summary>
    EY = 2,

    /// <summary>gEt</summary>
    EH = 3,

    /// <summary>fAt</summary>
    AE = 4,

    /// <summary>fAther</summary>
    AA = 5,

    /// <summary>lAWn</summary>
    AO = 6,

    /// <summary>lOne</summary>
    OW = 7,

    /// <summary>fUll</summary>
    UH = 8,

    /// <summary>fOOl</summary>
    UW = 9,

    /// <summary>mURdER</summary>
    ER = 10,

    /// <summary>About</summary>
    AX = 11,

    /// <summary>bUt</summary>
    AH = 12,

    /// <summary>hIde</summary>
    AY = 13,

    /// <summary>hOW</summary>
    AW = 14,

    /// <summary>tOY</summary>
    OY = 15,

    /// <summary>Pack</summary>
    P = 16,

    /// <summary>Back</summary>
    B = 17,

    /// <summary>Time</summary>
    T = 18,

    /// <summary>Dime</summary>
    D = 19,

    /// <summary>Coat</summary>
    K = 20,

    /// <summary>Goat</summary>
    G = 21,

    /// <summary>Fault</summary>
    F = 22,

    /// <summary>Vault</summary>
    V = 23,

    /// <summary>eTHer</summary>
    TH = 24,

    /// <summary>eiTHer</summary>
    DH = 25,

    /// <summary>Sue</summary>
    S = 26,

    /// <summary>Zoo</summary>
    Z = 27,

    /// <summary>leaSH</summary>
    SH = 28,

    /// <summary>leiSure</summary>
    ZH = 29,

    /// <summary>How</summary>
    HH = 30,

    /// <summary>suM</summary>
    M = 31,

    /// <summary>suN</summary>
    N = 32,

    /// <summary>suNG</summary>
    NG = 33,

    /// <summary>Laugh</summary>
    L = 34,

    /// <summary>Wear</summary>
    W = 35,

    /// <summary>Young</summary>
    Y = 36,

    /// <summary>Rate</summary>
    R = 37,

    /// <summary>CHar</summary>
    CH = 38,

    /// <summary>Jar</summary>
    JH = 39,

    /// <summary>WHere</summary>
    WH = 40,

    /// <summary>The gap between words.</summary>
    ShortPause = 41,

    /// <summary>The gap at a comma or the end of a sentence.</summary>
    LongPause = 42,
}
