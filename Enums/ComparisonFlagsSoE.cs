using System;
using Common.Shared.Min.Attributes;
using SramComparer.Enums;
using Res = SramComparer.SoE.Properties.Resources;

namespace SramComparer.SoE.Enums
{
	[Flags]
	public enum ComparisonFlagsSoE : uint
	{
		[DisplayNameLocalized(nameof(Res.NonGameBuffer), typeof(Res))]
		NonGameBuffer = ComparisonFlags.NonGameBuffer,
		
		[DisplayNameLocalized(nameof(Res.WholeGameBuffer), typeof(Res))]
		WholeGameBuffer = ComparisonFlags.WholeGameBuffer,

		[DisplayNameLocalized(nameof(Res.GameChecksum), typeof(Res))]
		GameChecksum = 1 << 2,

		[DisplayNameLocalized(nameof(Res.AllGameChecksums), typeof(Res))]
		AllGameChecksums = 1 << 3 | GameChecksum,

		[DisplayNameLocalized(nameof(Res.Unknown12B), typeof(Res))]
		Unknown12B = 1 << 4,

		[DisplayNameLocalized(nameof(Res.AllUnknown12Bs), typeof(Res))]
		AllUnknown12Bs = 1 << 5 | Unknown12B,
	}
}