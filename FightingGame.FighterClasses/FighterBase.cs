using System;
using System.Collections.Generic;
using System.Net;

namespace FightingGame.Entities
{
    public class FighterBase
    {
        public FighterBase(string name)
        {
            Name = name;
            MaxMana = 100;
            CurrentMana = 20;
        }

        public string Name { get; set; }
        public double MaxHealth { get; set; }
        public double CurrentHealth { get; set; } 
        public double Armor { get; set; }
        public double Power { get; set; }
        public double CurrentMana { get; set; }
        public double MaxMana { get; set; }
        public double HealthPercentage { get => CurrentHealth / MaxHealth * 100; }
        public double ManaPercentage { get => CurrentMana / MaxMana * 100; }


        public List<Ability> Abilities { get; set; } = new List<Ability>()
        {
            new Ability()
            {
                Name = "Attack",
                Modifier = 1,
                ManaCost = 0,
                Type = AbilityType.Attack
            },
            new Ability()
            {
                Name = "Increase Power",
                Modifier = 1.10,
                ManaCost = 50,
                Type = AbilityType.Buff
            }
        };        

        public void UseAbility(Ability ability, FighterBase targetFighter)
        {
            switch (ability.Type)
            {
                case AbilityType.Attack:
                    double maxRoll = Power * 1.20;
                    double minRoll = Power * 0.80;
                    Random random = new Random();
                    double damageRoll = random.NextDouble() * (maxRoll - minRoll) + minRoll;

                    double damage = (damageRoll * ability.Modifier) * (1 - targetFighter.Armor);

                    targetFighter.CurrentHealth -= Math.Round(damage);

                    if (targetFighter.CurrentHealth < 0)
                    {
                        targetFighter.CurrentHealth = 0;
                    }

                    // Abilities will always have a modifier
                    if (ability.Modifier == 1)
                    {
                        if (targetFighter.HealthPercentage <= 30)
                        {
                            CurrentMana += 10;
                        }
                        else
                        {
                            CurrentMana += Math.Ceiling( targetFighter.HealthPercentage / 10 ) * 3;
                        }

                        if (CurrentMana > MaxMana)
                            CurrentMana = MaxMana;
                    }
                    else
                    {
                        CurrentMana -= ability.ManaCost;
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(Name);
                    Console.ResetColor();

                    Console.Write(" dealt ");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(Math.Round(damage));
                    Console.ResetColor();

                    Console.Write(" to ");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(targetFighter.Name);
                    Console.ResetColor();


                    break;
                case AbilityType.Heal:
                    CurrentMana -= ability.ManaCost;
                    double heal = Power * ability.Modifier;

                    targetFighter.CurrentHealth += heal;

                    if (targetFighter.CurrentHealth > targetFighter.MaxHealth)
                        targetFighter.CurrentHealth = targetFighter.MaxHealth;

                    break;
                case AbilityType.Buff:
                    Power = Math.Round(Power * ability.Modifier);

                    if (ability.ManaCost != 100)
                        ability.ManaCost += 10;

                    CurrentMana -= ability.ManaCost;

                    break;
            }
        }

        public override string ToString()
        {
            return $"{Name} - {GetType().Name}";
        }

        public string AbilityToString(Ability ability)
        {
            return ability.GetDescription(Power);
        }
    }
}
