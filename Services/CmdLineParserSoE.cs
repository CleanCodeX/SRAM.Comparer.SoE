using SramComparer.Services;
using SramComparer.SoE.Enums;
using SramFormat.SoE.Enums;

namespace SramComparer.SoE.Services
{
	/// <summary>Parser implementation for SoE</summary>
	/// <inheritdoc cref="CmdLineParser{TOptions,TGameRegion,TComparisonFlags}"/>
	public class CmdLineParserSoE : CmdLineParser<Options, GameRegion, ComparisonFlagsSoE>
	{ }
}