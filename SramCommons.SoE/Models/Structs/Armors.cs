using System.Diagnostics;
using System.Runtime.InteropServices;
using SramCommons.Extensions;

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{ToString(),nq}")]
    public struct Armors
    {
        // Vests (0 - 6)
        public byte GrassVest;
        public byte ShellPlate;
        public byte DinoSkin;
        public byte BronzeArmor;
        public byte StoneVest;
        public byte CenturionCape;
        public byte SilverMail;
        public byte GoldPlatedVest;
        public byte ShiningArmor;
        public byte MagnaMail;
        public byte TitaniumVest;
        public byte VirtualVest;

        // Hats (0 - 6)
        public byte GrassHat;
        public byte ShellHat;
        public byte DinoHelmet;
        public byte BronzeHelmet;
        public byte ObsidianHelmet;
        public byte CenturionHelmet;
        public byte TitansCrown;
        public byte DragonHelmet;
        public byte KnightsHelmet;
        public byte LightningHelmet;
        public byte OldReliable;
        public byte BrainStorm;

        // Bracelets (0 - 6)
        public byte VineBracelet;
        public byte MammothGuard;
        public byte ClawGuard;
        public byte SerpentBracer;
        public byte BronzeGauntlet;
        public byte GlovesOfRa;
        public byte IronBracer;
        public byte MagiciansRing;
        public byte DragonsClaw;
        public byte CyberGlove;
        public byte ProtectorRing;
        public byte VirtualGlove;

        // Collars (0 - 6)
        public byte LeatherCollar;
        public byte SpikyCollar;
        public byte DefenderCollar;
        public byte SpotsCollar;

        public override string ToString() => 
            $@"{nameof(GrassVest)}: {GrassVest}
{nameof(ShellPlate)}: {ShellPlate}
{nameof(DinoSkin)}: {DinoSkin}
{nameof(BronzeArmor)}: {BronzeArmor}
{nameof(StoneVest)}: {StoneVest}
{nameof(CenturionCape)}: {CenturionCape}
{nameof(SilverMail)}: {SilverMail}
{nameof(GoldPlatedVest)}: {GoldPlatedVest}
{nameof(ShiningArmor)}: {ShiningArmor}
{nameof(MagnaMail)}: {MagnaMail}
{nameof(TitaniumVest)}: {TitaniumVest}
{nameof(VirtualVest)}: {VirtualVest}

{nameof(GrassHat)}: {GrassHat}
{nameof(ShellHat)}: {ShellHat}
{nameof(DinoHelmet)}: {DinoHelmet}
{nameof(BronzeHelmet)}: {BronzeHelmet}
{nameof(ObsidianHelmet)}: {ObsidianHelmet}
{nameof(CenturionHelmet)}: {CenturionHelmet}
{nameof(TitansCrown)}: {TitansCrown}
{nameof(DragonHelmet)}: {DragonHelmet}
{nameof(KnightsHelmet)}: {KnightsHelmet}
{nameof(LightningHelmet)}: {LightningHelmet}
{nameof(OldReliable)}: {OldReliable}
{nameof(BrainStorm)}: {BrainStorm}

{nameof(VineBracelet)}: {VineBracelet}
{nameof(MammothGuard)}: {MammothGuard}
{nameof(ClawGuard)}: {ClawGuard}
{nameof(SerpentBracer)}: {SerpentBracer}
{nameof(BronzeGauntlet)}: {BronzeGauntlet}
{nameof(GlovesOfRa)}: {GlovesOfRa}
{nameof(IronBracer)}: {IronBracer}
{nameof(MagiciansRing)}: {MagiciansRing}
{nameof(DragonsClaw)}: {DragonsClaw}
{nameof(CyberGlove)}: {CyberGlove}
{nameof(ProtectorRing)}: {ProtectorRing}
{nameof(VirtualGlove)}: {VirtualGlove}

{nameof(LeatherCollar)}: {LeatherCollar}
{nameof(SpikyCollar)}: {SpikyCollar}
{nameof(DefenderCollar)}: {DefenderCollar}
{nameof(SpotsCollar)}: {SpotsCollar}
".FormatStruct();
    }
}