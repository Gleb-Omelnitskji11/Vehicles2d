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

        public void Init()
        {
            _tanksRegister = Services.Get<TanksRegister>();
            _gameProgressor = Services.Get<GameProgressor>();
        }

        public SaveData GetSaveDate()
        {
            SaveData saveData = new SaveData
            {
                LevelData = _gameProgressor.GetLevelSaveData()
            };
            if(_tanksRegister.Player)
            {
                saveData.PlayerData = GetVehicleSaveData(_tanksRegister.Player);
            }

            saveData.Enemies = new EnemySaveData[_tanksRegister.ActiveTanks.Count];
            int i = 0;
            foreach (Vehicle vehicle in _tanksRegister.ActiveTanks)
            {
                saveData.Enemies[i] = GetEnemySaveData(vehicle);
                i++;
            }
            return saveData;
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
            bool hasGun = vehicle.transform.TryGetComponent<SimpleGun>(out SimpleGun simpleGun);
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
    }
}