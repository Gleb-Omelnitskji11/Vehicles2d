using System;
using Config;
using Core;
using Core.ObjectPool;
using Core.ServiceLocator;
using Enums;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Spawners
{
    public class ProjectileSpawner : IProjectileSpawner
    {
        private ProjectileConfigScript _projectileConfig;
        private GameObject _projectileParent;
        private ProjectileRegister _projectileRegister;
        private IObjectPooler _pooler;
        private GameInstance _gameInstance;

        public void Init()
        {
            var configProvider = Services.Get<IConfigProvider>();
            _projectileRegister = Services.Get<ProjectileRegister>();
            _pooler = Services.Get<IObjectPooler>();
            _gameInstance = Services.Get<GameInstance>();
            
            _projectileConfig = configProvider.ProjectileConfig;
            _projectileParent = new GameObject
            {
                name = "ProjectileParent",
            };

            CreatePools();
        }

        public BasicProjectile SpawnProjectile(ProjectileID projectileID)
        {
            string key = GetKey(projectileID);
            BasicProjectile projectile = _pooler.Get<BasicProjectile>(key);
            return projectile;
        }
        
        private void CreatePools()
        {
            foreach (ProjectileID projectileID in Enum.GetValues(typeof(ProjectileID)))
            {
                string key = GetKey(projectileID);
                Func<BasicProjectile> creator = GetCreatorFunc(projectileID);
                _pooler.CreatePool<BasicProjectile>(key, factory: creator,
                    onGet: OnGetFromPool, onRelease: OnRealiseToPool,
                    prewarmCount: 0);
            }
        }
        
        private Func<BasicProjectile> GetCreatorFunc(ProjectileID projectileType)
        {
            switch (projectileType)
            {
                case ProjectileID.Bullet:
                    return CreateNewBullet;
            }

            throw new InvalidOperationException();
        }

        private void OnGetFromPool(BasicProjectile projectile)
        {
            _projectileRegister.RegisterProjectile(projectile);
        }
        
        private void OnRealiseToPool(BasicProjectile projectile)
        {
            _projectileRegister.UnRegisterProjectile(projectile);
        }

        private BasicProjectile CreateNewBullet()
        {
            ProjectileID projectileID = ProjectileID.Bullet;
            BasicProjectile prefab = _projectileConfig.GetProjectilePrefab(projectileID);
            var projectile = SpawnNew(prefab);
            SetPoolObject(projectile, projectileID);
            
            if(projectile is IUpdatable updatable)
                _gameInstance.Register(updatable);
            return projectile;
        }

        private void SetPoolObject(BasicProjectile projectile, ProjectileID projectileID)
        {
            PooledObject<BasicProjectile> pooledObject = new PooledObject<BasicProjectile>();
            pooledObject.SetPoolableData(_pooler, GetKey(projectileID));
            projectile.Deactivated += pooledObject.Deactivate;
        }
        
        private BasicProjectile SpawnNew(BasicProjectile prefab)
        {
            var projectile = Object.Instantiate(prefab, _projectileParent.transform);
            projectile.tag = "Projectile";
            projectile.gameObject.layer = LayerMask.NameToLayer("Projectile");
            _projectileRegister.RegisterProjectile(projectile);
            return projectile;
        }
        
        private string GetKey(ProjectileID type) => $"Projectile_{type}";
    }
}