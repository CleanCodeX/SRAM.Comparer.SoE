using SramCommons.SoE;
using SramCommons.SoE.Models.Structs;
using SramComparer.Services;
using SramComparer.SoE.Enums;

namespace SramComparer.SoE.Services
{
    public class CommandExecutorSoE: CommandExecutor<SramFile, SramGame>
    {
        public CommandExecutorSoE() { }
		public CommandExecutorSoE(IConsolePrinter consolePrinter) :base(consolePrinter) {}

        public void CompareFiles(IOptions options) => CompareFiles<SramComparerSoE>(options);
        public void ExportCurrentComparison(IOptions options) => ExportCurrentComparison<SramComparerSoE>(options);

        protected override bool OnShouldHandleCommand(string command, IOptions options)
        {
            switch (command)
            {
                case nameof(Commands.c):
                    CompareFiles(options);
                    break;
                case nameof(Commands.e):
                    ExportCurrentComparison(options);
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
                    return true;
            }

            return false;
        }
    }
}