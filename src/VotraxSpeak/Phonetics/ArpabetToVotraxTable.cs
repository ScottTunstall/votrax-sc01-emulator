using System.Collections.Generic;
using VotraxSpeak.Synthesis;
using static VotraxSpeak.Synthesis.VotraxPhoneme;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// The Votrax SC-01 phonemes used to say each rule phoneme. The SC-01 has several lengths of most
/// vowels; the mid-length ones are used because the rules carry no stress information.
/// Diphthongs and affricates take several chip phonemes.
/// </summary>
internal static class ArpabetToVotraxTable
{
    /// <summary>Gets the chip phonemes for each rule phoneme.</summary>
    public static IReadOnlyDictionary<ArpabetPhoneme, IReadOnlyList<VotraxPhoneme>> Entries { get; } =
        new Dictionary<ArpabetPhoneme, IReadOnlyList<VotraxPhoneme>>
        {
            [ArpabetPhoneme.IY] = [E1],
            [ArpabetPhoneme.IH] = [I1],
            [ArpabetPhoneme.EY] = [A1, AY, Y],
            [ArpabetPhoneme.EH] = [EH1],
            [ArpabetPhoneme.AE] = [AE1],
            [ArpabetPhoneme.AA] = [AH1],
            [ArpabetPhoneme.AO] = [AW1],
            [ArpabetPhoneme.OW] = [O],
            [ArpabetPhoneme.UH] = [OO1],
            [ArpabetPhoneme.UW] = [U],
            [ArpabetPhoneme.ER] = [ER],
            [ArpabetPhoneme.AX] = [UH2],
            [ArpabetPhoneme.AH] = [UH1],
            [ArpabetPhoneme.AY] = [AH1, EH3, I3],
            [ArpabetPhoneme.AW] = [AH1, U1],
            [ArpabetPhoneme.OY] = [O1, I3],
            [ArpabetPhoneme.P] = [P],
            [ArpabetPhoneme.B] = [B],
            [ArpabetPhoneme.T] = [T],
            [ArpabetPhoneme.D] = [D],
            [ArpabetPhoneme.K] = [K],
            [ArpabetPhoneme.G] = [G],
            [ArpabetPhoneme.F] = [F],
            [ArpabetPhoneme.V] = [V],
            [ArpabetPhoneme.TH] = [TH],
            [ArpabetPhoneme.DH] = [THV],
            [ArpabetPhoneme.S] = [S],
            [ArpabetPhoneme.Z] = [Z],
            [ArpabetPhoneme.SH] = [SH],
            [ArpabetPhoneme.ZH] = [ZH],
            [ArpabetPhoneme.HH] = [H],
            [ArpabetPhoneme.M] = [M],
            [ArpabetPhoneme.N] = [N],
            [ArpabetPhoneme.NG] = [NG],
            [ArpabetPhoneme.L] = [L],
            [ArpabetPhoneme.W] = [W],
            [ArpabetPhoneme.Y] = [Y1],
            [ArpabetPhoneme.R] = [R],
            [ArpabetPhoneme.CH] = [T, CH],
            [ArpabetPhoneme.JH] = [D, J],
            [ArpabetPhoneme.WH] = [H, W],
            [ArpabetPhoneme.ShortPause] = [PA0],
            [ArpabetPhoneme.LongPause] = [PA1],
        };
}
