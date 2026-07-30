using System;
using System.Collections;
using Core.ServiceLocator;
using Enums;
using Gameplay.Timer;
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

        private void Awake()
        {
            _gameInstance = Services.Get<GameInstance>();
        }

        private void OnEnable()
        {
            _gameInstance.Register(this);
        }

        private void OnDisable()
        {
            _gameInstance.Unregister(this);
        }

        public virtual void Init(Team team, ProjectileID projectileID)
        {
            CurrentLifeTime = _lifeTime;
            ProjectileID = projectileID;
            Team = team;
            StartDirection = transform.up;
        }

        public virtual void Apply(Vehicle tank)
        {
            Despawn();
        }

        public void OnUpdate()
        {
            CurrentLifeTime -= Time.deltaTime;
            if (CurrentLifeTime <= 0)
                Despawn();
        }

        public virtual void Despawn()
        {
            _gameInstance.Unregister(this);
            gameObject.SetActive(false);
            Deactivated?.Invoke(this, true);
        }

        private void OnDestroy()
        {
            _gameInstance.Unregister(this);
            Deactivated = null;
        }

        public Action<BasicProjectile, bool> Deactivated;
    }
}