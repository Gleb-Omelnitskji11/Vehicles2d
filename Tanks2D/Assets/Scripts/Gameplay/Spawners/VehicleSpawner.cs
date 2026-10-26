using System;
using Config;
using Core;
using Core.ObjectPool;
using Core.ServiceLocator;
using Enums;
using Gameplay.UpdatingInstance;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay
{
    public class VehicleSpawner : IService, IInit
    {
        private MapLoader _mapLoader;
        private TanksRegister _vehicleRegister;
        private VehicleConfigScript _vehicleConfig;
        private IObjectPooler _pooler;

        private const int MaxSpawnAttempts = 10;
        private const float Radius = 4.0f;
        private const float SqrRadius = Radius * Radius;

        public void Init()
        {
            _mapLoader = Services.Get<MapLoader>();
            _vehicleRegister = Services.Get<TanksRegister>();
            var configProvider = Services.Get<IConfigProvider>();
            _pooler = Services.Get<IObjectPooler>();

            _vehicleConfig = configProvider.GetConfig<VehicleConfigScript>(ConfigType.VehicleConfig);
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

        public Vehicle SpawnVehicle(VehicleType vehicleType, bool isPlayer)
        {
            if (isPlayer)
            {
                var prefab = _vehicleConfig.GetVehiclePrefab(vehicleType);
                Vehicle player = Spawn(prefab);
                SetTransform(player);
                return player;
            }

            string key = GetKey(vehicleType);
            Vehicle vehicle = _pooler.Get<Vehicle>(key);

            _vehicleRegister.RegisterVehicle(vehicle);
            return vehicle;
        }

        private void OnGetFromPool(Vehicle vehicle)
        {
            SetTransform(vehicle);
        }

        private void SetTransform(Vehicle vehicle)
        {
            Vector2 position = GetClearSpawnPosition();
            Vector2 dir = -position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            vehicle.transform.position = position;
            vehicle.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void OnRealiseToPool(Vehicle vehicle)
        {
            _vehicleRegister.UnRegisterVehicle(vehicle);
            vehicle.Reset();
        }

        private Func<Vehicle> GetCreatorFunc(VehicleType vehicleType)
        {
            switch (vehicleType)
            {
                case VehicleType.Tank:
                    return CreateTankForPool;
            }

            throw new InvalidOperationException();
        }

        private Vehicle CreateTankForPool()
        {
            var prefab = _vehicleConfig.GetVehiclePrefab(VehicleType.Tank);
            return CreateObjectForPool(prefab);
        }

        private Vehicle CreateObjectForPool(GameObject prefab)
        {
            var vehicle = Spawn(prefab);
            SetPoolObject(vehicle, VehicleType.Tank);
            return vehicle;
        }

        private void SetPoolObject(Vehicle vehicle, VehicleType vehicleType)
        {
            PooledObject<Vehicle> pooledObject = new PooledObject<Vehicle>();
            pooledObject.SetPoolableData(_pooler, GetKey(vehicleType));
            vehicle.Died += pooledObject.Deactivate;
        }

        private Vehicle Spawn(GameObject prefab)
        {
            var newObj = Object.Instantiate(prefab);
            Vehicle tank = newObj.GetComponent<Vehicle>();

            return tank;
        }

        private Vector2 GetClearSpawnPosition()
        {
            Vector2 potentialPoint = Vector3.zero;
            int attempts = 0;

            while (attempts < MaxSpawnAttempts)
            {
                potentialPoint = GetRandomPosition();

                if (IsFree(potentialPoint))
                {
                    return potentialPoint;
                }

                attempts++;
            }

            Debug.LogWarning("[EnemySpawner] Could not find a completely clean spawn location. Spawned at random.");
            return potentialPoint;
        }

        private Vector2 GetRandomPosition()
        {
            return _mapLoader.Map.GetRandomPointOnBorder();
        }

        private bool IsFree(Vector2 potentialPoint)
        {
            if (_vehicleRegister.Player && IsNear(_vehicleRegister.Player.transform.position, potentialPoint))
                return false;
            
            foreach (var tank in _vehicleRegister.ActiveTanks)
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
