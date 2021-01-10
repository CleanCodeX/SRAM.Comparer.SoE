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
		Compare = Commands.Compare,
		Export = Commands.Export,

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