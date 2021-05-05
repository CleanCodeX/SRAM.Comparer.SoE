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
		ExportCompResult = Commands.ExportCompResult,
		ExportCompResultOpen = Commands.ExportCompResultOpen,
		ExportCompResultSelect = Commands.ExportCompResultSelect,

		[DisplayNameLocalized(nameof(Resources.CmdShowChecksum), typeof(Resources))]
		Checksum,

		[DisplayNameLocalized(nameof(Resources.CmdShowChecksumIfDifferent), typeof(Resources))]
		ChecksumDiff,

		[DisplayNameLocalized(nameof(Resources.CmdShowScriptedEventTimer), typeof(Resources))]
		EventTimer,

		[DisplayNameLocalized(nameof(Resources.CmdShowScriptedEventTimerIfDifferent), typeof(Resources))]
		EventTimerDiff,

		[DisplayNameLocalized(nameof(Resources.CmdShowTerminalCodes), typeof(Resources))]
		ShowTerminalCodes,
	}

	internal enum AlternateCommandsSoe
	{
		Check = CommandsSoE.Checksum,
		CheckDiff = CommandsSoE.ChecksumDiff,
		Timer = CommandsSoE.EventTimer,
		TimerDiff = CommandsSoE.EventTimerDiff,
		ShowCodes = CommandsSoE.ShowTerminalCodes,
		STC = CommandsSoE.ShowTerminalCodes,
	}
}