using Core.SaveLoadSystem;
using Core.ServiceLocator;
using Enums;
using Gameplay.Spawners;
using UnityEngine;

namespace Gameplay.Attacking
{
    public class BaseGun : MonoBehaviour
    {
        private float _fireCooldown = 0.2f;

        private float _cooldown;
        private IProjectileSpawner _projectileSpawner;
        private ProjectileID _projectileID;
        private Team _team;

        public virtual void Init(float fireCooldown, ProjectileID projectileID, Team team)
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

        public virtual void Fire()
        {
            if (_cooldown > 0)
                return;

            _cooldown = _fireCooldown;
            BasicProjectile projectile = _projectileSpawner.SpawnProjectile(_projectileID);
            
            projectile.transform.position = transform.position;
            projectile.transform.rotation = transform.rotation;
            projectile.gameObject.SetActive(true);
            projectile.Init(_team, _projectileID);
        }

        public virtual SimpleGunSaveData GetGunSaveData()
        {
            SimpleGunSaveData gunSaveData = new SimpleGunSaveData()
            {
                Cooldawn = _cooldown
            };
            return gunSaveData;
        }

        public void LoadData(SimpleGunSaveData data)
        {
            _cooldown = data.Cooldawn;
        }
    }
}