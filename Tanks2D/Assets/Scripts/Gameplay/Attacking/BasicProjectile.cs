using System;
using Core.ServiceLocator;
using Enums;
using Gameplay.UpdatingInstance;
using UnityEngine;

namespace Gameplay.Attacking
{
    public class BasicProjectile : MonoBehaviour, IUpdatable
    {
        [SerializeField] protected float _lifeTime = 5f;
        protected Vector2 StartDirection;
        private GameInstance _gameInstance;
        public ProjectileID ProjectileID { get; private set; }
        public float CurrentLifeTime { get; private set; }
        public Team Team { get; private set; }

        public bool IgnorePause { get; } = false;

        private void Awake()
        {
            _gameInstance = Services.Get<GameInstance>();
        }

        private void OnEnable()
        {
            _gameInstance.TryAddNewUpdatable(this);
        }

        private void OnDisable()
        {
            _gameInstance.AddToUnsubUpdate(this);
        }

        public virtual void Init(Team team, ProjectileID projectileID)
        {
            CurrentLifeTime = _lifeTime;
            ProjectileID = projectileID;
            Team = team;
            StartDirection = transform.up;
        }

        public void RestoreLifetime(float lifetime)
        {
            CurrentLifeTime = Mathf.Clamp(lifetime, 0f, _lifeTime);
        }

        public virtual void ApplyHit(Vehicle tank)
        {
            Despawn();
        }

        public virtual void OnUpdate()
        {
            CurrentLifeTime -= Time.deltaTime;
            if (CurrentLifeTime <= 0)
                Despawn();
        }

        public virtual void Despawn()
        {
            if (!gameObject.activeInHierarchy)
                return;
            _gameInstance.AddToUnsubUpdate(this);
            gameObject.SetActive(false);
            Deactivated?.Invoke(this, true);
        }

        private void OnDestroy()
        {
            _gameInstance.AddToUnsubUpdate(this);
            Deactivated = null;
        }

        public Action<BasicProjectile, bool> Deactivated;
    }
}
