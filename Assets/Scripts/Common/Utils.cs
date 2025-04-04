using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnitSystem;

namespace Common
{
    public static class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="phase">This should return 0 for first phase, 1 for second one, 0 and 1 for both etc... Scales if we extend enum</param>
        /// <returns></returns>
        public static IEnumerable<int> FlagIndexes(EPhase phase)
        {
            int index = 0;
            foreach (EPhase value in Enum.GetValues(typeof(EPhase)))
            {
                var isPowerOfTwo = (value & (value - 1)) == 0; //To ignore "conpound values" and only use atomic ones
                if (value != 0 && isPowerOfTwo && phase.HasFlag(value))
                {
                    yield return index - 1;
                }

                index++;
            }
        }

        public static string BattleElementToSimpleString(IBattleElement unit, bool displayHealth = false)
        {
            if (unit == null)
                return "        ";
            return $"{PhaseToChar(unit.Phase)}{TeamToChar(unit.Team)} " +
                   (displayHealth ? $"{unit.CurrentHealth:D2}/{unit.MaxHealth:D2}" : "     ");
        }

        public static string BattleElementToString(IBattleElement element, bool displayHealth = false)
        {
            if (element == null)
                return "";
            return $"({element.Position}):{BattleElementToSimpleString(element)}";
        }

        public static string TeamToChar(ETeam team)
        {
            switch (team)
            {
                case ETeam.Player:
                    return "P";
                case ETeam.Enemy:
                    return "H";
                default:
                    return " ";
            }
        }

        public static string PhaseToChar(EPhase phase)
        {
            switch (phase)
            {
                case EPhase.Normal:
                    return "N";
                case EPhase.Ethered:
                    return "E";
                case EPhase.Both:
                    return "B";
                default:
                    return "x";
            }
        }

        public static char WalkTypeToChar(EAllowedMovement bAllowedMovement)
        {
            
            switch(bAllowedMovement)
            {
                case EAllowedMovement.Nothing:
                    return 'X';
                case EAllowedMovement.Cross:
                    return '-';
                case EAllowedMovement.Stop:
                    return 'o';
                default:
                    return '?';
            }
        }
    }
}