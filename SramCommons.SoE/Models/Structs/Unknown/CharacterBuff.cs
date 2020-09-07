using System;
using System.Runtime.InteropServices;
using System.Text;
using SramCommons.Extensions;

// ReSharper disable InconsistentNaming

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CharacterBuff
    {
        [Flags]
        public enum BuffFlags_Offset0To1 : ushort
        {
            PixieDust = 0b0101_0000__1000_0000, // offset 0: bit 5 + 7, offset 1: bit 8
        }

        public BuffFlags_Offset0To1 BuffFlags;
        public ushort Unknown1; // Duration?
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public byte[] Unknown2;
        public ushort Unknown3;
        public ushort Unknown4;

        public string FormatAsString()
        {
            var sb = new StringBuilder(5);

            sb.AppendLine(nameof(BuffFlags) + ": " + BuffFlags)
                .AppendLine(nameof(Unknown1) + ": " + Unknown1)
                .AppendLine(nameof(Unknown2) + ": " + Unknown2.FormatAsString())
                .AppendLine(nameof(Unknown3) + ": " + Unknown3)
                .AppendLine(nameof(Unknown4) + ": " + Unknown4);

            return sb.ToString();
        }
    }
}