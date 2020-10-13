using SramFormat.SoE.Models.Enums;
using SramComparer.Services;
using SramComparer.SoE.Enums;

namespace SramComparer.SoE.Services
{
	/// <summary>Parser implementation for SoE</summary>
	/// <inheritdoc cref="CmdLineParser{TOptions,TFileRegion,TComparisonFlags}"/>
	public class CmdLineParserSoE : CmdLineParser<Options, FileRegion, ComparisonFlagsSoE>
	{ }
}