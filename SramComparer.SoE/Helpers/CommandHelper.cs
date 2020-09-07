using SramCommons.SoE.Helpers;
using SramCommons.SoE.Models;
using SramCommons.SoE.Models.Structs;
using SramComparer.Helpers;

namespace SramComparer.SoE.Helpers
{
    public class CommandHelper: CommandHelperBase<SramFile, SramGame, GameId>
    {
        public static void CompareFiles(IOptions options) => CompareFiles<SramComparer>(options);
        public static void ExportCurrentComparison(IOptions options) => ExportCurrentComparison<SramComparer>(options);

        public static void InvertIncludeFlag(ref ComparisonFlags flags, ComparisonFlags flag) =>
            CommandHelperBase<SramFile, SramGame, GameId>.InvertIncludeFlag(ref flags, flag);
    }
}