using System.Diagnostics.CodeAnalysis;
using SRAM.SoE.Models.Structs;

namespace SRAM.Comparison.SoE.Helpers
{
	/// <summary>
	/// Base offsets for <see cref="SramUnknownOffset" /> members
	/// </summary>
	internal enum SramBaseOffset
	{
		None
	}

	/// <summary>
	/// Offsets for possibly non-unique field names in <see cref="SramSoE" /> structure.
	/// </summary>
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	internal enum SramUnknownOffset
	{
		None,
	}
}