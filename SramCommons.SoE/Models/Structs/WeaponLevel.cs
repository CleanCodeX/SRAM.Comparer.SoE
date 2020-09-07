using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{ToString(),nq}")]
    public struct WeaponLevel
    {
        public byte Minor; // 0-255
        public byte Major; // 1-3
        
        public override string ToString() => $"{Major}.{Minor}";
    }
}