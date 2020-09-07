using System;

namespace SramComparer.SoE
{
    [Flags]
    public enum ComparisonFlags
    {
        NonGameBuffer   = 1 << 0,
        WholeGameBuffer = 1 << 1,
        GameChecksum        = 1 << 2,
        AllGameChecksums    = 1 << 3 | GameChecksum,
        Unknown12B      = 1 << 4,
        AllUnknown12Bs  = 1 << 5 | Unknown12B,
    }
}