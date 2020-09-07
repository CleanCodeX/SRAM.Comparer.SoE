using System;

namespace SramCommons.SoE.Models.Enums
{
    /// the charms
    [Flags]
    public enum Charm : uint
    {
        // No meaning for 0x1 - 0x10
        ArmorPolish = 0x20,
        ChocoboEgg = 0x40,
        InsectIncense = 0x80,

        JadeDisk = 0x100,
        JaguarRing = 0x200,
        MagicGourd = 0x400,
        MoxaStick = 0x800,

        OracleBone = 0x1_000,
        RubyHeart = 0x2_000,
        SilverSheath = 0x4_000,
        StaffOfLife = 0x8_000,

        SunStone = 0x10_000,
        ThugsCloak = 0x20_000,
        WizardsCoin = 0x40_000
    }
}