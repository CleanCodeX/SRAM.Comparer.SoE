using System;
using SramComparer.Enums;

namespace SramComparer.SoE.Enums
{
    [Flags]
    public enum ComparisonFlagsSoE
    {
        NonGameBuffer   = ComparisonFlags.NonGameBuffer,
        WholeGameBuffer = ComparisonFlags.WholeGameBuffer,
        GameChecksum        = 1 << 2,
        AllGameChecksums    = 1 << 3 | GameChecksum,
        Unknown12B      = 1 << 4,
        AllUnknown12Bs  = 1 << 5 | Unknown12B,
    }
}