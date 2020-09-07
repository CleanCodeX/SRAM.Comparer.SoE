using System;

namespace SramCommons.SoE.Models.Enums
{
    /// the alchemies
    [Flags]
    public enum Alchemy: ulong
    {
        None = 0,

        AcidRain = 0x1,
        Atlas = 0x2,
        Barrier = 0x4,
        CallUp = 0x8,
        Corrosion = 0x10,
        Crush = 0x20,
        Cure = 0x40,
        Defend = 0x80,

        DoubleDrain = 0x100,
        Drain = 0x200,
        Energize = 0x400,
        Escape = 0x800,
        Explosion = 0x1_000,
        FireBall = 0x2_000,
        FirePower = 0x4_000,
        Flash = 0x8_000,

        ForceField = 0x10_000,
        HardBall = 0x20_000,
        Heal = 0x40_000,
        Lance = 0x80_000,
        Laser = 0x100_000,
        Levitate = 0x200_000,
        LightningStorm = 0x400_000,
        MiracleCure = 0x800_000,

        Nitro = 0x1_000_000,
        OneUp = 0x2_000_000,
        Reflect = 0x4_000_000,
        Regrowth = 0x8_000_000,
        Revealer = 0x10_000_000,
        Revive = 0x20_000_000,
        SlowBurn = 0x40_000_000,
        Speed = 0x80_000_000,

        Sting = 0x100_000_000,
        Stop = 0x200_000_000,
        SuperHeal = 0x400_000_000
        //Other Bits?
    }
}