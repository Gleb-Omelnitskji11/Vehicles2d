using System;
using System.Collections.Generic;
using Core.ServiceLocator;
using Enums;
using UnityEngine;

namespace Gameplay.Spawners
{
    public class EnemySpawner : TankSpawner, IEnemySpawner
    {
        private readonly Dictionary<EnemyType, Type> _enemyAi = new Dictionary<EnemyType, Type>()
        {
            { EnemyType.Simple, typeof(SimpleEnemy) }
        };

        private readonly Dictionary<EnemyType, VehicleType> _enemiesData = new Dictionary<EnemyType, VehicleType>()
        {
            { EnemyType.Simple, VehicleType.Tank}
        };
        
        public Vehicle SpawnEnemy(EnemyType enemyType)
        {
            var vehicleID = GetVehicleID(EnemyType.Simple);
            Vehicle enemy = SpawnVehicle(vehicleID);

            Type enemyAi = GetAiComponent(enemyType);
            enemy.gameObject.AddComponent(enemyAi);
            
            enemy.name = "Enemy";
            enemy.tag = "Enemy";
            enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");

            enemy.Init(new BasicUnitHealth(1), Team.Enemy);

            return enemy;
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