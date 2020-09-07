using System;

namespace SramCommons.SoE.Models.Enums
{
    /// the boy's weapons
    [Flags]
    public enum Weapon : ushort
    {
        //Bit 1?
        BoneCrusher = 0x2,
        GladiatorSword = 0x4,
        CrusaderSword = 0x8,
        NeutronBlade = 0x10,

        SpidersClaw = 0x20,
        BronzeAxe = 0x40,
        KnightBasher = 0x80,
        AtomSmasher = 0x100,

        HornSpear = 0x200,
        BronzeSpear = 0x400,
        LanceWeapon = 0x800,
        LaserLance = 0x1_000,

        Bazooka = 0x2_000
    }
}