using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Laba5
{
    abstract class Potion: IShowInfo
    {
        protected int _CD;
        protected string _Name;
        public int CD { get { return _CD; } }
        public string Name { get { return _Name; } }
        public abstract void ShowInfo();
        public abstract void UniqueEffect(Player player);

        public int CompareTo(IShowInfo other)
        {
            Potion temp = other as Potion;
            if (_Name[0] < temp._Name[0])
                return -1;
            else
                return 1;
        }

        public static Potion[] Potions = new Potion[] { new HealingPotion(), new RagePotion(), new ClearPotion() };
    }

    class HealingPotion:Potion
    {
        public HealingPotion()
        {
            _CD = 3;
            _Name = "Healing potion";
        }

        public override void UniqueEffect(Player player)
        {
            Random rand = new Random();
            int HealedHealth = rand.Next(1, 35);
            if (((player.Health +(120 + HealedHealth)) <= player.BasicHealth)||(player.ClassName=="Mage"))
            {
                player.Health += (120 + HealedHealth);
                Console.WriteLine($"Healing potion +{120 + HealedHealth}");
            }
            else
            {
                int PlayerHealthCopy = player.Health;
                player.Health = player.BasicHealth;
                Console.WriteLine($"Healing potion +{-PlayerHealthCopy + player.Health}");
                
            }
            player.PotionCD = _CD;
        }

        public override void ShowInfo()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Healing potion's info:");
            Console.ResetColor();
            Console.WriteLine($"\tRestores form 120 to 155 health");
            Console.WriteLine($"\tCooldown:{_CD} turnes");
            Console.WriteLine("-------------------------------------------------");
        }
    }

    class RagePotion : Potion 
    {   
        public RagePotion()
        {
            _CD = 4;
            _Name = "Rage potion";
        }

        public override void UniqueEffect(Player player)
        {
            player.BuffCD = 3;
            player.PotionCD = _CD;
        }

        public override void ShowInfo()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Rage Potion's potion's info:");
            Console.ResetColor();
            Console.WriteLine("\tIncrease your damage on 30% for 3 turnes");
            Console.WriteLine($"\tCooldown:{_CD} turnes");
            Console.WriteLine("-------------------------------------------------");
        }
    }

    class ClearPotion:Potion
    {
        public ClearPotion()
        {
            _CD = 3;
            _Name = "Clear Potion";
        }

        public override void UniqueEffect(Player player)
        {
            player.Stun = 0;
            player.PotionCD = _CD;
        }

        public override void ShowInfo()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Clear potion's info:");
            Console.ResetColor();
            Console.WriteLine("\tRemoves all negative effects");
            Console.WriteLine($"\tColdown:{_CD} turnes");
            Console.WriteLine("-------------------------------------------------");
        }
    }
}
