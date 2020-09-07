using System.Diagnostics;
using System.Runtime.InteropServices;
using SramCommons.Models.Structs;

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
    [DebuggerDisplay("{ToString(),nq}")]
    public struct Moneys
    {
        public ThreeByteUint Talons;
        public ThreeByteUint Jewels;
        public ThreeByteUint GoldCoins;
        public ThreeByteUint Credits;

        public override string ToString() => $"T: {Talons} | J: {Jewels} | GC: {GoldCoins} | C: {Credits}";
    }
}