using System;
using Unity.VisualScripting;

namespace BattleSystem
{
    //We use FLag enums so we can have implicitely "no phase" as value 0 and both phase as value 3, make sure that really new values are power of 2, other values that are not power of twos are juste equivalent of all the sum of the power of two that are in the value like Both = 3 = 2 + 1 = Normal + Ethered
    [Flags]
    public enum EPhase : byte
    {
        None = 0,
        Normal = 1,
        Ethered = 2,
        Both = Normal | Ethered,

        Max = 128, //This way "everyting" is 7!= both and we can use serialized both correctly
    }

    public static class EnumExtensions
    {
        public static EPhase Both => EPhase.Normal | EPhase.Ethered;

        public static string ToFancyString(this EPhase phase, bool bothIsAny = false)
        {
            if (phase == EPhase.Both && bothIsAny)
                return "Any";
            if (phase == EPhase.Normal)
                return "Real";
            return phase.ToString();
        }

        public static bool IsOnlyOnOnePhase(this EPhase phase)
        {
            //Is power of 2
            return (phase & (phase - 1)) == 0;
        }

        public static bool Intersects(this EPhase _phase, EPhase other)
        {
            return (_phase & other) != 0b0;
        }
    }
}