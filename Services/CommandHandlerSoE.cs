using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common.Shared.Min.Extensions;
using Common.Shared.Min.Helpers;
using IO.Extensions;
using SoE.Models.Enums;
using SRAM.Comparison.Enums;
using SRAM.Comparison.Helpers;
using SRAM.Comparison.Services;
using SRAM.Comparison.SoE.Enums;
using SRAM.SoE.Models;
using SRAM.SoE.Models.Structs;
using Resources = SRAM.Comparison.Properties.Resources;
using Snes9x = WRAM.Snes9x.SoE.Extensions.StreamExtensions;

namespace SRAM.Comparison.SoE.Services
{
	/// <summary>Command handler implementation for SoE</summary>
	/// <inheritdoc cref="CommandHandler{TSramFile,TSaveSlot}"/>
	public class CommandHandlerSoE: CommandHandler<SramFileSoE, SaveSlotSoE>
	{
		private const string Snes9xId = "Snes9x";

		public CommandHandlerSoE() { }
		public CommandHandlerSoE(IConsolePrinter consolePrinter) : base(consolePrinter) {}

		#region Command Handing

		/// <inheritdoc cref="CommandHandler{TSramFile,TSaveSlot}"/>
		protected override bool OnRunCommand(string command, IOptions options)
		{
			Requires.NotNull(command, nameof(command));
			Requires.NotNull(options, nameof(options));

			var cmd = command.ParseEnum<CommandsSoE>();
			if (cmd == default)
			{
				if (Enum.TryParse<AlternateCommandsSoe>(command, true, out var altSoECommand))
					command = ((CommandsSoE) altSoECommand).ToString();
				else if (Enum.TryParse<AlternateCommands>(command, true, out var altCommand))
					command = ((Commands) altCommand).ToString();
				else if (CheckCustomKeyBinding(command, out var boundCommand))
					command = boundCommand;

				cmd = command.ParseEnum<CommandsSoE>();
			}

			switch (cmd)
			{
				case CommandsSoE.Compare:
					Compare(options);

					break;
				case CommandsSoE.Export:
					ExportComparisonResult(options);

					break;
				case CommandsSoE.U12b:
				case CommandsSoE.U12b_Diff:
					options.ComparisonFlags = InvertIncludeFlag(options.ComparisonFlags,
						cmd == CommandsSoE.U12b
							? ComparisonFlagsSoE.Unknown12B
							: ComparisonFlagsSoE.Unknown12BIfDifferent);

					break;
				case CommandsSoE.Checksum:
				case CommandsSoE.Checksum_Diff:
					options.ComparisonFlags = InvertIncludeFlag(options.ComparisonFlags,
						cmd == CommandsSoE.Checksum
							? ComparisonFlagsSoE.Checksum
							: ComparisonFlagsSoE.ChecksumIfDifferent);

					break;
				default:
					return base.OnRunCommand(command, options);
			}

			return true;
		}

		#endregion Command Handing

		#region Compare S-RAM

		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void Compare(Stream currFile, Stream compFile, IOptions options) => Compare<SramComparerSoE>(currFile, compFile, options);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void Compare(Stream currFile, Stream compFile, IOptions options, TextWriter output) => Compare<SramComparerSoE>(currFile, compFile, options, output);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void Compare(IOptions options) => Compare<SramComparerSoE>(options);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void Compare(IOptions options, TextWriter output) => Compare<SramComparerSoE>(options, output);

		protected override bool ConvertStreamIfSavestate(IOptions options, ref Stream stream, string? filePath)
		{
			stream.ThrowIfNull(nameof(stream));
			filePath.ThrowIfNull(nameof(filePath));

			if (!base.ConvertStreamIfSavestate(options, ref stream, filePath)) return false;

			const int length = SramSizes.All;
			MemoryStream ms;

			try
			{
				ms = stream.GetSlice(length);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

			if (length != ms.Length)
				throw new InvalidOperationException($"Copied stream has wrong size. Was {ms.Length}, but should be {length}");

			stream = ms;

			return true;
		}

		#endregion Compare S-RAM

		#region Export

		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void ExportComparisonResult(IOptions options) => ExportComparisonResult<SramComparerSoE>(options);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void ExportComparisonResult(IOptions options, string filePath) => ExportComparisonResult<SramComparerSoE>(options, filePath); 

		#endregion Export

		#region Config

		protected override void LoadConfig(IOptions options, string? configName = null)
		{
			ConsolePrinter.PrintSectionHeader();
			var filePath = base.GetConfigFilePath(options.ConfigPath, configName);
			Requires.FileExists(filePath, string.Empty, Resources.ErrorConfigFileDoesNotExist.InsertArgs(filePath));

			try
			{
				var loadedOptions = JsonFileSerializer.Deserialize<Options>(filePath)!;
				foreach (var propertyInfo in typeof(IOptions).GetProperties().Where(e => e.CanWrite))
				{
					var newValue = propertyInfo.GetValue(loadedOptions);
					propertyInfo.SetValue(options, newValue);
				}
			}
			catch (Exception ex)
			{
				Debug.Print(ex.Message);
				throw;
			}

			ConsolePrinter.PrintColoredLine(ConsoleColor.Yellow, Resources.StatusConfigFileLoadedTemplate.InsertArgs(filePath));
		}

		#endregion Config

		protected override Stream GetSramFromSavestate(IOptions options, Stream stream)
		{
			var savestateType = options.SavestateType ?? Snes9xId;

			return savestateType switch
			{
				Snes9xId => Snes9x.GetSramFromSavestate(stream, (GameRegion)options.GameRegion),
				_ => throw new NotSupportedException($"Savestate type {savestateType} is not supported.")
			};
		}

		protected override void CreateKeyBindingsFile<TEnum>() => base.CreateKeyBindingsFile<CommandsSoE>();
	}
}