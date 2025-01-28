using System;
using BattleSystem;

namespace UnitSystem
{
    public enum ERelativePhase
    {
        Same = -1,
        Opposite = -2,
        Normal = EPhase.Normal,
        Ethered = EPhase.Ethered,
        All = EPhase.Both,
    }

    public static class EnumExtensions
    {
        public static EPhase ToPhase(this ERelativePhase relativePhase, EPhase originPhase)
        {
            EPhase phase = EPhase.None;
            if ((int)relativePhase >= 0 && (int)relativePhase < Enum.GetValues(typeof(EPhase)).Length)
            {
                return (EPhase)relativePhase;
            }
            switch (relativePhase)
            {
                case ERelativePhase.Same:
                    phase = originPhase;
                    break;
                case ERelativePhase.Opposite:
                    phase = originPhase == EPhase.Both ? EPhase.Both : originPhase ^ EPhase.Both;
                    break;
                case ERelativePhase.All:
                    phase = EPhase.Both;
                    break;
                default:
                    throw new Exception("Missing either a case in the switch or a value converted from the enum.");
            }

            return phase;
        }
    }
}