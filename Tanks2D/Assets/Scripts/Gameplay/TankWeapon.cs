using Core;
using Core.ServiceLocator;
using Enums;
using Gameplay.Spawners;
using UnityEngine;

namespace Gameplay
{
    public class TankWeapon : MonoBehaviour
    {
        private float _fireCooldown = 0.2f;

        private float _cooldown;
        private IProjectileSpawner _projectileSpawner;
        private ProjectileID _projectileID;
        private Team _team;

        public void Init(float fireCooldown, ProjectileID projectileID, Team team)
        {
            _team = team;
            _projectileID = projectileID;
            _projectileSpawner = Services.Get<IProjectileSpawner>();
            _fireCooldown = fireCooldown;
        }

        private void Update()
        {
            if (_cooldown > 0)
                _cooldown -= Time.deltaTime;
        }

        public void Fire()
        {
            if (_cooldown > 0)
                return;

            _cooldown = _fireCooldown;
            BasicProjectile projectile = _projectileSpawner.Spawn(_projectileID, transform.position, transform.rotation);
            projectile.Init(_team);
        }
    }
}