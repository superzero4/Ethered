using System;

namespace BattleSystem
{
    //We use FLag enums so we can have implicitely "no phase" as value 0 and both phase as value 3, make sure that really new values are power of 2, other values that are not power of twos are juste equivalent of all the sum of the power of two that are in the value like Both = 3 = 2 + 1 = Normal + Ethered
    [Flags]
    public enum EPhase
    {
        None = 0, Normal= 1, Ethered = 2, Both = Normal | Ethered
        
    }
    public static class EnumExtensions
    {
        public static bool IsOnlyOnOnePhase(this EPhase phase)
        {
            //Is power of 2
            return (phase & (phase - 1)) == 0;
        }
    }
}