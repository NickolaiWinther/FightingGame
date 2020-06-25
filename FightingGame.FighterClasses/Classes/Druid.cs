using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FightingGame.Entities.Classes
{
    public class Druid : FighterBase
    {
        public Druid(string name)
            : base(name)
        {
            MaxHealth = 300;
            CurrentHealth = MaxHealth;
            Armor = 0.15;
            Power = 30;

            Abilities.Add(new Ability()
            {
                Name = "Weak attack",
                Modifier = 1.3,
                ManaCost = 10,
                Type = AbilityType.Attack
            });
            Abilities.Add(new Ability()
            {
                Name = "Strong attack",
                Modifier = 2.4,
                ManaCost = 30,
                Type = AbilityType.Attack
            });
            Abilities.Add(new Ability()
            {
                Name = "Weak heal",
                Modifier = 1.4,
                ManaCost = 20,
                Type = AbilityType.Heal
            });
            Abilities.Add(new Ability()
            {
                Name = "Strong heal",
                Modifier = 3,
                ManaCost = 50,
                Type = AbilityType.Heal
            });
        }
    }
}
