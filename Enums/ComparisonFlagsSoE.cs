using System;
using Common.Shared.Min.Attributes;
using SramComparer.Enums;
using Res = SramComparer.SoE.Properties.Resources;
using ResComp = SramComparer.Properties.Resources;

namespace SramComparer.SoE.Enums
{
	[Serializable]
	[Flags]
	public enum ComparisonFlagsSoE : uint
	{
		[DisplayNameLocalized(nameof(ResComp.EnumSlotByteByByteComparison), typeof(Res))]
		SlotByteByByteComparison = ComparisonFlags.SlotByteByByteComparison,

		[DisplayNameLocalized(nameof(ResComp.EnumNonSlotByteByByteComparison), typeof(Res))]
		NonSlotByteByByteComparison = ComparisonFlags.NonSlotByteByByteComparison,

		[DisplayNameLocalized(nameof(Res.ChecksumIfDifferent), typeof(Res))]
		ChecksumIfDifferent = 1 << 2,

		[DisplayNameLocalized(nameof(Res.Checksum), typeof(Res))]
		Checksum = 1 << 3 | ChecksumIfDifferent,

		[DisplayNameLocalized(nameof(Res.Unknown12BIfDifferent), typeof(Res))]
		Unknown12BIfDifferent = 1 << 4,

		Unknown12B = 1 << 5 | Unknown12BIfDifferent,
	}
}