using UnityEngine;

namespace Gameplay
{
    public class Bullet : BasicProjectile
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] protected int _damage = 1;

        private void Update()
        {
            transform.position += (Vector3)(StartDirection * _speed * Time.deltaTime);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Wall"))
            {
                Despawn();
            }
        }

        public override void Apply(Tank tank)
        {
            tank.BasicUnitHealth.TakeDamage(_damage);
            base.Apply(tank);
        }
    }
}