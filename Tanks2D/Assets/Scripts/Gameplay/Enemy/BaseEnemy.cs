using Core;
using Enums;
using Gameplay.UpdatingInstance;
using UnityEngine;

namespace Gameplay.Enemy
{
    public abstract class BaseEnemy : MonoBehaviour, IUpdatable
    {
        public bool IgnorePause { get; } = false;
        public abstract EnemyType EnemyType { get; }
        public abstract void OnUpdate();

        protected virtual void OnCollision(Collision2D collision)
        {
            GameObject obj = collision.gameObject;
            if (obj.CompareTag(Constants.Player))
            {
                if (collision.gameObject.TryGetComponent<Vehicle>(out var player))
                {
                    player.BasicUnitHealth.TakeLethalDamage();
                    Reset();
                }
            }
        }

        protected abstract void Reset();
    }
}