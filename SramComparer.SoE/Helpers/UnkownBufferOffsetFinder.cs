using System.Diagnostics.CodeAnalysis;
using Commons.Extensions;
using SramCommons.SoE.Models;
// ReSharper disable InconsistentNaming

namespace SramComparer.SoE.Helpers
{
    internal static class UnkownBufferOffsetFinder
    {
        public static int GetGameBufferOffset(string bufferName) => (int)bufferName.ParseEnum<GameUnknownOffset>();
        public static int GetSramBufferOffset(string bufferName) => Offsets.SramUnknown1;

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        private enum GameUnknownOffset
        {
            Unknown1 = Offsets.Game.Unknown1, // 38 // 0
            Unknown2 = Offsets.Game.Unknown2, // Offset 72
            Unknown3 = Offsets.Game.Unknown3, // Offset 108
            
            Unknown4_BoyBuff = Offsets.Game.Unknown4, // Offset 112
            Unknown4_BoyBuff__BuffFlags = Unknown4_BoyBuff, // Offset 112
            Unknown4_BoyBuff__Unknown1 = Unknown4_BoyBuff + 2, // Offset 114
            Unknown4_BoyBuff__Unknown2 = Unknown4_BoyBuff + 4, // Offset 116
            Unknown4_BoyBuff__Unknown3 = Unknown4_BoyBuff + 28, // Offset 138
            Unknown4_BoyBuff__Unknown4 = Unknown4_BoyBuff + 30, // Offset 140
            
            Unknown5 = Offsets.Game.Unknown5, // Offset 144 
            Unknown6 = Offsets.Game.Unknown6, // Offset 159
            
            Unknown7_DogBuff = Offsets.Game.Unknown7, // Offset 177
            Unknown7_DogBuff__BuffFlags = Unknown7_DogBuff, // Offset 177
            Unknown7_DogBuff__Unknown1 = Unknown7_DogBuff + 2, // Offset 179
            Unknown7_DogBuff__Unknown2 = Unknown7_DogBuff + 4, // Offset 181
            Unknown7_DogBuff__Unknown3 = Unknown7_DogBuff + 28, // Offset 203
            Unknown7_DogBuff__Unknown7 = Unknown7_DogBuff + 30, // Offset 205

            Unknown8 = Offsets.Game.Unknown8, // Offset 209
            Unknown9 = Offsets.Game.Unknown9, // Offset 224
            Unknown10 = Offsets.Game.Unknown10, // Offset 264
            Unknown11 = Offsets.Game.Unknown11, // Offset 303
            Unknown12A = Offsets.Game.Unknown12A, // Offset 319
            Unknown12B = Offsets.Game.Unknown12B, // Offset 335
            Unknown12C = Offsets.Game.Unknown12C, // Offset 337
            Unknown13 = Offsets.Game.Unknown13, // Offset 481
            Unknown14_AntiquaFlags = Offsets.Game.Unknown14_AntiquaFlags, // Offset 508

            Unknown15 = Offsets.Game.Unknown15, // Offset 515
            Unknown15_Offset0To16 = Unknown15, // Offset 515
            Unknown15_Offset17 = Unknown15 + 17, // 532
            Unknown15_Offset18 = Unknown15 + 18, // Offset 533
            Unknown15_Offset19 = Unknown15 + 19, // Offset 534
            Unknown15_Offset20 = Unknown15 + 20, // Offset 535
            Unknown15_Offset21 = Unknown15 + 21, // Offset 536
            Unknown15_Offset22 = Unknown15 + 22, // Offset 537
            Unknown15_Offset23 = Unknown15 + 23, // Offset 538
            Unknown15_Offset24 = Unknown15 + 24, // Offset 539
            Unknown15_Offset25To117 = Unknown15 + 25, // Offset 540

            Unknown16A = Offsets.Game.Unknown16A, // Offset 635
            Unknown16B_GoticaFlags = Offsets.Game.Unknown16B_GoticaFlags, // Offset 639
            Unknown16C = Offsets.Game.Unknown16C, // Offset 643
            Unknown17 = Offsets.Game.Unknown17, // Offset 722
            Unknown18 = Offsets.Game.Unknown18 // Offset 802
        }
    }
}
