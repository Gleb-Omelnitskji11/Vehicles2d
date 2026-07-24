using System;
using UnityEngine;

namespace Gameplay
{
    public class BasicUnitHealth
    {
        private int MaxHp { get; }
        public int CurrentHp { get; private set; }

        public BasicUnitHealth(int maxHp)
        {
            MaxHp = maxHp;
        }

        public void TakeDamage(int damage)
        {
            CurrentHp = Mathf.Max(0, CurrentHp - damage);

            if (CurrentHp == 0)
            {
                OnDeath?.Invoke();
            }
            else
            {
                OnTakeDamage?.Invoke();
            }
        }
        
        public void TakeLethalDamage()
        {
            TakeDamage(MaxHp);
        }

        public void Restore(int value)
        {
            CurrentHp = Mathf.Min(MaxHp, value);
        }

        public void ResetData()
        {
            CurrentHp = MaxHp;
        }

        public event Action OnDeath;
        public event Action OnTakeDamage;
    }
}