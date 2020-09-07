using System;

// ReSharper disable InconsistentNaming

namespace SramCommons.SoE.Models.Enums
{
    [Flags]
    public enum Unknown15_IvoryTowerFlags_Offset19 : byte
    {
        Level3_House2_Floor1__2xPedals = 0b0000_0001, // bit 1
        Level3_House3_Floor2_LeftChest__GoldVest = 0b0000_0010, // bit 2
        Level3_House3_Floor2_RightChest__AnnihilationAmulet = 0b0000_0100, // bit 3 (no U12C flag)
        Level3_House3_Floor1__4xNectar = 0b0000_1000, // bit 4 (no U12C flag)

        Level4_House1_Floor1_LeftChest__6xAsh = 0b0001_0000, // bit 5 (no U12C flag)
        Level4_House1_Floor1_ReadEntry_LeftChest__DogBuscuit = 0b0010_0000, // bit 6 (no U12C flag)
        Level4_House1_Floor1_ReadEntry_RightChest__AnnihilationAmulet = 0b0100_0000, // bit 7 (no U12C flag)
        Level4_House1_Floor2_LeftChest__3xEthanol = 0b1000_0000, // bit 8 (no U12C flag)
    }
}