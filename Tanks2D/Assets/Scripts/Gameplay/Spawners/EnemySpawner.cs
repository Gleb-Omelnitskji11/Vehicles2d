using System;
using System.Collections.Generic;
using Core.ServiceLocator;
using Enums;
using UnityEngine;

namespace Gameplay.Spawners
{
    public class EnemySpawner : IEnemySpawner
    {
        private VehicleSpawner _vehicleSpawner;
        private readonly Dictionary<EnemyType, Type> _enemyAi = new Dictionary<EnemyType, Type>()
        {
            { EnemyType.Simple, typeof(SimpleEnemy) }
        };

        private readonly Dictionary<EnemyType, VehicleType> _enemiesData = new Dictionary<EnemyType, VehicleType>()
        {
            { EnemyType.Simple, VehicleType.Tank}
        };

        private GameInstance _gameInstance;

        public void Init()
        {
            _vehicleSpawner = Services.Get<VehicleSpawner>();
            _gameInstance = Services.Get<GameInstance>();
        }
        
        public Vehicle SpawnEnemy(EnemyType enemyType)
        {
            var vehicleID = GetVehicleID(EnemyType.Simple); //hardcode
            Vehicle enemy = _vehicleSpawner.SpawnVehicle(vehicleID, false);

            if (!enemy.Configured)
                SetupEnemy(enemy, enemyType);
            
            enemy.gameObject.SetActive(true);
            return enemy;
        }

        private void SetupEnemy(Vehicle enemy, EnemyType enemyType)
        {
            Type enemyAi = GetAiComponent(enemyType);
            var ai =enemy.gameObject.AddComponent(enemyAi);
            
            string enemyStr = "Enemy";
            enemy.name = enemyStr;
            enemy.tag = enemyStr;
            enemy.gameObject.layer = LayerMask.NameToLayer(enemyStr);

            enemy.Init(new BasicUnitHealth(1), Team.Enemy);
            if(ai is IUpdatable bot)
                _gameInstance.Register(bot);
        }
        
        private VehicleType GetVehicleID(EnemyType enemyType)
        {
            if (_enemiesData.TryGetValue(enemyType, out VehicleType tankID))
            {
                return tankID;
            }
        
            Debug.LogError($"[EnemySpawner] AI type for {enemyType} not found in dictionary! Adding default ai type.");
            throw new Exception("VehicleId Not Found");
        }

        private Type GetAiComponent(EnemyType enemyType)
        {
            if (_enemyAi.TryGetValue(enemyType, out Type aiType))
            {
                return aiType;
            }
        
            Debug.LogError($"[EnemySpawner] AI type for {enemyType} not found in dictionary! Adding default ai type.");
            throw new Exception("AI type Not Found");
        }
    }
}