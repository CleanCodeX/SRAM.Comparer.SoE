using System;

// ReSharper disable InconsistentNaming

namespace SramCommons.SoE.Models.Enums
{
    [Flags]
    public enum Unknown15_IvoryTowerFlags_Offset17 : byte
    {
        Level1_House1_Floor0_LeftUpperChest_20GM = 0b0010_0000, // bit 6 (no U12C flag)
        Level1_House1_Floor0_LeftFrontChest_2xAcorns = 0b0100_0000, // bit 7 (U12C flag b1)
    }
}