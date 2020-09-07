using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{ToString(),nq}")]
    public struct Ingredients
    {
        public byte Wax;
        public byte Water;
        public byte Vinegar;
        public byte Root;
        public byte Oil;
        public byte Mushroom;
        public byte MudPepper;
        public byte Meteorite;
        public byte Limestone;
        public byte Iron;
        public byte GunPowder;
        public byte Grease;
        public byte Feather;
        public byte Ethanol;
        public byte DryIce;
        public byte Crystal;
        public byte Clay;
        public byte Brimstone;
        public byte Bone;
        public byte AtlasMedallion;
        public byte Ash;
        public byte Acorn;

        public override string ToString() => $@"{nameof(Wax)}: {Wax}
{nameof(Water)}: {Water}
{nameof(Vinegar)}: {Vinegar}
{nameof(Root)}: {Root}
{nameof(Oil)}: {Oil}
{nameof(Mushroom)}: {Mushroom}
{nameof(MudPepper)}: {MudPepper}
{nameof(Meteorite)}: {Meteorite}
{nameof(Limestone)}: {Limestone}
{nameof(Iron)}: {Iron}
{nameof(GunPowder)}: {GunPowder}
{nameof(Grease)}: {Grease}
{nameof(Feather)}: {Feather}
{nameof(Ethanol)}: {Ethanol}
{nameof(DryIce)}: {DryIce}
{nameof(Crystal)}: {Crystal}
{nameof(Clay)}: {Clay}
{nameof(Brimstone)}: {Brimstone}
{nameof(Bone)}: {Bone}
{nameof(AtlasMedallion)}: {AtlasMedallion}
{nameof(Ash)}: {Ash}
{nameof(Acorn)}: {Acorn}
";
    }
}