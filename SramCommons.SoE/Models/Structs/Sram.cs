//#define EXPLICIT

using System.Runtime.InteropServices;

namespace SramCommons.SoE.Models.Structs
{
#if EXPLICIT
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = Sizes.Sram)]
#else
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
#endif
    public struct Sram
    {
#if EXPLICIT
        [FieldOffset(Offsets.SramChecksum)] 
#endif
        public ushort Checksum; // Offset 0 (2 Bytes)

#if EXPLICIT
        [FieldOffset(2)] 
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] 
        public SramGame[] Game; // Offset 2 (3268 = 4* 817 Bytes)

#if EXPLICIT
        [FieldOffset(Offsets.FirstGame + Sizes.Game.All * 4)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.SramUnknown1)]
        public byte[] Unknown1; // Offset 3270 (4922 Bytes)
    }
}