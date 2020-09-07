// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;
using System.Text;
using SramCommons.SoE.Models.Enums;
using SramCommons.Extensions;

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = Sizes.Game.Unknown15)] // 118
    public struct Unknown15
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)] 
        public byte[] Offset0To16;

        public Unknown15_IvoryTowerFlags_Offset17 Offset17; // 1
        public Unknown15_IvoryTowerFlags_Offset18 Offset18; // 1
        public Unknown15_IvoryTowerFlags_Offset19 Offset19; // 1
        public Unknown15_IvoryTowerFlags_Offset20 Offset20; // 1
        public byte Offset21; 
        public byte Offset22; 
        public byte Offset23; 
        public Unknown15_IvoryTowerFlags_Offset24 Offset24; // 1

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 93)]
        public byte[] Offset25To117;

        public string FormatAsString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(nameof(Offset0To16) + ": " + Offset0To16.FormatAsString())
                .AppendLine(nameof(Offset17) + ": " + Offset17)
                .AppendLine(nameof(Offset18) + ": " + Offset18)
                .AppendLine(nameof(Offset19) + ": " + Offset19)
                .AppendLine(nameof(Offset20) + ": " + Offset20)
                .AppendLine(nameof(Offset21) + ": " + Offset21)
                .AppendLine(nameof(Offset22) + ": " + Offset22)
                .AppendLine(nameof(Offset23) + ": " + Offset23)
                .AppendLine(nameof(Offset24) + ": " + Offset24)
                .AppendLine(nameof(Offset25To117) + ": " + Offset25To117.FormatAsString())
                ;

            return sb.ToString();
        }
    }
}