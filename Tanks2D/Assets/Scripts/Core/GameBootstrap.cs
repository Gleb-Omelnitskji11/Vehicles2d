using Core.ObjectPool;
using Core.SaveLoadSystem;
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
        [SerializeField] private EffectSpawner _effectSpawner;

        private void Awake()
        {
            if (!Services.TryGet<IServiceLocator>(out IServiceLocator serviceLocator))
            {
                Debug.LogError("Service Locator could not be found.");
                return;
            }

            serviceLocator.Register<GameInstance>(_gameInstance);
            serviceLocator.Register<IGamePlayerInput>(new GamePlayerInput());
            serviceLocator.Register<MapLoader>(_mapLoader);
            serviceLocator.Register<TanksRegister>(new TanksRegister());
            serviceLocator.Register<ProjectileRegister>(new ProjectileRegister());
            serviceLocator.Register<IObjectPooler>(new ObjectPooler());
            serviceLocator.Register<IPlayerLoadoutService>(new PlayerLoadoutService());
            serviceLocator.Register<EffectSpawner>(_effectSpawner);
            serviceLocator.Register<VehicleSpawner>(new VehicleSpawner());
            serviceLocator.Register<IPlayerSpawner>(new PlayerSpawner());
            serviceLocator.Register<IEnemySpawner>(new EnemySpawner());
            serviceLocator.Register<IProjectileSpawner>(new ProjectileSpawner());
            serviceLocator.Register<IGamePopupsProvider>(_gamePopupsProvider);
            serviceLocator.Register<ILevelLoader>(new LevelLoader());
            serviceLocator.Register<GameTimerService>(new GameTimerService());
            serviceLocator.Register<ProgressVisualizer>(_progressVisualizer);
            serviceLocator.Register<GameProgressor>(new GameProgressor());
            serviceLocator.Register<GameDataSerialization>(new GameDataSerialization());
            serviceLocator.Register<KeyHolder>(new KeyHolder());

            StartGame();
        }

        private void StartGame()
        {
            GameProgressor gameProgressor = Services.Get<GameProgressor>();
            var saveSystem = Services.Get<ISaveSystem>();
            saveSystem.OnEntryGameScene();
            if (saveSystem.HasSaved())
            {
                var data = saveSystem.GetSaveData();
                gameProgressor.LoadGame(data);
            }
            else
            {
                gameProgressor.StartGame();
            }
        }
    }
}
