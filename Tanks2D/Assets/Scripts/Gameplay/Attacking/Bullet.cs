using System;
using UnityEngine;

namespace Gameplay.Attacking
{
    public class Bullet : BasicProjectile
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] protected int _damage = 1;

        public event Action<Vector2, Enums.ProjectileID> OnHit;

        public override void OnUpdate()
        {
            base.OnUpdate();
            transform.position += (Vector3)(StartDirection * _speed * Time.deltaTime);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Wall"))
            {
                Despawn();
                OnHit?.Invoke(collision.ClosestPoint(transform.position), ProjectileID);
            }
        }

        public override void ApplyHit(Vehicle tank)
        {
            Collider2D tankCollider = tank.GetComponent<Collider2D>();
            Vector2 hitPoint = tankCollider != null
                ? tankCollider.ClosestPoint(transform.position)
                : transform.position;
            OnHit?.Invoke(hitPoint, ProjectileID);
            tank.BasicUnitHealth.TakeDamage(_damage);
            base.ApplyHit(tank);
        }
    }
}
