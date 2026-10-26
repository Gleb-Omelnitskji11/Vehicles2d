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
            MaxHp = Mathf.Max(1, maxHp);
            CurrentHp = MaxHp;
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || CurrentHp <= 0)
                return;

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
            CurrentHp = Mathf.Clamp(value, 0, MaxHp);
        }

        public void ResetData()
        {
            CurrentHp = MaxHp;
        }

        public event Action OnDeath;
        public event Action OnTakeDamage;
    }
}
