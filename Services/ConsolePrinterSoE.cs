using SramComparer.Services;
using SramComparer.SoE.Enums;
using SramComparer.SoE.Properties;

namespace SramComparer.SoE.Services
{
	/// <summary>Console printer implementation for SoE</summary>
	public class ConsolePrinterSoE : ConsolePrinter
	{
		protected override void PrintCustomCommands()
		{
			PrintCommandKey(Commands.fc);
			PrintLine(Resources.CommandIncludeChecksum);

			PrintCommandKey(Commands.fca);
			PrintLine(Resources.CommandIncludeAllChecksums);

			PrintCommandKey(Commands.fu12b);
			PrintLine(Resources.CommandIncludeUnknown12B);

			PrintCommandKey(Commands.fu12ba);
			PrintLine(Resources.CommandIncludeAllUnknown12Bs);
		}
	}
}