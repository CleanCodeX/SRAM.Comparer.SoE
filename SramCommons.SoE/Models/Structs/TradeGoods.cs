using System.Diagnostics;
using System.Runtime.InteropServices;
using SramCommons.Extensions;

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{ToString(),nq}")]
    public struct TradeGoods
    {
        //  (0 - 99)
        public ushort AnnihilationAmulet; 
        public ushort Beads;
        public ushort CeramicPot; 
        public ushort Chicken; 
        public ushort GoldenJackal; 
        public ushort JeweledScarab; 
        public ushort LimestoneTablet; 
        public ushort Perfume; 
        public ushort Rice; 
        public ushort Spice; 
        public ushort SouvenirSpoon; 
        public ushort Tapestry; 
        public ushort TicketForExhibition; 

        public override string ToString() => $@"{nameof(AnnihilationAmulet)}: {AnnihilationAmulet}
{nameof(Beads)}: {Beads}
{nameof(CeramicPot)}: {CeramicPot}
{nameof(Chicken)}: {Chicken}
{nameof(GoldenJackal)}: {GoldenJackal}
{nameof(JeweledScarab)}: {JeweledScarab}
{nameof(LimestoneTablet)}: {LimestoneTablet}
{nameof(Perfume)}: {Perfume}
{nameof(Rice)}: {Rice}
{nameof(Spice)}: {Spice}
{nameof(SouvenirSpoon)}: {SouvenirSpoon}
{nameof(Tapestry)}: {Tapestry}
{nameof(TicketForExhibition)}: {TicketForExhibition}
".FormatStruct();
    }
}