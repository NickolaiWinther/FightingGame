using FightingGame.Entities;
using FightingGame.Entities.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace FightingGame.UI
{
    class Program
    {
        public static List<List<FighterBase>> Teams { get; set; }
        public static List<FighterBase> Team1 { get; set; }
        public static List<FighterBase> Team2 { get; set; }
        static void Main(string[] args)
        {
            do
            {
                Team1 = new List<FighterBase>();
                Team2 = new List<FighterBase>();
                Teams = new List<List<FighterBase>>() { Team1, Team2 };
                Console.Clear();
                Team1.Add(CreateFighter("Team 1 - Player 1"));
                Team1.Add(CreateFighter("Team 1 - Player 2"));
                Team2.Add(CreateFighter("Team 2 - Player 1"));
                Team2.Add(CreateFighter("Team 2 - Player 2"));
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Team 1:");
                Console.ForegroundColor = ConsoleColor.Blue;
                ShowFighters(Team1);

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("\nTeam 2:");
                Console.ForegroundColor = ConsoleColor.Blue;
                ShowFighters(Team2);

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nPress any key to start the epic battle!");
                Console.ReadKey();

                StartBattle();

                List<FighterBase> winningTeam = new List<FighterBase>();

                if (Teams[0].Any(f => f.CurrentHealth > 0))
                {
                    winningTeam = Teams[0];
                }
                else
                {
                    winningTeam = Teams[1];
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{winningTeam[0].Name} and {winningTeam[1].Name} won the epic battle!\n");

                Console.ResetColor();
                Console.WriteLine("Press escape to exit \nPress anything else to play again");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }


        public static FighterBase CreateFighter(string player)
        {
            string name = "";

            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Clear();
                Console.Write($"{player}\nInsert name: ");
                name = Console.ReadLine();
            }
            

            while (true)
            {
                Console.Clear();
                Console.WriteLine("" +
                    $"{player} \n" +
                    "Select class: \n" +
                    "1) Warrior \n" +
                    "2) Druid");

                switch (Console.ReadLine())
                {
                    case "1":
                        return new Warrior(name);

                    case "2":
                        return new Druid(name);
                }
            }
        }

        public static void ShowFighters(List<FighterBase> fighters)
        {
            foreach (FighterBase fighter in fighters)
            {
                Console.WriteLine(fighter);
            }
        }

        private static void StartBattle()
        {
            List<FighterBase> turnOrder = new List<FighterBase>() 
            { 
                Team1[0],
                Team2[0],
                Team1[1],
                Team2[1]
            };
            int counter = 0;
            while (true)
            {
                Console.Clear();
                StatusBars();
                while (turnOrder[counter%4].CurrentHealth == 0)
                {
                    counter++;
                }
                SelectAbility(turnOrder[counter%4]);
                if (Teams.Any(t => t.All(f => f.CurrentHealth == 0)))
                {
                    break;
                }
                counter++;
            }

        }

        private static void SelectAbility(FighterBase fighter)
        {
            Console.WriteLine($"{fighter.Name}, Choose ability!");
            int counter = 1;
            foreach (Ability ability in fighter.Abilities)
            {
                if (ability.ManaCost > fighter.CurrentMana)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                Console.WriteLine($"{counter}) {fighter.AbilityToString(ability)}");

                Console.ResetColor();

                counter++;
            }

            int input;

            do
            {
                int.TryParse(Console.ReadLine(), out input);

            } while (input < 1 || input > fighter.Abilities.Count || fighter.Abilities[input - 1].ManaCost > fighter.CurrentMana);

            Ability selectedAbility = fighter.Abilities[input - 1];

            List<FighterBase> selectedTeam = new List<FighterBase>();

            FighterBase selectedFighter = null;

            switch (selectedAbility.Type)
            {
                case AbilityType.Attack:

                    if (Team1.Contains(fighter))
                    {
                        selectedTeam = Team2;
                    }
                    else
                    {
                        selectedTeam = Team1;
                    }
                    break;

                case AbilityType.Heal:

                    if (Team1.Contains(fighter))
                    {
                        selectedTeam = Team1;
                    }
                    else
                    {
                        selectedTeam = Team2;
                    }
                    break;
                case AbilityType.Buff:
                    selectedFighter = fighter;
                    break;
            }


            

            if (selectedFighter != null || selectedTeam.Any(f => f.CurrentHealth == 0))
            {
                selectedFighter = selectedTeam.FirstOrDefault(f => f.CurrentHealth > 0);
            }
            else
            {
                do
                {
                    Console.WriteLine(
                        $"Who do you want to target? \n" +
                        $"1) {selectedTeam[0]} \n" +
                        $"2) {selectedTeam[1]}");

                    int.TryParse(Console.ReadLine(), out input);

                } while (input < 1 || input > 2);

                selectedFighter = selectedTeam[input - 1];
            }

            fighter.UseAbility(selectedAbility, selectedFighter);

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Press any button to continue");
            Console.ResetColor();
            Console.ReadKey();
        }

        private static void StatusBars()
        {
            Console.Clear();
            foreach (List<FighterBase> team in Teams)
            {
                foreach (FighterBase fighter in team)
                {
                    // Health bar
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"" +
                        $"{fighter} \n" +
                        $"Health --{{");

                    string healthbar = "";

                    double healthChunks = Math.Ceiling( fighter.HealthPercentage / 10 );

                    for (int i = 0; i < healthChunks; i++)
                    {
                        healthbar += "■";
                    }

                    double lostHealthChunks = 10 - healthChunks;

                    for (int i = 0; i < lostHealthChunks; i++)
                    {
                        healthbar += " ";
                    }

                    Console.ForegroundColor = HealthBarColor(fighter);
                    Console.Write(healthbar);

                    Console.ResetColor();
                    Console.WriteLine($"}}-- {fighter.CurrentHealth} / {fighter.MaxHealth}");

                    // Mana bar
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"Mana   --{{");

                    string manabar = "";

                    double manaChunks = Math.Ceiling(fighter.ManaPercentage / 10);

                    for (int i = 0; i < manaChunks; i++)
                    {
                        manabar += "■";
                    }

                    double lostManaChunks = 10 - manaChunks;

                    for (int i = 0; i < lostManaChunks; i++)
                    {
                        manabar += " ";
                    }

                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(manabar);

                    Console.ResetColor();
                    Console.WriteLine($"}}-- {fighter.CurrentMana} / {fighter.MaxMana} \n");
                }
            }
        }

        private static ConsoleColor HealthBarColor(FighterBase fighter)
        {
            if (fighter.HealthPercentage >= 90)
                return ConsoleColor.DarkGreen;
            else if (fighter.HealthPercentage < 90 && fighter.HealthPercentage >= 60)
                return ConsoleColor.Green;
            else if (fighter.HealthPercentage < 60 && fighter.HealthPercentage >= 30)
                return ConsoleColor.Yellow;
            else
                return ConsoleColor.Red;
        }
    }
}
