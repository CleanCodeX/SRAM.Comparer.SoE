using SramComparer.SoE.Enums;
using RosettaStone.Sram.SoE.Models.Enums;
using SramComparer.Enums;

namespace SramComparer.SoE
{
	/// <summary>Options implementation for SoE</summary>
	/// <inheritdoc cref="Options{TGameRegion,TComparisonFlags, ExportFlags}"/>
	public class Options : Options<GameRegion, ComparisonFlagsSoE, ExportFlags>
	{
		public Options() => ExportFlags = ExportFlags.PromptName | ExportFlags.AppendLog;
	}
}