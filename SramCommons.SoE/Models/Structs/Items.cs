using System.Diagnostics;
using System.Runtime.InteropServices;
using SramCommons.Extensions;

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{ToString(),nq}")]
    public struct Items
    {
        // Items
        public byte Petal; // 0 - 6
        public byte Nectar; // 0 - 6
        public byte Honey; // 0 - 6
        public byte DogBiscuit; // 0 - 6
        public byte Wings; // 0 - 6
        public byte Essence; // 0 - 6
        public byte PixieDust; // 0 - 6
        public byte CallBead; // 0 - 99

        public override string ToString() => $@"{nameof(Petal)}: {Petal}
{nameof(Nectar)}: {Nectar}
{nameof(Honey)}: {Honey}
{nameof(DogBiscuit)}: {DogBiscuit}
{nameof(Wings)}: {Wings}
{nameof(Essence)}: {Essence}
{nameof(PixieDust)}: {PixieDust}
{nameof(CallBead)}: {CallBead}
".FormatStruct();
    }
}