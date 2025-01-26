using BattleSystem;
using UnitSystem;

namespace Common
{
    public static class Utils
    {
        public static string UnitToSimpleString(Unit unit)
        {
            if(unit==null)
                return "  ";
            return $"{PhaseToChar(unit.Phase)}{TeamToChar(unit.Team)}";
        }
        public static string BattleElementToString(IBattleElement element)
        {
            if(element==null)
                return "";
            return $"({element.Position}):{PhaseToChar(element.Phase)}{TeamToChar(element.Team)}";
        }
        public static string TeamToChar(ETeam team)
        {
            switch (team)
            {
                case ETeam.Player:
                    return "P";
                case ETeam.Enemy:
                    return "E";
                default:
                    return "x";
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
    }
}