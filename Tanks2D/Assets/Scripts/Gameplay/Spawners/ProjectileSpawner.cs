using Config;
using Core;
using Core.ServiceLocator;
using Enums;
using UnityEngine;

namespace Gameplay.Spawners
{
    public class ProjectileSpawner : IProjectileSpawner
    {
        private ProjectileConfigScript _projectileConfig;
        private GameObject _projectileParent;

        public void Init()
        {
            var configProvider = Services.Get<IConfigProvider>();
            _projectileConfig = configProvider.ProjectileConfig;
            _projectileParent = new GameObject
            {
                name = "ProjectileParent",
            };
        }

        public BasicProjectile Spawn(ProjectileID projectileID, Vector2 position, Quaternion rotation)
        {
            var prefab = _projectileConfig.GetProjectilePrefab(projectileID);
            var projectile = Object.Instantiate(prefab, position, rotation, _projectileParent.transform);
            projectile.tag = "Projectile";
            projectile.gameObject.layer = LayerMask.NameToLayer("Projectile");
            return projectile;
        }
    }
}