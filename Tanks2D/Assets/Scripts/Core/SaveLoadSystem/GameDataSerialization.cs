using Core.ServiceLocator;
using Gameplay;
using Gameplay.Attacking;
using Gameplay.Enemy;

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

        public SaveData GetSaveDate()
        {
            //level
            SaveData saveData = new SaveData
            {
                LevelData = _gameProgressor.GetLevelSaveData()
            };
            
            //player
            if(_tanksRegister.Player)
            {
                saveData.PlayerData = GetPlayerSaveData(_tanksRegister.Player);
            }

            //enemy
            saveData.Enemies = new EnemySaveData[_tanksRegister.ActiveTanks.Count];
            int i = 0;
            foreach (Vehicle vehicle in _tanksRegister.ActiveTanks)
            {
                saveData.Enemies[i] = GetEnemySaveData(vehicle);
                i++;
            }
            
            //projectile
            saveData.ProjectilesData = new ProjectileSaveData[_projectileRegister.ActiveProjectile.Count];
            i = 0;
            foreach (BasicProjectile projectile in _projectileRegister.ActiveProjectile)
            {
                saveData.ProjectilesData[i] = GetProjectileSaveData(projectile);
                i++;
            }
            
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
            if(vehicle.TryGetComponent<SimpleEnemy>(out SimpleEnemy simpleEnemy))
                vehicleSaveData.EnemyType = simpleEnemy.EnemyType;
            return vehicleSaveData;
        }

        private VehicleSaveData GetVehicleSaveData(Vehicle vehicle)
        {
            VehicleSaveData vehicleSaveData = new VehicleSaveData();
            vehicleSaveData.Position = (vehicle.transform.position.x, vehicle.transform.position.y);
            vehicleSaveData.Rotation = new SerializableQuaternion(vehicle.transform.rotation);
            vehicleSaveData.VehicleType = vehicle.VehicleType;
            bool hasGun = vehicle.transform.TryGetComponent<BaseGun>(out BaseGun simpleGun);
            if (hasGun)
            {
                vehicleSaveData.GunSaveData = new GunSaveData()
                {
                    GunType = simpleGun.GetType(),
                    Data = simpleGun.GetGunSaveData()
                };
            }
            
            return vehicleSaveData;
        }

        private ProjectileSaveData GetProjectileSaveData(BasicProjectile projectile)
        {
            ProjectileSaveData projectileData = new ProjectileSaveData();
            projectileData.Position = (projectile.transform.position.x, projectile.transform.position.y);
            projectileData.Rotation = new SerializableQuaternion(projectile.transform.rotation);
            projectileData.ProjectileID = projectile.ProjectileID;
            projectileData.Lifetime = projectile.CurrentLifeTime;
            return projectileData;
        }
    }
}