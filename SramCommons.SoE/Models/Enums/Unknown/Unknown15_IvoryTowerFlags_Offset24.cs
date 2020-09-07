using System;

// ReSharper disable InconsistentNaming

namespace SramCommons.SoE.Models.Enums
{
    [Flags]
    public enum Unknown15_IvoryTowerFlags_Offset24 : byte
    {
        SouthernArea_NorthWestChest__AnnihilationAmulet = 0b0000_0001, // bit 1  (no U12C flag, sets U13{O1:b2 & O3:b2})
        SouthernArea_SouthWestChest__5xAsh = 0b0000_0010, // bit 2  (no U12C flag)
        SouthernArea_SouthMostChest__AnnihilationAmulet = 0b0000_0100, // bit 3  (no U12C flag)
    }
}