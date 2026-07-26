using Core.ObjectPool;
using Core.ServiceLocator;
using Gameplay;
using Gameplay.Spawners;
using Gameplay.Timer;
using Gameplay.UpdatingInstance;
using UI;
using UnityEngine;

namespace Core
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private MapLoader _mapLoader;
        [SerializeField] private GamePopupsProvider _gamePopupsProvider;
        [SerializeField] private GameInstance _gameInstance;
        [SerializeField] private ProgressVisualizer _progressVisualizer;

        private void Awake()
        {
            if (!Services.TryGet<IServiceLocator>(out IServiceLocator serviceLocator))
            {
                Debug.LogError("Service Locator could not be found.");
                return;
            }

            serviceLocator.Register<GameInstance>(_gameInstance);
            serviceLocator.Register<MapLoader>(_mapLoader);
            serviceLocator.Register<TanksRegister>(new TanksRegister());
            serviceLocator.Register<ProjectileRegister>(new ProjectileRegister());
            serviceLocator.Register<IObjectPooler>(new ObjectPooler());
            serviceLocator.Register<VehicleSpawner>(new VehicleSpawner());
            serviceLocator.Register<IPlayerSpawner>(new PlayerSpawner());
            serviceLocator.Register<IEnemySpawner>(new EnemySpawner());
            serviceLocator.Register<IProjectileSpawner>(new ProjectileSpawner());
            serviceLocator.Register<GamePopupsProvider>(_gamePopupsProvider);
            serviceLocator.Register<ILevelLoader>(new LevelLoader());
            serviceLocator.Register<GameTimerService>(new GameTimerService());
            serviceLocator.Register<ProgressVisualizer>(_progressVisualizer);
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