using System;

namespace UnitSystem
{
    [Flags]
    public enum ERelativePhase
    {
        None = 0,
        Same = 1,
        Opposite = 2,
        All = Same | Opposite,
    }
}