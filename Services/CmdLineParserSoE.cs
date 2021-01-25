using SoE.Models.Enums;
using SRAM.Comparison.Enums;
using SRAM.Comparison.Services;
using SRAM.Comparison.SoE.Enums;

namespace SRAM.Comparison.SoE.Services
{
	/// <summary>Parser implementation for SoE</summary>
	/// <inheritdoc cref="CmdLineParser{TOptions,TGameRegion,TComparisonFlags,ExportFlags}"/>
	public class CmdLineParserSoE : CmdLineParser<Options, GameRegion, ComparisonFlagsSoE, ExportFlags>
	{ }
}