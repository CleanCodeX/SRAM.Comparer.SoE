using System.Diagnostics.CodeAnalysis;
using Common.Shared.Min.Attributes;
using SramComparer.Enums;
using SramComparer.SoE.Properties;

namespace SramComparer.SoE.Enums
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Ausstehend>")]
	public enum CommandsSoE
	{
		cmd = Commands.cmd,
		s = Commands.s,
		m = Commands.m,
		asbc = Commands.asbc,
		nsbc = Commands.nsbc,
		ss = Commands.ss,
		ssc = Commands.ssc,
		c = Commands.c,
		ow = Commands.ow,
		b = Commands.b,
		bc = Commands.bc,
		r = Commands.r,
		rc = Commands.rc,
		e = Commands.e,
		ts = Commands.ts,
		w = Commands.w,
		q = Commands.q,
		ov = Commands.ov,
		mov = Commands.mov,
		[DisplayNameLocalized(nameof(Resources.CommandShowChecksumOfComparedSaveSlot), typeof(Resources))]
		cs,
		[DisplayNameLocalized(nameof(Resources.CommandShowChecksumsOfAllSaveSlots), typeof(Resources))]
		csa,
		[DisplayNameLocalized(nameof(Resources.CommandShowUnknown12BValueOfComparedSaveSlot), typeof(Resources))]
		u12b,
		[DisplayNameLocalized(nameof(Resources.CommandShowUnknown12BValuesOfAllSaveSlots), typeof(Resources))]
		u12ba
	}
}