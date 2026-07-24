using Core;
using Core.ServiceLocator;
using Enums;
using Gameplay.Spawners;
using Object = UnityEngine.Object;

namespace Gameplay
{
    public class GameProgressor : IService, IInit
    {
        private IPlayerSpawner _playerSpawner;
        private IEnemySpawner _enemySpawner;
        private TanksRegister _tankRegistry;

        public GameStatus GameStatus { get; private set; }
        private int _enemiesCount;
        private LevelData _levelData;
        private int _enemiesLess;
        private GamePopupsProvider _gamePopupsProvider;
        private ILevelLoader _levelLoader;
        private ProjectileRegister _projectileRegister;

        public void Init()
        {
            _playerSpawner = Services.Get<IPlayerSpawner>();
            _enemySpawner = Services.Get<IEnemySpawner>();
            _tankRegistry = Services.Get<TanksRegister>();
            _gamePopupsProvider = Services.Get<GamePopupsProvider>();
            _levelLoader = Services.Get<ILevelLoader>();
            _projectileRegister = Services.Get<ProjectileRegister>();
        }

        public void RepeatGame()
        {
            Clear();
            StartGame();
        }

        public void StartGame()
        {
            var player = _playerSpawner.SpawnPlayer();
            player.Died += OnPlayerDie;

            _levelData = _levelLoader.LoadLevel(0);
            _enemiesCount = 0;
            _enemiesLess = _levelData.EnemiesCount;
            SpawnEnemies();

            GameStatus = GameStatus.Running;
        }

        private void SpawnEnemies()
        {
            while (_enemiesCount < _levelData.EnemiesMax && _enemiesLess > 0)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            var enemy = _enemySpawner.SpawnEnemy(EnemyType.Simple);
            enemy.Died += OnEnemyDie;
            _enemiesCount++;
            _enemiesLess--;
        }

        private void OnEnemyDie(Tank tank)
        {
            _enemiesCount--;
            Object.Destroy(tank.gameObject);
            _tankRegistry.UnRegisterTank(tank);
            SpawnEnemies();

            if (_enemiesCount == 0 && _enemiesLess == 0)
            {
                GameStatus = GameStatus.Win;
                _gamePopupsProvider.ResultPopup.Open();
            }
        }

        private void Clear()
        {
            while (_tankRegistry.ActiveTanks.Count > 0)
            {
                var tank = _tankRegistry.ActiveTanks[0];
                tank.gameObject.SetActive(false);
                _tankRegistry.UnRegisterTank(tank);
                Object.Destroy(tank.gameObject);
            }

            while (_projectileRegister.ActiveProjectile.Count > 0)
            {
                var projectile = _projectileRegister.ActiveProjectile[0];
                projectile.gameObject.SetActive(false);
                _projectileRegister.UnRegisterEnemy(projectile);
                Object.Destroy(projectile.gameObject);
            }
            GameStatus = GameStatus.NotStarted;
        }

        private void OnPlayerDie(Tank tank)
        {
            GameStatus = GameStatus.Lose;
            _gamePopupsProvider.ResultPopup.Open();
        }
    }
}