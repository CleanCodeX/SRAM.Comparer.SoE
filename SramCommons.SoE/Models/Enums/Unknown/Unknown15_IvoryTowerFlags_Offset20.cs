using System;

// ReSharper disable InconsistentNaming

namespace SramCommons.SoE.Models.Enums
{
    [Flags]
    public enum Unknown15_IvoryTowerFlags_Offset20 : byte
    {
        Level4_House1_Floor2_RightChest__6xAsh = 0b0000_0001, // bit 1 (no U12C flag)
        Level4_House1_Floor1_RightChest__1xLimestone = 0b0000_0010, // bit 2
        Level3_House3_Floor1_RearEntry_LeftChest__AnnihilationAmulet = 0b0000_0100, // bit 3  (U12C flag 0000-b1)
        Level3_House3_Floor1_RearEntry_CenterChest__100GM = 0b0000_1000, // bit 4

        //Level3_House3_Floor1_RearEntry_RightChest__3xAcorns = 0b0000_1000, // bit 4 or 5? // Needs to be proofed again
    }
}