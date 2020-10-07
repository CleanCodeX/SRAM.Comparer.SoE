using System.IO;
using SramFormat.SoE;
using SramFormat.SoE.Models.Structs;
using SramComparer.Services;
using SramComparer.SoE.Enums;

namespace SramComparer.SoE.Services
{
    public class CommandHandlerSoE: CommandHandler<SramFileSoE, SramGame>
    {
        public CommandHandlerSoE() { }
		public CommandHandlerSoE(IConsolePrinter consolePrinter) :base(consolePrinter) {}

        public void Compare(Stream currFile, Stream compFile, IOptions options) => Compare<SramComparerSoE>(currFile, compFile, options);
        public void Compare(Stream currFile, Stream compFile, IOptions options, TextWriter output) => Compare<SramComparerSoE>(currFile, compFile, options, output);
        public void Compare(IOptions options) => Compare<SramComparerSoE>(options);
        public void Compare(IOptions options, TextWriter output) => Compare<SramComparerSoE>(options, output);

        public void ExportComparison(IOptions options, bool showInExplorer = false) => ExportComparison<SramComparerSoE>(options, showInExplorer);
        public void ExportComparison(IOptions options, string filepath, bool showInExplorer = false) => ExportComparison<SramComparerSoE>(options, filepath, showInExplorer);

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