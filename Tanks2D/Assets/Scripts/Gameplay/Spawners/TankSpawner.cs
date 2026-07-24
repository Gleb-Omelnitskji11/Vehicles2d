using Config;
using Core;
using Core.ServiceLocator;
using UnityEngine;

namespace Gameplay
{
    public abstract class TankSpawner
    {
        private MapLoader _mapLoader;
        protected TanksRegister TanksRegister;
        protected TankConfigScript TankConfigScript;
        
        private const int MaxSpawnAttempts = 10;
        private const float Radius = 2.0f; 
        private const float SqrRadius = Radius * Radius;

        public virtual void Init()
        {
            _mapLoader = Services.Get<MapLoader>();
            TanksRegister = Services.Get<TanksRegister>();
            var configProvider = Services.Get<IConfigProvider>();
            TankConfigScript = configProvider.TankConfig;
        }

        protected Tank Spawn(GameObject prefab)
        {
            Vector2 position = _mapLoader.Map.GetRandomPoint();
            Vector2 dir = -position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            var newObj = Object.Instantiate(prefab, position, Quaternion.Euler(0, 0, angle));
            Tank tank = newObj.GetComponent<Tank>();

            return tank;
        }
        
        private Vector3 GetClearSpawnPosition()
        {
            Vector3 potentialPoint = Vector3.zero;
            int attempts = 0;

            while (attempts < MaxSpawnAttempts)
            {
                potentialPoint = _mapLoader.Map.GetRandomPoint();

                if (!IsFree(potentialPoint))
                {
                    return potentialPoint;
                }

                attempts++;
            }
            
            Debug.LogWarning("[EnemySpawner] Could not find a completely clean spawn location. Spawned at random.");
            return potentialPoint;
        }

        private bool IsFree(Vector2 potentialPoint)
        {
            Vector3 playerPos = TanksRegister.Player.transform.position;
            
            if(IsNear(playerPos, potentialPoint))
                return false;

            foreach (var tank in TanksRegister.ActiveEnemies)
            {
                if (IsNear(tank.transform.position, potentialPoint))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsNear(Vector2 pos1, Vector2 pos2)
        {
            return (pos1 - pos2).sqrMagnitude < SqrRadius;
        }
    }
}