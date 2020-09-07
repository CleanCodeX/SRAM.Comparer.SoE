using System;

// ReSharper disable InconsistentNaming

namespace SramCommons.SoE.Models.Enums
{
    [Flags]
    public enum Unknown15_IvoryTowerFlags_Offset18 : byte
    {
        Level2_House1_Floor1_RearChest__4xClay = 0b0000_1000, // bit 4 (no U12C flag)

        Level2_House1_Floor1_FrontChest__1xMushroom = 0b0001_0000, // bit 5 (no U12C flag)
        Level3_House1_Inn_Floor0__3xBrimstones = 0b0010_0000, // bit 6
        Level3_House2_Floor1_LeftChest__6xWater = 0b0100_0000, // bit 7 (U12C flag b1)
        Level3_House2_Floor1_RightChest__4xVinegar = 0b1000_0000, // bit 8
    }
}