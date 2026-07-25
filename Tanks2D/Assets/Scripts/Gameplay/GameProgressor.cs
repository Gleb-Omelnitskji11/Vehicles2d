using Core;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Enums;
using Gameplay.Spawners;
using UnityEngine;

namespace Gameplay
{
    public class GameProgressor : IService, IInit
    {
        private IPlayerSpawner _playerSpawner;
        private IEnemySpawner _enemySpawner;
        private TanksRegister _tankRegistry;
        private GamePopupsProvider _gamePopupsProvider;
        private ILevelLoader _levelLoader;
        private ProjectileRegister _projectileRegister;

        public GameStatus GameStatus { get; private set; }
        private int _enemiesCount;
        private int _enemiesLess;

        private LevelData _levelData;

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
                _enemiesLess--;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            var enemy = _enemySpawner.SpawnEnemy(EnemyType.Simple);
            enemy.Died += OnEnemyDie;
            _enemiesCount++;
        }

        private void OnEnemyDie(Vehicle tank, bool byManual)
        {
            if (byManual) return;
            tank.Died -= OnEnemyDie;
            _enemiesCount--;

            if (_enemiesCount == 0 && _enemiesLess == 0)
            {
                GameStatus = GameStatus.Win;
                _gamePopupsProvider.ResultPopup.Open();
            }
            else
            {
                TrySpawnEnemies();
            }
        }

        private async UniTask TrySpawnEnemies()
        {
            await UniTask.Delay(1000);
            SpawnEnemies();
        }

        private void Clear()
        {
            while (_tankRegistry.ActiveTanks.Count > 0)
            {
                var tank = _tankRegistry.ActiveTanks[0];
                tank.TurnOff();
            }

            while (_projectileRegister.ActiveProjectile.Count > 0)
            {
                var projectile = _projectileRegister.ActiveProjectile[0];
                projectile.Despawn();
            }
            GameStatus = GameStatus.NotStarted;
        }

        private void OnPlayerDie(Vehicle tank, bool byManual)
        {
            if (byManual) return;
            GameStatus = GameStatus.Lose;
            _gamePopupsProvider.ResultPopup.Open();
        }
    }
}