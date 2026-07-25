using System;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class Vehicle : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _teamHighlight;
        [SerializeField] private Rigidbody2D _rigidbody;
        public  bool Configured { get; private set; }
        public Team Team { get; private set; }

        public BasicUnitHealth BasicUnitHealth { get; private set; }

        public void Init(BasicUnitHealth basicUnitHealth, Team team)
        {
            if (Configured) return;
            BasicUnitHealth = basicUnitHealth;
            BasicUnitHealth.OnTakeDamage += OnTakeDamage;
            BasicUnitHealth.OnDeath += OnDie;
            SetTeam(team);
            Configured = true;
        }

        private void OnDestroy()
        {
            BasicUnitHealth.OnTakeDamage -= OnTakeDamage;
            BasicUnitHealth.OnDeath -= OnDie;

            Died = null;
            TakenDamage = null;
        }

        public void Reset()
        {
            BasicUnitHealth.ResetData();
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

            if (tag.Equals("Projectile"))
            {
                if (collision.gameObject.TryGetComponent<BasicProjectile>(out var projectile))
                {
                    if (projectile.Team != Team)
                        projectile.Apply(this);
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