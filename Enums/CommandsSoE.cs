using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Common.Shared.Min.Attributes;
using SramComparer.Enums;
using SramComparer.Helpers;
using SramComparer.SoE.Properties;

namespace SramComparer.SoE.Enums
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Ausstehend>")]
	[JsonConverter(typeof(JsonStringEnumObjectConverter))]
	public enum CommandsSoE
	{
		Help = Commands.Help,
		Config = Commands.Config,
		Guide_Srm = Commands.Guide_Srm,
		Guide_Savestate = Commands.Guide_Savestate,
		Sbc = Commands.Sbc,
		Nsbc = Commands.Nsbc,
		SetSlot = Commands.SetSlot,
		SetSlot_Comp = Commands.SetSlot_Comp,
		Compare = Commands.Compare,
		OverwriteComp = Commands.OverwriteComp,
		Backup = Commands.Backup,
		Backup_Comp = Commands.Backup_Comp,
		Restore = Commands.Restore,
		Restore_Comp = Commands.Restore_Comp,
		Export = Commands.Export,
		Transfer = Commands.Transfer,
		Lang = Commands.Lang,
		Lang_Comp = Commands.Lang_Comp,
		Clear = Commands.Clear,
		Quit = Commands.Quit,
		Offset = Commands.Offset,
		EditOffset = Commands.EditOffset,
		LoadConfig = Commands.LoadConfig,
		SaveConfig = Commands.SaveConfig,

		[DisplayNameLocalized(nameof(Resources.CmdShowChecksum), typeof(Resources))]
		Checksum,

		[DisplayNameLocalized(nameof(Resources.CmdShowChecksumIfDifferent), typeof(Resources))]
		Checksum_Diff,

		[DisplayNameLocalized(nameof(Resources.CmdShowUnknown12B), typeof(Resources))]
		U12b,

		[DisplayNameLocalized(nameof(Resources.CmdShowUnknown12BIfDifferent), typeof(Resources))]
		U12b_Diff
	}

	internal enum AlternateCommandsSoe
	{
		Cksm = CommandsSoE.Checksum,
		Cksm_D = CommandsSoE.Checksum_Diff,
	}
}