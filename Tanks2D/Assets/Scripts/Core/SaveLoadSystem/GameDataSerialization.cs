using System;
using System.Linq;
using Core.ServiceLocator;
using Gameplay;
using Gameplay.Attacking;
using Gameplay.Enemy;
using UnityEngine;

namespace Core.SaveLoadSystem
{
    public class GameDataSerialization : IService, IInit
    {
        private TanksRegister _tanksRegister;
        private GameProgressor _gameProgressor;
        private ProjectileRegister _projectileRegister;

        public void Init()
        {
            _tanksRegister = Services.Get<TanksRegister>();
            _projectileRegister = Services.Get<ProjectileRegister>();
            _gameProgressor = Services.Get<GameProgressor>();
        }

        public SaveData GetSaveData()
        {
            //level
            SaveData saveData = new SaveData
            {
                Version = SaveData.CurrentVersion,
                LevelData = _gameProgressor.GetLevelSaveData(),
                Enemies = Array.Empty<EnemySaveData>(),
                ProjectilesData = Array.Empty<ProjectileSaveData>()
            };
            
            //player
            if(_tanksRegister.Player)
            {
                saveData.PlayerData = GetPlayerSaveData(_tanksRegister.Player);
            }

            //enemy
            saveData.Enemies = _tanksRegister.ActiveTanks.Select(GetEnemySaveData).ToArray();
            
            //projectile
            saveData.ProjectilesData = _projectileRegister.ActiveProjectile.Select(GetProjectileSaveData).ToArray();
            
            return saveData;
        }

        private PlayerSaveData GetPlayerSaveData(Vehicle vehicle)
        {
            PlayerSaveData playerSaveData = new PlayerSaveData();
            playerSaveData.VehicleSaveData = GetVehicleSaveData(vehicle);
            return playerSaveData;
        }

        private EnemySaveData GetEnemySaveData(Vehicle vehicle)
        {
            EnemySaveData vehicleSaveData = new EnemySaveData();
            vehicleSaveData.VehicleSaveData = GetVehicleSaveData(vehicle);
            if(vehicle.TryGetComponent<BaseEnemy>(out BaseEnemy baseEnemy))
                vehicleSaveData.EnemyType = baseEnemy.EnemyType;
            return vehicleSaveData;
        }

        private VehicleSaveData GetVehicleSaveData(Vehicle vehicle)
        {
            VehicleSaveData vehicleSaveData = new VehicleSaveData();
            Vector3 position = vehicle.transform.position;
            vehicleSaveData.Position = new SerializableVector2(position.x, position.y);
            vehicleSaveData.Rotation = new SerializableQuaternion(vehicle.transform.rotation);
            vehicleSaveData.VehicleType = vehicle.VehicleType;
            bool hasGun = vehicle.transform.TryGetComponent<BaseGun>(out BaseGun simpleGun);
            if (hasGun)
            {
                vehicleSaveData.GunSaveData = new GunSaveData()
                {
                    Data = simpleGun.GetGunSaveData()
                };
            }
            
            return vehicleSaveData;
        }

        private ProjectileSaveData GetProjectileSaveData(BasicProjectile projectile)
        {
            ProjectileSaveData projectileData = new ProjectileSaveData();
            Vector3 position = projectile.transform.position;
            projectileData.Position = new SerializableVector2(position.x, position.y);
            projectileData.Rotation = new SerializableQuaternion(projectile.transform.rotation);
            projectileData.Team = projectile.Team;
            projectileData.ProjectileID = projectile.ProjectileID;
            projectileData.Lifetime = projectile.CurrentLifeTime;
            return projectileData;
        }
    }
}
