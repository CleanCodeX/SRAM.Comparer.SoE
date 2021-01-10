using System;
using System.Diagnostics;
using System.IO;
using Common.Shared.Min.Extensions;
using Common.Shared.Min.Helpers;
using SramComparer.Enums;
using SramComparer.Helpers;
using SramComparer.Services;
using SramComparer.SoE.Enums;
using SramComparer.SoE.Extensions;
using SramComparer.SoE.Properties;
using SramFormat.SoE;
using SramFormat.SoE.Constants;
using SramFormat.SoE.Models.Structs;

namespace SramComparer.SoE.Services
{
	/// <summary>Command handler implementation for SoE</summary>
	/// <inheritdoc cref="CommandHandler{TSramFile,TSaveSlot}"/>
	public class CommandHandlerSoE: CommandHandler<SramFileSoE, SaveSlot>
	{
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
					ExportComparisonResult(options, true);

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

		#region Compare SRAM

		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void Compare(Stream currFile, Stream compFile, IOptions options) => Compare<SramComparerSoE>(currFile, compFile, options);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void Compare(Stream currFile, Stream compFile, IOptions options, TextWriter output) => Compare<SramComparerSoE>(currFile, compFile, options, output);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void Compare(IOptions options) => Compare<SramComparerSoE>(options);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void Compare(IOptions options, TextWriter output) => Compare<SramComparerSoE>(options, output);

		protected override bool ConvertStreamIfSavestate(ref Stream stream, string? filePath, string? savestateType)
		{
			stream.ThrowIfNull(nameof(stream));
			filePath.ThrowIfNull(nameof(filePath));

			if (!base.ConvertStreamIfSavestate(ref stream, filePath, savestateType)) return false;

			const int length = Sizes.Sram;
			MemoryStream ms;

			try
			{
				ms = stream.GetStreamSlice(length);
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

		#endregion Compare SRAM

		#region Export

		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void ExportComparisonResult(IOptions options) => ExportComparisonResult<SramComparerSoE>(options);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void ExportComparisonResult(IOptions options, bool showInExplorer) => ExportComparisonResult<SramComparerSoE>(options, showInExplorer);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void ExportComparisonResult(IOptions options, string filePath) => ExportComparisonResult<SramComparerSoE>(options, filePath);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void ExportComparisonResult(IOptions options, string filePath, bool showInExplorer) => ExportComparisonResult<SramComparerSoE>(options, filePath, showInExplorer);

		#endregion Export

		#region Config

		protected override void LoadConfig(IOptions options, string? configName = null)
		{
			ConsolePrinter.PrintSectionHeader();
			var filePath = base.GetConfigFilePath(options.ConfigFilePath, configName);
			Requires.FileExists(filePath, string.Empty, SramComparer.Properties.Resources.ErrorConfigFileDoesNotExist.InsertArgs(filePath));

			try
			{
				var loadedOptions = JsonFileSerializer.Deserialize<Options>(filePath)!;

				var typedOptions = (Options)options;

				typedOptions.CurrentFilePath = loadedOptions.CurrentFilePath;
				typedOptions.CurrentFileSaveSlot = loadedOptions.CurrentFileSaveSlot;
				
				typedOptions.ComparisonFilePath = loadedOptions.ComparisonFilePath;
				typedOptions.ComparisonFileSaveSlot = loadedOptions.ComparisonFileSaveSlot;

				typedOptions.SavestateType = loadedOptions.SavestateType;
				typedOptions.ExportDirectory = loadedOptions.ExportDirectory;
				typedOptions.ColorizeOutput = loadedOptions.ColorizeOutput;

				typedOptions.ComparisonFlags = loadedOptions.ComparisonFlags;
				typedOptions.GameRegion = loadedOptions.GameRegion;

				typedOptions.UILanguage = loadedOptions.UILanguage;
				typedOptions.ComparisonResultLanguage = loadedOptions.ComparisonResultLanguage;
			}
			catch (Exception ex)
			{
				Debug.Print(ex.Message);
				throw;
			}

			ConsolePrinter.PrintColoredLine(ConsoleColor.Yellow, Resources.StatusConfigFileLoadedTemplate.InsertArgs(filePath));
		}

		#endregion Config

		protected override void CreateKeyBindingsFile<TEnum>() => base.CreateKeyBindingsFile<CommandsSoE>();
	}
}