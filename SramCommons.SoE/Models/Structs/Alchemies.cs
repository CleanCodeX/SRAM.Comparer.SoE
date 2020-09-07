using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SramCommons.SoE.Models.Enums;
using SramCommons.Attributes;

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{ToString(),nq}")]
    [NoByteReordering]
    public struct Alchemies
    {
        public byte Byte1;
        public byte Byte2;
        public byte Byte3;
        public byte Byte4;
        public byte Byte5;

        public ulong Value5Byte => BitConverter.ToUInt64(new[] { Byte1, Byte2, Byte3, Byte4, Byte5, byte.MinValue, byte.MinValue, byte.MinValue });

        public Alchemy EnumValue
        {
            get => (Alchemy)Value5Byte;
            set
            {
                var bytes = BitConverter.GetBytes((ulong) value);

                (Byte1, Byte2, Byte3, Byte4, Byte5) = (bytes[0], bytes[1], bytes[2], bytes[3], bytes[4]);
            }
        }

        public override string ToString() => EnumValue.ToString();
    }
}