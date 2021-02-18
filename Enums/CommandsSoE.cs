using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Common.Shared.Min.Attributes;
using SRAM.Comparison.Enums;
using SRAM.Comparison.Helpers;
using SRAM.Comparison.SoE.Properties;

namespace SRAM.Comparison.SoE.Enums
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Ausstehend>")]
	[JsonConverter(typeof(JsonStringEnumObjectConverter))]
	public enum CommandsSoE
	{
		Compare = Commands.Compare,
		ExportComparison = Commands.ExportComparison,

		[DisplayNameLocalized(nameof(Resources.CmdShowChecksum), typeof(Resources))]
		Checksum,

		[DisplayNameLocalized(nameof(Resources.CmdShowChecksumIfDifferent), typeof(Resources))]
		Checksum_Diff,

		[DisplayNameLocalized(nameof(Resources.CmdShowScriptedEventTimer), typeof(Resources))]
		EventTimer,

		[DisplayNameLocalized(nameof(Resources.CmdShowScriptedEventTimerIfDifferent), typeof(Resources))]
		EventTimer_Diff
	}

	internal enum AlternateCommandsSoe
	{
		Cksm = CommandsSoE.Checksum,
		Cksm_D = CommandsSoE.Checksum_Diff,
	}
}