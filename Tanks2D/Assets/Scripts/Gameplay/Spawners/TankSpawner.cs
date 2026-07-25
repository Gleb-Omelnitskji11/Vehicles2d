using System;
using Config;
using Core;
using Core.ObjectPool;
using Core.ServiceLocator;
using Enums;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay
{
    public abstract class TankSpawner
    {
        private MapLoader _mapLoader;
        protected TanksRegister VehicleRegister;
        protected VehicleConfigScript VehicleConfig;
        private IObjectPooler _pooler;

        private const int MaxSpawnAttempts = 10;
        private const float Radius = 2.0f; 
        private const float SqrRadius = Radius * Radius;

        public virtual void Init()
        {
            _mapLoader = Services.Get<MapLoader>();
            VehicleRegister = Services.Get<TanksRegister>();
            var configProvider = Services.Get<IConfigProvider>();
            _pooler = Services.Get<IObjectPooler>();
            VehicleConfig = configProvider.VehicleConfig;
        
            CreatePools();
        }

        private void CreatePools()
        {
            foreach (VehicleType vehicleType in Enum.GetValues(typeof(VehicleType)))
            {
                string key = GetKey(vehicleType);
                Func<Vehicle> creator = GetCreatorFunc(vehicleType);
                _pooler.CreatePool<Vehicle>(key, factory: creator,
                    onGet: OnGetFromPool, onRelease: OnRealiseToPool,
                    prewarmCount: 0);
            }
        }
        
        protected Vehicle SpawnVehicle(VehicleType vehicleType)
        {
            string key = GetKey(vehicleType);
            Vehicle vehicle = _pooler.Get<Vehicle>(key);
            return vehicle;
        }

        private void OnGetFromPool(Vehicle vehicle)
        {
            VehicleRegister.RegisterVehicle(vehicle);
        }
        
        private void OnRealiseToPool(Vehicle vehicle)
        {
            VehicleRegister.UnRegisterVehicle(vehicle);
        }
        
        private Func<Vehicle> GetCreatorFunc(VehicleType vehicleType)
        {
            switch (vehicleType)
            {
                case VehicleType.Tank:
                    return CreateNewVehicle;
            }

            throw new InvalidOperationException();
        }

        private Vehicle CreateNewVehicle()
        {
            var prefab = VehicleConfig.GetVehiclePrefab(VehicleType.Tank);
            var vehicle = Spawn(prefab);
            return vehicle;
        }

        private Vehicle Spawn(GameObject prefab)
        {
            Vector2 position = GetClearSpawnPosition();
            Vector2 dir = -position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            var newObj = Object.Instantiate(prefab, position, Quaternion.Euler(0, 0, angle));
            Vehicle tank = newObj.GetComponent<Vehicle>();
            
            VehicleRegister.RegisterVehicle(tank);

            return tank;
        }
        
        private Vector3 GetClearSpawnPosition()
        {
            Vector3 potentialPoint = Vector3.zero;
            int attempts = 0;

            while (attempts < MaxSpawnAttempts)
            {
                potentialPoint = _mapLoader.Map.GetRandomPoint();

                if (IsFree(potentialPoint))
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
            foreach (var tank in VehicleRegister.ActiveTanks)
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
        
        
        private string GetKey(VehicleType type) => $"Vehicle_{type}";
    }
}