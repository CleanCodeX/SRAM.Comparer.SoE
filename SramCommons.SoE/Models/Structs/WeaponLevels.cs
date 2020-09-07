using System.Diagnostics;
using System.Runtime.InteropServices;
using SramCommons.Extensions;

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{ToString(),nq}")]
    public struct WeaponLevels
    {
        public WeaponLevel BoneCrusher;
        public WeaponLevel GladiatorSword;
        public WeaponLevel CrusaderSword;
        public WeaponLevel NeutronBlade;

        public WeaponLevel SpidersClaw;
        public WeaponLevel BronzeAxe;
        public WeaponLevel KnightBasher;
        public WeaponLevel AtomSmasher;

        public WeaponLevel HornSpear;
        public WeaponLevel BronzeSpear;
        public WeaponLevel LanceWeapon;
        public WeaponLevel LaserLance;

        public WeaponLevel Bazooka;

        public override string ToString() => $@"{nameof(BoneCrusher)}: {BoneCrusher.Major}.{BoneCrusher.Minor}
{nameof(GladiatorSword)}: {GladiatorSword.Major}.{GladiatorSword.Minor}
{nameof(CrusaderSword)}: {CrusaderSword.Major}.{CrusaderSword.Minor}
{nameof(NeutronBlade)}: {NeutronBlade.Major}.{NeutronBlade.Minor}

{nameof(SpidersClaw)}: {SpidersClaw.Major}.{SpidersClaw.Minor}
{nameof(BronzeAxe)}: {BronzeAxe.Major}.{BronzeAxe.Minor}
{nameof(KnightBasher)}: {KnightBasher.Major}.{KnightBasher.Minor}
{nameof(AtomSmasher)}: {AtomSmasher.Major}.{AtomSmasher.Minor}

{nameof(HornSpear)}: {HornSpear.Major}.{HornSpear.Minor}
{nameof(BronzeSpear)}: {BronzeSpear.Major}.{BronzeSpear.Minor}
{nameof(LanceWeapon)}: {LanceWeapon.Major}.{LanceWeapon.Minor}
{nameof(LaserLance)}: {LaserLance.Major}.{LaserLance.Minor}

{nameof(Bazooka)}: {Bazooka.Major}.{Bazooka.Minor}
".FormatStruct();
    }
}