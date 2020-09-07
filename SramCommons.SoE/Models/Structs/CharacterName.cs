//#define EXPLICIT

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SramCommons.SoE.Models.Structs
{
#if EXPLICIT
    [StructLayout(LayoutKind.Explicit, Pack = 1, CharSet = CharSet.Ansi, Size = Sizes.Game.BoyName)]
#else
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
#endif

    [DebuggerDisplay("{ToString(),nq}")]
    public unsafe struct CharacterName
    {
#if EXPLICIT
        [FieldOffset(0)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 34)]
        public byte[] BytesValue; // (34 Bytes) Null terminated

#if EXPLICIT
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17, ArraySubType = UnmanagedType.ByValTStr)]
        public char[] NameCharacters; // (34 Bytes) Null terminated
#endif

        public string StringValue
        {
            get
            {
                const int length = 32;
                var sb = new StringBuilder(length);
                fixed (byte* pointer = BytesValue)
                {
                    for (var i = 0; i < length; ++i)
                    {
                        var value = (char)*(pointer + i);
                        if (value == 0 || value == 96) continue;

                        sb.Append(value);
                    }
                }

                var result = sb.ToString();
                return result;
            }
            set
            {
                // TODO Check for correctness
                //BytesValue = Encoding.ASCII.GetBytes(value + (char) 0);
            }
        }

        public override string ToString() => StringValue;
    }
}