using System;
using System.Collections.Generic;
using System.Text;

namespace Laba5
{
    class TargetEventArgs
    {
        public bool TheDamagePassed { get; } = false;
        public string TypeOfHit { get; }
        public int Damage { get; }
        public double TargetArmorRate { get; }
        public int RandFactor { get; }
        public int EvadeChance { get; }
        public double DMGBuffModificator { get; }
        public double CritModifier { get; }

        public TargetEventArgs(bool TheDamagePassed,string TypeOfHit, int Damage=0, int RandFactor=0,int EvadeChance=0,double DMGBuffModificator=0,double CritModifier=0,double TargetArmorRate=0)
        {
            this.TheDamagePassed = TheDamagePassed;
            this.TypeOfHit = TypeOfHit;
            this.Damage = Damage;
            this.RandFactor = RandFactor;
            this.EvadeChance = EvadeChance;
            this.DMGBuffModificator = DMGBuffModificator;
            this.CritModifier = CritModifier;
            this.TargetArmorRate = TargetArmorRate;
        }

    }
}
