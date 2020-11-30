using System.IO;
using SramComparer.Services;
using SramComparer.SoE.Enums;
using SramFormat.SoE;
using SramFormat.SoE.Models.Structs;

namespace SramComparer.SoE.Services
{
	/// <summary>Command handler implementation for SoE</summary>
	/// <inheritdoc cref="CommandHandler{TSramFile,TSramGame}"/>
	public class CommandHandlerSoE: CommandHandler<SramFileSoE, SramGame>
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

		/// <inheritdoc cref="CommandHandler{TSramFile,TSramGame}"/>
		protected override bool OnRunCommand(string command, IOptions options)
		{
			switch (command)
			{
				case nameof(Commands.c):
					Compare(options);
					break;
				case nameof(Commands.e):
					ExportComparison(options, true);
					break;
				case nameof(Commands.fu12b) or nameof(Commands.fu12ba):
					options.Flags = InvertIncludeFlag(options.Flags,
						command == nameof(Commands.fu12ba)
							? ComparisonFlagsSoE.AllUnknown12Bs
							: ComparisonFlagsSoE.Unknown12B);
					break;
				case nameof(Commands.fc) or nameof(Commands.fca):
					options.Flags = InvertIncludeFlag(options.Flags,
						command == nameof(Commands.fca)
							? ComparisonFlagsSoE.AllGameChecksums
							: ComparisonFlagsSoE.GameChecksum);
					break;
				default:
					return base.OnRunCommand(command, options);
			}

			return true;
		}
	}
}