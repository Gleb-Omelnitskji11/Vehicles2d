using System;
using System.Collections;
using Enums;
using UnityEngine;

namespace Gameplay
{
    public class BasicProjectile : MonoBehaviour
    {
        [SerializeField] protected float _lifeTime = 5f;
        protected Vector2 StartDirection;
        private Coroutine _lifeTimeCoroutine;
        public Team Team { get; private set; }

        public virtual void Init(Team team)
        {
            Team = team;
            StartDirection = transform.up;
            _lifeTimeCoroutine = StartCoroutine(LifetimeRoutine());
        }

        public virtual void Apply(Tank tank)
        {
            Despawn();
        }

        private IEnumerator LifetimeRoutine()
        {
            yield return new WaitForSeconds(_lifeTime);
            Despawn();
        }

        protected virtual void Despawn()
        {
            if (_lifeTimeCoroutine != null)
            {
                StopCoroutine(_lifeTimeCoroutine);
                _lifeTimeCoroutine = null;
            }

            Die?.Invoke(this);
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            if (_lifeTimeCoroutine != null)
            {
                StopCoroutine(_lifeTimeCoroutine);
                _lifeTimeCoroutine = null;
            }
        }

        private void OnDestroy()
        {
            Die = null;
        }

        public Action<BasicProjectile> Die;
    }
}