using System;
using Common.Shared.Min.Attributes;
using SRAM.Comparison.Enums;
using Res = SRAM.Comparison.SoE.Properties.Resources;
using ResComp = SRAM.Comparison.Properties.Resources;

namespace SRAM.Comparison.SoE.Enums
{
	[Serializable]
	[Flags]
	public enum ComparisonFlagsSoE : uint
	{
		[DisplayNameLocalized(nameof(ResComp.EnumSlotByteComparison), typeof(ResComp))]
		SlotByteComparison = ComparisonFlags.SlotByteComparison,

		[DisplayNameLocalized(nameof(ResComp.EnumNonSlotComparison), typeof(ResComp))]
		NonSlotComparison = ComparisonFlags.NonSlotComparison,

		[DisplayNameLocalized(nameof(ResComp.EnumChecksumStatus), typeof(ResComp))]
		ChecksumStatus = ComparisonFlags.ChecksumStatus,

		[DisplayNameLocalized(nameof(Res.EnumChecksumIfDifferent), typeof(Res))]
		ChecksumIfDifferent = 0x100,

		[DisplayNameLocalized(nameof(Res.EnumChecksum), typeof(Res))]
		Checksum = 0x200 | ChecksumIfDifferent,

		[DisplayNameLocalized(nameof(Res.EnumUnknown12BIfDifferent), typeof(Res))]
		Unknown12BIfDifferent = 0x400,

		Unknown12B = 0x800 | Unknown12BIfDifferent,
	}
}