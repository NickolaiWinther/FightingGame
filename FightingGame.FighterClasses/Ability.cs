using System;
using System.Collections.Generic;
using System.Text;

namespace FightingGame.Entities
{
    public class Ability
    {
        public string Name { get; set; }
        public int ManaCost { get; set; }
        public double Modifier { get; set; }
        public AbilityType Type { get; set; }

        public string GetDescription(double power)
        {
            string description = $"{Name} - {ManaCost} mana - ";

            switch (Type)
            {
                case AbilityType.Attack:
                    return description += $"deals {Modifier * power} damage";
                case AbilityType.Heal:
                    return description += $"heals a friendly target by {Modifier * power}";
                case AbilityType.Buff:
                    return description += $"increase your power by 10%";
                default:
                    return "";
            }
        }
    }
}
