using Enums;
using UnityEngine;

namespace Gameplay
{
    public class BasicProjectile : MonoBehaviour
    {
        [SerializeField] protected float _lifeTime = 5f;
        protected Vector2 StartDirection;
        public Team Team { get; private set; }

        public void Init(Team team)
        {
            Team = team;
            StartDirection = transform.up;

            Destroy(gameObject, _lifeTime);
        }

        public virtual void Apply(Tank tank)
        {
            Destroy(gameObject);
        }
    }
}