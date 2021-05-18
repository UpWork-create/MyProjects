using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Laba5
{
    abstract class Enemy:IShowInfo,ITargetInteraction<Player>
    {
        protected delegate void GetInfo(Enemy Race);
        protected static GetInfo GetEnemyInfo = (Enemy Race) =>
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{Race.EnemyName}'s info:");
            Console.ResetColor();
            Console.WriteLine($"\tDamage:{Race.Damage}");
            Console.WriteLine($"\tHealth:{Race.Health}");
            Console.WriteLine($"\tEvasion Chance:{Race.EvasionChance}%");
            Console.WriteLine($"\tCritical Chance:{Race.CritChance}%");
            Console.WriteLine($"\tMiss Chance:{Race.MissChance}%");
            Console.WriteLine($"\tArmor:{Race.Armor}");
            Console.WriteLine("Ability:");
        };
        public delegate void DamageInfo(object sender, TargetEventArgs e);
        public static event DamageInfo EnemyDamageInfo;

        protected int _Damage;
        protected double _Armor;
        protected int _EvasionChance;
        protected int _MissChance;
        protected int _CritChance;
        protected int _Health;
        public string EnemyName { get; protected set; }
        public bool EnabledAbility { get; protected set; }
        public int AbilityCD { get; protected set; }
        public int Stun { get; set; }
        public void ReduceCD()
        {
            if (Stun != 0)
                Stun--;
            if (AbilityCD != 0)
                AbilityCD--;
        }

        public int Damage { get { return _Damage; } }
        public double Armor { get { return _Armor; } }
        public int EvasionChance { get { return _EvasionChance; } }
        public int MissChance { get { return _MissChance; } }
        public int CritChance { get { return _CritChance; } }
        public int Health
        { 
            get { return _Health; } 
            set { _Health = value; }
        }

        public abstract void ShowInfo();
        public abstract void UniqueAbility(Player player);

        public void DealDamage(Player target)
        {
            Random rand = new Random();
            int RandFactor = rand.Next(1, 15);
            if (target.UsingShield == false)
            {
                if ((rand.Next(1, 100) <= MissChance) || (rand.Next(1, 100) <= target.EvadeChance))
                {
                    EnemyDamageInfo?.Invoke(this,new TargetEventArgs(false,"Enemy MISS!"));
                    target.Health -= 0;
                }
                else
                {
                    if (rand.Next(1, 100) <= CritChance)
                    {
                        EnemyDamageInfo?.Invoke(this, new TargetEventArgs(true,"Enemy CRIT!",Damage*2,RandFactor*2,EvasionChance, DMGBuffModificator:1, CritModifier:2, target.Armor));
                        target.Health -= Convert.ToInt32((((Damage + RandFactor) * 2) + ((Damage + RandFactor) * 2 * target.Armor / 1000)));
                    }
                    else
                    {
                        EnemyDamageInfo?.Invoke(this, new TargetEventArgs(true,"Enemy hit!", Damage, RandFactor, EvasionChance, DMGBuffModificator:1, CritModifier:2, target.Armor));
                        target.Health -= Convert.ToInt32(((Damage + RandFactor) + (target.Armor / 1000 * (Damage + RandFactor))));
                    }
                }
            }
            else if(target.UsingShield==true)
            {
                if ((rand.Next(1, 100) <= MissChance) || (rand.Next(1, 100) <= target.EvadeChance))
                {
                    EnemyDamageInfo?.Invoke(this, new TargetEventArgs(false,"Enemy MISS!"));
                    target.Health -= 0;
                }
                else if(rand.Next(1,100)<=10)
                {
                    EnemyDamageInfo?.Invoke(this, new TargetEventArgs(false,"BLOCK"));
                    target.Health -= 0;
                }
                else
                {
                    if (rand.Next(1, 100) <= CritChance)
                    {                        
                        EnemyDamageInfo?.Invoke(this, new TargetEventArgs(true,"Enemy CRIT!", Damage*2, RandFactor*2, EvasionChance, DMGBuffModificator:1, CritModifier:2, target.Armor*2));
                        target.Health -= Convert.ToInt32((((Damage + RandFactor) * 2) + ((Damage + RandFactor) * 2 * target.Armor / 1000)));
                    }
                    else
                    {
                        EnemyDamageInfo?.Invoke(this, new TargetEventArgs(true,"Enemy CRIT!", Damage, RandFactor, EvasionChance, DMGBuffModificator:1, CritModifier:2, target.Armor * 2));
                        target.Health -= Convert.ToInt32(((Damage + RandFactor) + (target.Armor*2 / 1000 * (Damage + RandFactor))));
                    }
                }
            }
        }

        public int CompareTo(IShowInfo other)
        {
            Enemy temp = other as Enemy;
            switch (IShowInfo.Choice)
            {
                case ConsoleKey.D1:
                    if (Health < temp.Health)
                        return 1;
                    else
                        return -1;
                case ConsoleKey.D2:
                    if (Damage < temp.Damage)
                        return 1;
                    else
                        return -1;
                case ConsoleKey.D3:
                    if (EnemyName[0] < temp.EnemyName[0])
                        return -1;
                    else
                        return 1;
                default:
                    return 1;
            }
        }

        public static Enemy[] Enemies = new Enemy[] {new Gnome(), new Goblin(), new Orc() };
        public Enemy(DifficultyLevel difficultyLevel)
        {
            if (difficultyLevel == DifficultyLevel.Hard)
                EnabledAbility = true;
            else
                EnabledAbility = false;
        }
    }

    class Gnome:Enemy
    {
        public Gnome(DifficultyLevel difficultyLevel=DifficultyLevel.Easy):base(difficultyLevel)
        {
            EnemyName = "Gnome";
            _Damage = 33;
            _Damage += Convert.ToInt32((_Damage * (int)difficultyLevel) / 100);
            _Health = 300;
            _Health += Convert.ToInt32((_Health * (int)difficultyLevel) / 100);
            _EvasionChance = 20;
            _CritChance = 25;
            _Armor = 180;
            _Armor += Convert.ToInt32((_Armor * (int)difficultyLevel) / 100);
            _MissChance = 10;
        }

        public override void ShowInfo()
        {
            GetEnemyInfo(this);
            Console.WriteLine("\tPassive<Dexterous hands>-takes one potion from your backpack (Once per battle)");
            Console.WriteLine("-------------------------------------------------");
        }

        public override void UniqueAbility(Player player)
        {
            AbilityCD = 9999;
            Console.WriteLine("Gnome steals a potion!");
            Random rand=new Random();
            player.Invenotry.RemoveAt(rand.Next(1, player.Invenotry.Count));

        }
    }

    class Orc : Enemy
    {
        public Orc(DifficultyLevel difficultyLevel = DifficultyLevel.Easy) : base(difficultyLevel)
        {
            EnemyName = "Orc";
            _Damage = 48;
            _Damage += Convert.ToInt32((_Damage * (int)difficultyLevel) / 100);
            _Health = 350;
            _Health += Convert.ToInt32((_Health * (int)difficultyLevel) / 100);
            _EvasionChance = 10;
            _CritChance = 12;
            _Armor = 350;
            _Armor += Convert.ToInt32((_Armor * (int)difficultyLevel) / 100);
            _MissChance = 27;
        }

        public override void ShowInfo()
        {
            GetEnemyInfo(this);
            Console.WriteLine("\tActive<Thunder tread>-stuns you for 1 tures (Once per 4 turnes)");
            Console.WriteLine("-------------------------------------------------");
        }

        public override void UniqueAbility(Player player)
        {
            if (player.ClassName == "Warrior")
            {
                Random rand = new Random();
                int StunChance = rand.Next(1, 2);
                if (StunChance == 1)
                {
                    Console.WriteLine("Enemy stuns you");
                    player.Stun = 2;
                }
                else
                    Console.WriteLine("Enemy tried to stun you");

            }
            else
            {
                Console.WriteLine("Enemy stuns you!");
                player.Stun = 2;
            }
            AbilityCD = 4;
        }
    }

    class Goblin : Enemy
    {
        public Goblin(DifficultyLevel difficultyLevel = DifficultyLevel.Easy) : base(difficultyLevel)
        {
            EnemyName = "Goblin";
            _Damage = 32;
            _Damage += Convert.ToInt32((_Damage * (int)difficultyLevel) / 100);
            _Health = 250;
            _Health+= Convert.ToInt32((_Health * (int)difficultyLevel) / 100);
            _EvasionChance = 27;
            _CritChance = 30;
            _Armor = 150;
            _Armor+= Convert.ToInt32((_Armor * (int)difficultyLevel) / 100);
            _MissChance = 5;

        }

        public override void ShowInfo()
        {
            GetEnemyInfo(this);
            Console.WriteLine("\tPassive<Vigorous>-Has imune to stuns");
            Console.WriteLine("-------------------------------------------------");
        }

        public override void UniqueAbility(Player player) { AbilityCD = 9999; DealDamage(player); }
    }
}
