using System.Diagnostics.CodeAnalysis;
using Common.Shared.Min.Attributes;
using SramComparer.Enums;
using SramComparer.SoE.Properties;

namespace SramComparer.SoE.Enums
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Ausstehend>")]
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

		[DisplayNameLocalized(nameof(Resources.CommandShowChecksum), typeof(Resources))]
		Checksum,

		[DisplayNameLocalized(nameof(Resources.CommandShowChecksumIfDifferent), typeof(Resources))]
		Checksum_IfDiff,

		[DisplayNameLocalized(nameof(Resources.CommandShowUnknown12B), typeof(Resources))]
		U12b,

		[DisplayNameLocalized(nameof(Resources.CommandShowUnknown12BIfDifferent), typeof(Resources))]
		U12b_IfDiff
	}
}