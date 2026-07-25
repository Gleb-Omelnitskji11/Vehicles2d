using Core.ServiceLocator;
using Enums;
using UnityEngine;

namespace Gameplay.Spawners
{
    public interface IEnemySpawner : IService, IInit
    {
        Vehicle SpawnEnemy(EnemyType enemyType);
    }
}