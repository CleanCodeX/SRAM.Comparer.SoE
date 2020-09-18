using System.Diagnostics.CodeAnalysis;
using SramComparer.Enums;

namespace SramComparer.SoE.Enums
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Ausstehend>")]
    public enum Commands
    {
        cmd = BaseCommands.cmd,
        s = BaseCommands.s,
        m = BaseCommands.m,
        fwg = BaseCommands.fwg,
        fng = BaseCommands.fng,
        sg = BaseCommands.sg,
        sgc = BaseCommands.sgc,
        c = BaseCommands.c,
        ow = BaseCommands.ow,
        b = BaseCommands.b,
        bc = BaseCommands.bc,
        r = BaseCommands.r,
        rc = BaseCommands.rc,
        e = BaseCommands.e,
        ts = BaseCommands.ts,
        w = BaseCommands.w,
        q = BaseCommands.q,
        fu12b,
        fu12ba,
        fc,
        fca,
    }
}