using System;
using Common.Shared.Min.Attributes;
using SramComparer.Enums;
using Res = SramComparer.SoE.Properties.Resources;
using ResComp = SramComparer.Properties.Resources;

namespace SramComparer.SoE.Enums
{
	[Flags]
	public enum ComparisonFlagsSoE : uint
	{
		[DisplayNameLocalized(nameof(ResComp.SlotByteByByteComparison), typeof(Res))]
		SlotByteByByteComparison = ComparisonFlags.SlotByteByByteComparison,

		[DisplayNameLocalized(nameof(ResComp.NonSlotByteByByteComparison), typeof(Res))]
		NonSlotByteByByteComparison = ComparisonFlags.NonSlotByteByByteComparison,

		[DisplayNameLocalized(nameof(Res.ChecksumComparedSlots), typeof(Res))]
		ChecksumComparedSlots = 1 << 2,

		[DisplayNameLocalized(nameof(Res.ChecksumsAllSlots), typeof(Res))]
		ChecksumAllSlots = 1 << 3 | ChecksumComparedSlots,

		[DisplayNameLocalized(nameof(Res.Unknown12BComparedSlots), typeof(Res))]
		Unknown12BComparedSlots = 1 << 4,

		[DisplayNameLocalized(nameof(Res.Unknown12BAllSlots), typeof(Res))]
		Unknown12BAllSlots = 1 << 5 | Unknown12BComparedSlots,
	}
}