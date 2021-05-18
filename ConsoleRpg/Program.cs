using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Laba5
{
    class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", CharSet=CharSet.Auto,SetLastError =true)]
        private static extern IntPtr ShowWindow(IntPtr hWnd,int cmdShow);
        private const int MAXIMAZE = 3;

        delegate void PlayerAndEnemyInfo(Player player, Enemy enemy);
        delegate void WarningMessage(string message);
        static WarningMessage warningMessage;
        static void DealDamageInfo(object sender,TargetEventArgs e)
        {
            if(e.TheDamagePassed==false)
                Console.WriteLine(e.TypeOfHit);
            else 
                Console.WriteLine($"{e.TypeOfHit} -{Convert.ToInt32(e.Damage + e.RandFactor + e.TargetArmorRate / 1000 * (e.Damage + e.RandFactor))} ({Convert.ToInt32(e.TargetArmorRate / 1000 * (e.Damage + e.RandFactor))} Blocked)");           
        }
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(GetConsoleWindow(), MAXIMAZE);

            warningMessage = (message) =>
              {
                  Console.ForegroundColor = ConsoleColor.Red;
                  Console.WriteLine(message);
                  Console.ResetColor();
                  System.Threading.Thread.Sleep(1300);
                  Console.Clear();
              };
            Enemy.EnemyDamageInfo += DealDamageInfo;
            Player.PlayerDamageInfo += DealDamageInfo;
            Introduction();
        }

        static void Introduction()
        {
            Console.Clear();
            Console.WriteLine("This is small console game where you will be offered to choose class, your weapon and some other parts of equipment");
            Console.WriteLine("After that you have to defeat your enemy which will be determined with help of the random");
            Console.WriteLine("During the battle you can your class abilities, potions and simple strikes with help of the weapon\n");
            Console.WriteLine("Press any button to continue or Backspace if you decided not to start");
            ConsoleKey consoleKey = Console.ReadKey().Key;
            switch (consoleKey)
            {
                case ConsoleKey.Backspace:
                    return;
                default:
                    GetPlayerInfo();
                    break;
            }
        }

        static void BattleProcess(Player player,Enemy enemy)
        {
            if(player.UsingWeapon.WeaponName=="Bow")
            {
                BattleInterface(player, enemy);
                PerformPlayerAction(player, enemy);
                System.Threading.Thread.Sleep(1300);
                Console.Clear();
            }
            while(player.Health>0&&enemy.Health>0)
            {
                BattleInterface(player, enemy);
                PerformPlayerAction(player, enemy);
                PerformEnemyAction(player, enemy);
                player.ReduceCD();
                enemy.ReduceCD();
                System.Threading.Thread.Sleep(1300);
                Console.Clear();
            }
            if ((player.Health <= 0) && (enemy.Health > 0))
            {
                BattleInterface(player, enemy);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You were defeated....");
                Console.ResetColor();
            }
            else if ((enemy.Health <= 0) && (player.Health > 0))
            {
                BattleInterface(player, enemy);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You have defeated your enemy!!");
                Console.ResetColor();
            }
            else
            {
                BattleInterface(player, enemy);
                Console.WriteLine("You are both dead...");
            }
            return;
        }

        static void BattleInterface(Player player,Enemy enemy)
        {
            Console.Write($"Health:{player.Health}\t\tEnemy Health:{enemy.Health}\t|\t ↑-For attack ↓-For special ability Your inventory:"); player.ShowInventory();
            Console.WriteLine($"\t\t\t\t\t\t\t Potion Cooldown:{player.PotionCD} Ability Cooldown:{player.AbilityCD} Damage Buff:{player.BuffCD}");
        }

        static void PerformPlayerAction(Player player, Enemy enemy)
        {
            if (player.Stun == 0)
            {
                ConsoleKey consoleKey = Console.ReadKey().Key;
                if (consoleKey == ConsoleKey.UpArrow)
                    player.DealDamage(enemy);
                else if (consoleKey == ConsoleKey.DownArrow)
                {
                    if (player.AbilityCD == 0)
                        player.UniqueAbility(enemy);
                    else
                    {
                        warningMessage?.Invoke("Ability is on coooldown");
                        BattleInterface(player, enemy);
                        PerformPlayerAction(player, enemy);
                    }

                }
                else if (((int)consoleKey - 48 > 0) && ((int)consoleKey - 48) <= player.Invenotry.Count || player.Invenotry.Count == 0)
                {
                    if (player.PotionCD == 0)
                    {
                        player.Invenotry[(int)consoleKey - 49].UniqueEffect(player);
                        player.Invenotry.RemoveAt((int)consoleKey - 49);
                    }
                    else if (player.PotionCD != 0)
                    {
                        warningMessage?.Invoke("Potion is on cooldown");
                        BattleInterface(player, enemy);
                        PerformPlayerAction(player, enemy);
                    }
                    else if (player.Invenotry.Count == 0)
                    {
                        warningMessage?.Invoke("There are no potions left");
                        BattleInterface(player, enemy);
                        PerformPlayerAction(player, enemy);
                    }
                }
                else
                {
                    warningMessage?.Invoke("Wrong option");
                    BattleInterface(player, enemy);
                    PerformPlayerAction(player, enemy);
                }
                
            }
            else
            {
                int IndexInInvenotry = -1;
                for(int i=0;i<player.Invenotry.Count;i++)
                {
                    if(player.Invenotry[i].Name == "Clear Potion")
                    {
                        Console.WriteLine("You have a clear potion in your inventory");
                        Console.WriteLine($"You can use it to dispel stun, if you want to do it press {i+1}");
                        IndexInInvenotry = i;
                        break;
                    }
                }
                if (IndexInInvenotry != -1)
                {
                    ConsoleKey consoleKey = Console.ReadKey().Key;
                    if((int)consoleKey-49==IndexInInvenotry)
                    {
                        player.Invenotry[IndexInInvenotry].UniqueEffect(player);
                        player.Invenotry.RemoveAt(IndexInInvenotry);
                        Console.Clear();
                        BattleInterface(player, enemy);
                        PerformPlayerAction(player, enemy);
                    }
                }
                else
                {
                    Console.WriteLine("Player is in stun");
                    System.Threading.Thread.Sleep(1300);
                }
            }
        }
        static void PerformEnemyAction(Player player,Enemy enemy)
        {
            if ((enemy.Stun == 0)&&(player.Invise==0)&&(enemy.AbilityCD==0)&&(enemy.EnabledAbility==true))
                enemy.UniqueAbility(player);
            else if ((enemy.Stun == 0) && (player.Invise == 0) && ((enemy.AbilityCD != 0)||(enemy.EnabledAbility==false)))
                enemy.DealDamage(player);
            else if(player.Invise!=0)
                Console.WriteLine("Player is invisible");
            else if (enemy.Stun != 0)            
               Console.WriteLine("Enemy is in stun");
            
        }

        static void CreatingCharacter(PlayerAndEnemyInfo Info)
        {
            Console.Clear();
            Console.WriteLine("Choose the level of difficulty");
            Console.WriteLine("\t1-Easy. Enemies have normal amount of health,damage and armor");
            Console.WriteLine("\t2-Moderate. Enemies have +20% to thier health,damage and armor");
            Console.WriteLine("\t3-Hard. Enemies have +25% to thier health,damage and armor,they can use abilities\n");
            ConsoleKey Choice = Console.ReadKey().Key;
            DifficultyLevel difficultyLevel = 0;
            switch (Choice)
            {
                case ConsoleKey.D1:
                    difficultyLevel = DifficultyLevel.Easy;
                    break;
                case ConsoleKey.D2:
                    difficultyLevel = DifficultyLevel.Moderate;
                    break;
                case ConsoleKey.D3:
                    difficultyLevel = DifficultyLevel.Hard;
                    break;
                default:
                    warningMessage?.Invoke("You have chosen wrong option...\nTry again");
                     CreatingCharacter(Info);
                    break;
            }
            Console.Clear();
            Player CurrentPlayer = ChoosingClass();
            ChoosingWeapon(CurrentPlayer);
            ChoosingPotions(CurrentPlayer);
            Enemy CurrentEnemy = ChoosingEnemies(difficultyLevel);
            Info(CurrentPlayer, CurrentEnemy);
            Console.Write("Press any key to continue...");
            Console.ReadLine();
            Console.Clear();
            BattleProcess(CurrentPlayer, CurrentEnemy);
            Environment.Exit(0);            
        }  
        
        static Player ChoosingClass()
        {
            while (true)
            {
                Console.WriteLine("Choosing class:");
                Console.WriteLine("\t1-Warrior");
                Console.WriteLine("\t2-Mage");
                Console.WriteLine("\t3-Rogue");
                Console.WriteLine("\t4-Hunter");
                ConsoleKey Choice = Console.ReadKey().Key;
                switch (Choice)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        return new Warrior();
                    case ConsoleKey.D2:
                        Console.Clear();
                        return new Mage();
                    case ConsoleKey.D3:
                        Console.Clear();
                        return new Rogue();
                    case ConsoleKey.D4:
                        Console.Clear();
                        return new Hunter();
                    default:
                        warningMessage?.Invoke("You have chosen wrong option...\nTry again");
                        continue;
                }
            }
        }
        static void ChoosingWeapon(Player CurrentPlayer)
        {
            bool ChoosingWeaponProccess = true;
            while (ChoosingWeaponProccess)
            {
                Console.WriteLine("Chosing weapon:");
                Console.WriteLine("\t1-Dagger");
                Console.WriteLine("\t2-Fist");
                Console.WriteLine("\t3-Two Hand Axe");
                Console.WriteLine("\t4-Staff");
                Console.WriteLine("\t5-PoleArm");
                if (CurrentPlayer.ClassName == "Hunter")
                    Console.WriteLine("\t6-Bow (only for hunters)");
                ConsoleKey Choice = Console.ReadKey().Key;
                switch (Choice)
                {
                    case ConsoleKey.D1:
                        CurrentPlayer.UsingWeapon = new Dagger();
                        CurrentPlayer.ReduceCapacity();
                        ChoosingWeaponProccess = false;
                        break;
                    case ConsoleKey.D2:
                        CurrentPlayer.UsingWeapon = new Fist();
                        CurrentPlayer.ReduceCapacity();
                        ChoosingWeaponProccess = false;
                        break;
                    case ConsoleKey.D3:
                        CurrentPlayer.UsingWeapon = new TwoHandAxe();
                        CurrentPlayer.ReduceCapacity();
                        CurrentPlayer.ReduceCapacity();
                        ChoosingWeaponProccess = false;
                        break;
                    case ConsoleKey.D4:
                        CurrentPlayer.UsingWeapon = new Staff();
                        CurrentPlayer.ReduceCapacity();
                        CurrentPlayer.ReduceCapacity();
                        ChoosingWeaponProccess = false;
                        break;
                    case ConsoleKey.D5:
                        CurrentPlayer.UsingWeapon = new PoleArm();
                        CurrentPlayer.ReduceCapacity();
                        CurrentPlayer.ReduceCapacity();
                        ChoosingWeaponProccess = false;
                        break;
                    case ConsoleKey.D6:
                        if (CurrentPlayer.ClassName == "Hunter")
                        {
                            CurrentPlayer.UsingWeapon = new Bow();
                            CurrentPlayer.ReduceCapacity();
                            CurrentPlayer.ReduceCapacity();
                            ChoosingWeaponProccess = false;
                            break;
                        }
                        else
                        {
                            warningMessage?.Invoke("Wrong option...\nTry again");
                            continue;
                        }
                    default:
                        warningMessage?.Invoke("Wrong option...\nTry again");
                        continue;
                }
            }
            if (CurrentPlayer.UsingWeapon.OneHand == true)
            {
                Console.WriteLine("You have chosen one-hand weapon");
                Console.WriteLine("you have opportunity to put on a shield or take one more potion in your inventory\n");
                Console.WriteLine("Shield's characteristics:");
                Console.WriteLine("\tDoubles your armor raiting");
                Console.WriteLine("\tGive you 10% chance to BLOCK enemy heat (you will receive 0 damage)\n");
                Console.WriteLine("Press Space to equip shield or another button to continue");
                ConsoleKey consoleKey = Console.ReadKey().Key;
                if (consoleKey == ConsoleKey.Spacebar)
                {
                    CurrentPlayer.UsingShield = true;
                    CurrentPlayer.ReduceCapacity();
                }
            }
            Console.Clear();
        }
        static void ChoosingPotions(Player CurrentPlayer)
        {
            bool ChoosingPotionsProcess = true;
            while (ChoosingPotionsProcess)
            {
                while (CurrentPlayer.Capacity != 0)
                {
                    Console.WriteLine("Chosing potions:");
                    Console.WriteLine("\t1-Healing Potion");
                    Console.WriteLine("\t2-Rage Potion");
                    Console.WriteLine("\t3-Clear Potion");
                    Console.WriteLine($"Inventory capacity:{CurrentPlayer.Capacity}");
                    ConsoleKey Choice = Console.ReadKey().Key;
                    switch (Choice)
                    {
                        case ConsoleKey.D1:
                            CurrentPlayer.ReduceCapacity();
                            CurrentPlayer.Invenotry.Add(new HealingPotion());
                            break;
                        case ConsoleKey.D2:
                            CurrentPlayer.ReduceCapacity();
                            CurrentPlayer.Invenotry.Add(new RagePotion());
                            break;
                        case ConsoleKey.D3:
                            CurrentPlayer.ReduceCapacity();
                            CurrentPlayer.Invenotry.Add(new ClearPotion());
                            break;
                        default:
                            warningMessage?.Invoke("You have chosen wrong option...\nTry again");
                            continue;
                    }
                    Console.Clear();
                }
                Console.WriteLine("Your potions:");
                CurrentPlayer.ShowInventory();
                Console.WriteLine();
                Console.WriteLine("Do you want to choose potions again?");
                Console.WriteLine("Press any button if no");
                Console.WriteLine("Press Backspace if yes");
                ConsoleKey consoleKey = Console.ReadKey().Key;
                switch (consoleKey)
                {
                    case ConsoleKey.Backspace:
                        Console.Clear();
                        CurrentPlayer.RenewCapacity();
                        continue;
                    default:                       
                        Console.Clear();
                        ChoosingPotionsProcess = false;
                        break;
                }
            }
        }
        static Enemy ChoosingEnemies(DifficultyLevel difficultyLevel)
        {
            Random rand = new Random();
            int Choice = rand.Next(1, 3);
            switch (Choice)
            {
                case 1:
                    return new Orc(difficultyLevel);
                case 2:
                    return new Goblin(difficultyLevel);
                case 3:
                    return new Gnome(difficultyLevel);
            }
            return new Orc(difficultyLevel);
        }

        static void GetPlayerInfo()
        {
            Console.Clear();
            ConsoleKey consoleKey;
            GetInfo(Player.Players);
            Console.WriteLine("Press \"1\" to sort by Health");
            Console.WriteLine("Press \"2\" to sort by Name");
            Console.WriteLine("Press Enter to continue...");
            consoleKey = Console.ReadKey().Key;
            if (consoleKey == ConsoleKey.Enter)
                GetWeaponInfo();
            else if (consoleKey == ConsoleKey.Backspace)
                Introduction();
            else if ((consoleKey == ConsoleKey.D1) || (consoleKey == ConsoleKey.D2))
            {
                Console.Clear();
                IShowInfo.Choice = consoleKey;
                GetPlayerInfo();
            }
            else
            {
                Console.WriteLine("Wrong input value...");
                return;
            }
        }
        static void GetWeaponInfo()
        {
            Console.Clear();
            ConsoleKey consoleKey;
            GetInfo(Weapon.Weapons);
            Console.WriteLine("Press \"1\" to sort by Damage");
            Console.WriteLine("Press \"2\" to sort by Name");
            Console.WriteLine("Press Enter to continue...");
            consoleKey = Console.ReadKey().Key;
            if (consoleKey == ConsoleKey.Enter)
                GetPotionsInfo();
            else if (consoleKey == ConsoleKey.Backspace)
                GetPlayerInfo();
            else if((consoleKey==ConsoleKey.D1)||(consoleKey==ConsoleKey.D2))
            {
                Console.Clear();
                IShowInfo.Choice = consoleKey;
                GetWeaponInfo();
            }
            else
            {
                Console.WriteLine("Wrong input value...");
                return;
            }
        }
        static void GetPotionsInfo()
        {
            Console.Clear();
            ConsoleKey consoleKey;
            GetInfo(Potion.Potions);
            Console.WriteLine("Press Enter to continue...");
            consoleKey = Console.ReadKey().Key;
            if (consoleKey == ConsoleKey.Enter)
                GetEnemyInfo();
            else if (consoleKey == ConsoleKey.Backspace)
                GetWeaponInfo();
            else
            {
                Console.WriteLine("Wrong input value...");
                return;
            }
        }
        static void GetEnemyInfo()
        {
            Console.Clear();
            ConsoleKey consoleKey;
            GetInfo(Enemy.Enemies);
            Console.WriteLine("Press \"1\" to sort by Damage");
            Console.WriteLine("Press \"2\" to sort by Health");
            Console.WriteLine("Press \"3\" to sort by Name");
            Console.WriteLine("Press Enter to continue...");
            consoleKey = Console.ReadKey().Key;
            if (consoleKey == ConsoleKey.Enter)
                CreatingCharacter((CurrentPlayer,CurrentEnemy)=> 
                {
                    Console.WriteLine($"\t\t{CurrentPlayer.ClassName}\t\t{CurrentEnemy.EnemyName}");
                    Console.WriteLine($"Health:\t\t{CurrentPlayer.Health}\t\t{CurrentEnemy.Health}");
                    Console.WriteLine($"Damage:\t\t{CurrentPlayer.Damage}\t\t{CurrentEnemy.Damage}");
                    Console.WriteLine($"Evade Chance:\t{CurrentPlayer.EvadeChance}%\t\t{CurrentEnemy.EvasionChance}%");
                    if (CurrentPlayer.UsingShield == true)
                    {
                        Console.WriteLine($"Armor rate:\t{CurrentPlayer.Armor * 2}\t\t{CurrentEnemy.Armor}");
                        Console.WriteLine($"Block chance:\t10%");
                    }
                    else
                    {
                        Console.WriteLine($"Armor rate:\t{CurrentPlayer.Armor}\t\t{CurrentEnemy.Armor}");
                    }
                }
                );
            else if ((consoleKey == ConsoleKey.D1) || (consoleKey == ConsoleKey.D2) || (consoleKey == ConsoleKey.D3))
            {
                Console.Clear();
                IShowInfo.Choice = consoleKey;
                GetEnemyInfo();
            }
            else if (consoleKey == ConsoleKey.Backspace)
                GetPotionsInfo();
            else
            {
                Console.WriteLine("Wrong input value...");
                return;
            }
        }

        static void GetInfo(IShowInfo[]items)
        {
            Array.Sort(items);
            for(int i=0;i<items.Length;i++)
            {
                items[i].ShowInfo();
            }
        }
    }
}
