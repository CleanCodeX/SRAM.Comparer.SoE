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

        protected override bool OnUnHandledCommand(string command, IOptions options)
        {
            switch(command)
            {
				case nameof(Commands.fu12b):
				case nameof(Commands.fu12ba):
					options.Flags = InvertIncludeFlag(options.Flags,
						command == nameof(Commands.fu12ba) ? ComparisonFlagsSoE.AllUnknown12Bs : ComparisonFlagsSoE.Unknown12B);
					return true;
				case nameof(Commands.fc):
				case nameof(Commands.fca):
					options.Flags = InvertIncludeFlag(options.Flags,
						command == nameof(Commands.fca) ? ComparisonFlagsSoE.AllGameChecksums : ComparisonFlagsSoE.GameChecksum);
					return true;
				default:
					return false;
			}
        }
    }
}