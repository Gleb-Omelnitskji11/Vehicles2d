using Core.ServiceLocator;
using Gameplay;
using Gameplay.Spawners;
using UnityEngine;

namespace Core
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private MapLoader _mapLoader;
        [SerializeField] private GamePopupsProvider _gamePopupsProvider;

        private void Awake()
        {
            if (!Services.TryGet<IServiceLocator>(out IServiceLocator serviceLocator))
            {
                Debug.LogError("Service Locator could not be found.");
                return;
            }

            serviceLocator.Register<MapLoader>(_mapLoader);
            serviceLocator.Register<TanksRegister>(new TanksRegister());
            serviceLocator.Register<ProjectileRegister>(new ProjectileRegister());
            serviceLocator.Register<IPlayerSpawner>(new PlayerSpawner());
            serviceLocator.Register<IEnemySpawner>(new EnemySpawner());
            serviceLocator.Register<IProjectileSpawner>(new ProjectileSpawner());
            serviceLocator.Register<GamePopupsProvider>(_gamePopupsProvider);
            serviceLocator.Register<ILevelLoader>(new LevelLoader());
            serviceLocator.Register<GameProgressor>(new GameProgressor());

            StartGame();
        }

        private void StartGame()
        {
            var gameProgress = Services.Get<GameProgressor>();
            gameProgress.StartGame();
        }
    }
}