using System.Diagnostics;
using System.Runtime.InteropServices;
using SramCommons.Extensions;

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{ToString(),nq}")]
    public struct BazookaAmmunitions
    {
        public byte ThunderBall; // 0 - 99
        public byte ParticleBomb; // 0 - 99
        public byte CryoBlast; // 0 - 99

        public override string ToString() => $@"{nameof(ThunderBall)}: {ThunderBall}
{nameof(ParticleBomb)}: {ParticleBomb}
{nameof(CryoBlast)}: {CryoBlast}
".FormatStruct();
    }
}