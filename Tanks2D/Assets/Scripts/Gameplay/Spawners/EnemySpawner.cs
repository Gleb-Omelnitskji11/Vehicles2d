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

        private readonly Dictionary<EnemyType, TankModel> _enemiesData = new Dictionary<EnemyType, TankModel>()
        {
            { EnemyType.Simple, TankModel.Default}
        };
        
        public Tank SpawnEnemy(EnemyType enemyType)
        {
            var config = TankConfigScript;
            var tankID = GetTankID(EnemyType.Simple);
            GameObject tankPrefab = config.GetTankPrefab(tankID);
            Tank enemy = Spawn(tankPrefab);

            Type enemyAi = GetAiComponent(enemyType);
            enemy.gameObject.AddComponent(enemyAi);
            TanksRegister.RegisterEnemy(enemy);
            
            enemy.name = "Enemy";
            enemy.tag = "Enemy";
            enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");

            enemy.Init(new BasicUnitHealth(1), Team.Enemy);

            return enemy;
        }
        
        private TankModel GetTankID(EnemyType enemyType)
        {
            if (_enemiesData.TryGetValue(enemyType, out TankModel tankID))
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