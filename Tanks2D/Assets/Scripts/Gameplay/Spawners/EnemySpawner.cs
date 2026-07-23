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

        private readonly Dictionary<EnemyType, TankID> _enemiesData = new Dictionary<EnemyType, TankID>()
        {
            { EnemyType.Simple, TankID.Default}
        };
        
        public GameObject SpawnEnemy(EnemyType enemyType)
        {
            var config = _configProvider.TankConfig;
            var tankID = GetTankID(EnemyType.Simple);
            GameObject tankPrefab = config.GetTankPrefab(tankID);
            GameObject enemy = Spawn(tankPrefab);

            Type enemyAi = GetAiComponent(enemyType);
            enemy.AddComponent(enemyAi);
            _tanksRegister.RegisterEnemy(enemy);
            enemy.name = "Enemy";

            return enemy;
        }
        
        private TankID GetTankID(EnemyType enemyType)
        {
            if (_enemiesData.TryGetValue(enemyType, out TankID tankID))
            {
                return tankID;
            }
        
            Debug.LogError($"[EnemySpawner] AI type for {enemyType} not found in dictionary! Adding default ai type.");
            throw new Exception("TankId Not Found");
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