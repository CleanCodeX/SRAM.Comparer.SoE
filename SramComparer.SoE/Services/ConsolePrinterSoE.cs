using System;
using SramComparer.Services;
using SramComparer.SoE.Enums;
using SramComparer.SoE.Properties;

namespace SramComparer.SoE.Services
{
    public class ConsolePrinterSoE : ConsolePrinter
    {
        protected override void PrintCustomCommands()
        {
            PrintCommandKey(Commands.fc);
            Console.WriteLine(Resources.CommandIncludeChecksum);

            PrintCommandKey(Commands.fca);
            Console.WriteLine(Resources.CommandIncludeAllChecksums);

            PrintCommandKey(Commands.fu12b);
            Console.WriteLine(Resources.CommandIncludeUnknown12B);

            PrintCommandKey(Commands.fu12ba);
            Console.WriteLine(Resources.CommandIncludeAllUnknown12Bs);
        }
    }
}