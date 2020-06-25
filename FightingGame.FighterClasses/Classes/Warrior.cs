using System;
using System.Collections.Generic;
using System.Text;

namespace FightingGame.Entities.Classes
{
    public class Warrior : FighterBase
    {
        public Warrior(string name)
            : base(name)
        {
            MaxHealth = 350;
            CurrentHealth = MaxHealth;
            Armor = 0.20;
            Power = 20;

            Abilities.Add(new Ability()
            {
                Name = "Slash",
                Modifier = 1.3,
                ManaCost = 10,
                Type = AbilityType.Attack
            });
            Abilities.Add(new Ability()
            {
                Name = "Medium Attack",
                Modifier = 2.4,
                ManaCost = 30,
                Type = AbilityType.Attack
            });
            Abilities.Add(new Ability()
            {
                Name = "Super Attack",
                Modifier = 5,
                ManaCost = 80,
                Type = AbilityType.Attack
            });
        }
    }
}
