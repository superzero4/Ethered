using System;
using UnityEngine;

namespace BattleSystem
{
    public interface IHealth
    {
        int CurrentHealth { get; }
        int MaxHealth { get; }
        void TakeDamage(int damage);

        void Heal(int heal)
        {
            TakeDamage(-heal);
        }
    }
}