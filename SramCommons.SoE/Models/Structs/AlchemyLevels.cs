using System.Diagnostics;
using System.Runtime.InteropServices;
using SramCommons.Extensions;

namespace SramCommons.SoE.Models.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{ToString(),nq}")]
    public struct AlchemyLevels
    {
        public ushort AcidRain;
        public ushort Atlas;
        public ushort Barrier;
        public ushort CallUp;
        public ushort Corrosion;
        public ushort Crush;
        public ushort Cure;
        public ushort Defend;

        public ushort DoubleDrain;
        public ushort Drain;
        public ushort Energize;
        public ushort Escape;
        public ushort Explosion;
        public ushort FireBall;
        public ushort FirePower;
        public ushort Flash;

        public ushort ForceField;
        public ushort HardBall;
        public ushort Heal;
        public ushort Lance;
        public ushort Laser;
        public ushort Levitate;
        public ushort LightningStorm;
        public ushort MiracleCure;

        public ushort Nitro;
        public ushort OneUp;
        public ushort Reflect;
        public ushort Regrowth;
        public ushort Revealer;
        public ushort Revive;
        public ushort SlowBurn;
        public ushort Speed;

        public ushort Sting;
        public ushort Stop;
        public ushort SuperHeal;

        public override string ToString() => $@"{nameof(AcidRain)}: {AcidRain}
{nameof(Atlas)}: {Atlas}
{nameof(Barrier)}: {Barrier}
{nameof(CallUp)}: {CallUp}
{nameof(Corrosion)}: {Corrosion}
{nameof(Crush)}: {Crush}
{nameof(Cure)}: {Cure}
{nameof(Defend)}: {Defend}

{nameof(DoubleDrain)}: {DoubleDrain}
{nameof(Drain)}: {Drain}
{nameof(Energize)}: {Energize}
{nameof(Escape)}: {Escape}
{nameof(Explosion)}: {Explosion}
{nameof(FireBall)}: {FireBall}
{nameof(FirePower)}: {FirePower}
{nameof(Flash)}: {Flash}

{nameof(ForceField)}: {ForceField}
{nameof(HardBall)}: {HardBall}
{nameof(Heal)}: {Heal}
{nameof(Lance)}: {Lance}
{nameof(Laser)}: {Laser}
{nameof(Levitate)}: {Levitate}
{nameof(LightningStorm)}: {LightningStorm}
{nameof(MiracleCure)}: {MiracleCure}

{nameof(Nitro)}: {Nitro}
{nameof(OneUp)}: {OneUp}
{nameof(Reflect)}: {Reflect}
{nameof(Regrowth)}: {Regrowth}
{nameof(Revealer)}: {Revealer}
{nameof(Revive)}: {Revive}
{nameof(SlowBurn)}: {SlowBurn}
{nameof(Speed)}: {Speed}

{nameof(Sting)}: {Sting}
{nameof(Stop)}: {Stop}
{nameof(SuperHeal)}: {SuperHeal}
".FormatStruct();
    }
}