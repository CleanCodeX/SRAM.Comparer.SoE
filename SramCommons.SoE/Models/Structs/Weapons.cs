using System.Diagnostics;
using System.Runtime.InteropServices;
using SramCommons.SoE.Models.Enums;

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    [DebuggerDisplay("{ToString(),nq}")]
    public struct Weapons
    {
        [FieldOffset(0)] public ushort Value;

        [FieldOffset(0)] public byte Byte1;
        [FieldOffset(1)] public byte Byte2;

        public Weapon EnumValue
        {
            get => (Weapon)Value;
            set => Value = (ushort)value;
        }

        public override string ToString() => EnumValue.ToString();
    }
}