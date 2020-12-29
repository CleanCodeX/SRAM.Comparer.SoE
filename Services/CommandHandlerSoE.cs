using System.IO;
using SramComparer.Services;
using SramComparer.SoE.Enums;
using SramFormat.SoE;
using SramFormat.SoE.Models.Structs;

namespace SramComparer.SoE.Services
{
	/// <summary>Command handler implementation for SoE</summary>
	/// <inheritdoc cref="CommandHandler{TSramFile,TSaveSlot}"/>
	public class CommandHandlerSoE: CommandHandler<SramFileSoE, SaveSlot>
	{
		public CommandHandlerSoE() { }
		public CommandHandlerSoE(IConsolePrinter consolePrinter) :base(consolePrinter) {}

		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void Compare(Stream currFile, Stream compFile, IOptions options) => Compare<SramComparerSoE>(currFile, compFile, options);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void Compare(Stream currFile, Stream compFile, IOptions options, TextWriter output) => Compare<SramComparerSoE>(currFile, compFile, options, output);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void Compare(IOptions options) => Compare<SramComparerSoE>(options);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void Compare(IOptions options, TextWriter output) => Compare<SramComparerSoE>(options, output);

		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void ExportComparison(IOptions options) => ExportComparison<SramComparerSoE>(options);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void ExportComparison(IOptions options, bool showInExplorer) => ExportComparison<SramComparerSoE>(options, showInExplorer);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void ExportComparison(IOptions options, string filepath) => ExportComparison<SramComparerSoE>(options, filepath);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public void ExportComparison(IOptions options, string filepath, bool showInExplorer) => ExportComparison<SramComparerSoE>(options, filepath, showInExplorer);

		/// <inheritdoc cref="CommandHandler{TSramFile,TSaveSlot}"/>
		protected override bool OnRunCommand(string command, IOptions options)
		{
			switch (command)
			{
				case nameof(CommandsSoE.c):
					Compare(options);
					break;
				case nameof(CommandsSoE.e):
					ExportComparison(options, true);
					break;
				case nameof(CommandsSoE.u12b) or nameof(CommandsSoE.u12ba):
					options.ComparisonFlags = InvertIncludeFlag(options.ComparisonFlags,
						command == nameof(CommandsSoE.u12ba)
							? ComparisonFlagsSoE.Unknown12BAllSlots
							: ComparisonFlagsSoE.Unknown12BWhenDifferent);
					break;
				case nameof(CommandsSoE.cs) or nameof(CommandsSoE.csa):
					options.ComparisonFlags = InvertIncludeFlag(options.ComparisonFlags,
						command == nameof(CommandsSoE.csa)
							? ComparisonFlagsSoE.ChecksumAllSlots
							: ComparisonFlagsSoE.ChecksumWhenDifferent);
					break;
				default:
					return base.OnRunCommand(command, options);
			}

			return true;
		}
	}
}