using SramComparer.Services;
using SramComparer.SoE.Enums;
using RosettaStone.Sram.SoE.Models.Enums;
using SramComparer.Enums;

namespace SramComparer.SoE.Services
{
	/// <summary>Parser implementation for SoE</summary>
	/// <inheritdoc cref="CmdLineParser{TOptions,TGameRegion,TComparisonFlags,ExportFlags}"/>
	public class CmdLineParserSoE : CmdLineParser<Options, GameRegion, ComparisonFlagsSoE, ExportFlags>
	{ }
}