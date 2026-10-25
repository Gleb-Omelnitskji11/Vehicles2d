using UnityEngine;

namespace Gameplay.Attacking
{
    public class Bullet : BasicProjectile
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] protected int _damage = 1;

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
            }
        }

        public override void ApplyHit(Vehicle tank)
        {
            tank.BasicUnitHealth.TakeDamage(_damage);
            base.ApplyHit(tank);
        }
    }
}