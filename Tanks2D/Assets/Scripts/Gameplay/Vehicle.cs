using System;
using Core;
using Enums;
using Gameplay.Attacking;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class Vehicle : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _teamHighlight;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private TrailRenderer _trail;
        public VehicleType VehicleType { get; private set; }
        public  bool Configured { get; private set; }
        public Team Team { get; private set; }

        public BasicUnitHealth BasicUnitHealth { get; private set; }

        public void Init(BasicUnitHealth basicUnitHealth, Team team, VehicleType vehicleType)
        {
            if (Configured) return;
            BasicUnitHealth = basicUnitHealth;
            BasicUnitHealth.OnTakeDamage += OnTakeDamage;
            BasicUnitHealth.OnDeath += OnDie;
            SetTeam(team);
            VehicleType = vehicleType;
            Configured = true;
        }

        private void OnDestroy()
        {
            if (BasicUnitHealth != null)
            {
                BasicUnitHealth.OnTakeDamage -= OnTakeDamage;
                BasicUnitHealth.OnDeath -= OnDie;
            }

            Died = null;
            TakenDamage = null;
        }

        public void Reset()
        {
            BasicUnitHealth.ResetData();
            _trail.Clear();
        }

        public void TurnOff()
        {
            gameObject.SetActive(false);
            Died?.Invoke(this, true);
        }

        private void SetTeam(Team team)
        {
            _teamHighlight.color = team == Team.Ally ? Color.green : Color.red;
            Team = team;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!Configured) return;
            
            OnCollision?.Invoke(collision);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!Configured) return;
            string tag = collision.gameObject.tag;

            if (tag.Equals(Constants.Projectile))
            {
                if (collision.gameObject.TryGetComponent<BasicProjectile>(out var projectile))
                {
                    if (projectile.Team != Team)
                        projectile.ApplyHit(this);
                }
            }
        }

        private void OnDie()
        {
            gameObject.SetActive(false);
            Died?.Invoke(this, false);
        }

        private void OnTakeDamage()
        {
            TakenDamage?.Invoke(this);
        }


        public event Action<Collision2D> OnCollision;
        
        public event Action<Vehicle, bool> Died;
        public event Action<Vehicle> TakenDamage;
    }
}
