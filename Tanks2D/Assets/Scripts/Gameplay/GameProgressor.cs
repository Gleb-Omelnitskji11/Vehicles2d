using System.Collections.Generic;
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
        private UniTask _awaitOperation;
        private GameTimerService _gameTimerService;
        private readonly List<Timer> _timers = new List<Timer>();
        private GameInstance _gameInstance;

        public void Init()
        {
            _playerSpawner = Services.Get<IPlayerSpawner>();
            _enemySpawner = Services.Get<IEnemySpawner>();
            _tankRegistry = Services.Get<TanksRegister>();
            _gamePopupsProvider = Services.Get<GamePopupsProvider>();
            _levelLoader = Services.Get<ILevelLoader>();
            _projectileRegister = Services.Get<ProjectileRegister>();
            _gameTimerService = Services.Get<GameTimerService>();
            _gameInstance = Services.Get<GameInstance>();
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

        public void Pause()
        {
            GameStatus = GameStatus.Pause;
        }

        public void Resume()
        {
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
        
        private void SpawnNewEnemies()
        {
            if(_enemiesCount < _levelData.EnemiesMax && _enemiesLess > 0)
            {
                _enemiesLess--;
                SpawnEnemy();
            }

            var timer = _timers[0];
            _timers.RemoveAt(0);
            _gameInstance.Unregister(timer);
        }

        private void SpawnEnemy()
        {
            var enemy = _enemySpawner.SpawnEnemy(EnemyType.Simple);
            enemy.Died += OnEnemyDie;
            _enemiesCount++;
        }

        private void OnEnemyDie(Vehicle tank, bool byManual)
        {
            tank.Died -= OnEnemyDie;
            _enemiesCount--;
            if (byManual) return;

            if (_enemiesCount == 0 && _enemiesLess == 0)
            {
                GameStatus = GameStatus.Win;
                _gamePopupsProvider.ResultPopup.Open();
            }
            else
            {
                var timer = _gameTimerService.AwaitGameOperation(1f, SpawnNewEnemies);
                _timers.Add(timer);
                _gameInstance.Register(timer);
            }
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

            foreach (var timer in _timers)
            {
                timer.Cancel();
            }

            _timers.Clear();
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