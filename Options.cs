using SoE.Models.Enums;
using SRAM.Comparison.Enums;
using SRAM.Comparison.SoE.Enums;

namespace SRAM.Comparison.SoE
{
	/// <summary>Options implementation for SoE</summary>
	/// <inheritdoc cref="Options{TGameRegion,TComparisonFlags, ExportFlags}"/>
	public class Options : Options<GameRegion, ComparisonFlagsSoE, ExportFlags>
	{
		public Options() => ExportFlags = ExportFlags.PromptName;
	}
}