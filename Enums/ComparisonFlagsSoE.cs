﻿using System;
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

		[DisplayNameLocalized(nameof(Res.ChecksumComparedIfDifferent), typeof(Res))]
		ChecksumIfDifferent = 1 << 2,

		[DisplayNameLocalized(nameof(Res.ChecksumCompared), typeof(Res))]
		Checksum = 1 << 3 | ChecksumIfDifferent,

		[DisplayNameLocalized(nameof(Res.Unknown12BComparedIfDifferent), typeof(Res))]
		Unknown12BIfDifferent = 1 << 4,

		[DisplayNameLocalized(nameof(Res.Unknown12BCompared), typeof(Res))]
		Unknown12B = 1 << 5 | Unknown12BIfDifferent,
	}
}