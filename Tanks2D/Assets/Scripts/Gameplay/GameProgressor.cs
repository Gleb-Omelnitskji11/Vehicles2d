using Core.ServiceLocator;
using Enums;
using Gameplay.Spawners;

namespace Gameplay
{
    public class GameProgressor : IService, IInit
    {
        private MapLoader _mapLoader;
        private IPlayerSpawner _playerSpawner;
        private IEnemySpawner _enemySpawner;

        public void Init()
        {
            _mapLoader = Services.Get<MapLoader>();
            _playerSpawner = Services.Get<IPlayerSpawner>();
            _enemySpawner = Services.Get<IEnemySpawner>();
        }

        public void StartGame()
        {
            _playerSpawner.SpawnPlayer();
            _enemySpawner.SpawnEnemy(EnemyType.Simple);
            _enemySpawner.SpawnEnemy(EnemyType.Simple);
        }
    }
}