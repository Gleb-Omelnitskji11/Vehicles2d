using System;
using Enums;
using UnityEngine;

namespace Gameplay
{
    public class Tank : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _teamHighlight;
        [SerializeField] private Rigidbody2D _rigidbody;
        private bool _configured;
        public Team Team { get; private set; }

        public BasicUnitHealth BasicUnitHealth { get; private set; }

        public void Init(BasicUnitHealth basicUnitHealth, Team team)
        {
            BasicUnitHealth = basicUnitHealth;
            BasicUnitHealth.OnTakeDamage += OnTakeDamage;
            BasicUnitHealth.OnDeath += OnDie;
            SetTeam(team);
            _configured = true;
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

        private void SetTeam(Team team)
        {
            _teamHighlight.color = team == Team.Ally ? Color.green : Color.red;
            Team = team;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_configured) return;
            
            OnCollision?.Invoke(collision);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_configured) return;
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
            Died?.Invoke(this);
            Destroy(gameObject);
        }

        private void OnTakeDamage()
        {
            TakenDamage?.Invoke(this);
        }


        public event Action<Collision2D> OnCollision;
        
        public event Action<Tank> Died;
        public event Action<Tank> TakenDamage;
    }
}