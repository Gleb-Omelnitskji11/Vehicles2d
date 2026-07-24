using Core.ServiceLocator;
using Enums;
using UnityEngine;

namespace Gameplay.Spawners
{
    public interface IEnemySpawner : IService, IInit
    {
        Tank SpawnEnemy(EnemyType enemyType);
    }
}