using System.Collections.Generic;
using System.Linq;
using Core;
using Core.SaveLoadSystem;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Enums;
using Gameplay.Attacking;
using Gameplay.Spawners;
using Gameplay.Timer;
using Gameplay.UpdatingInstance;
using UI;
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
        private List<EnemyType> _enemiesLess;

        private LevelData _levelData;
        private UniTask _awaitOperation;
        private GameTimerService _gameTimerService;
        private readonly List<Timer.Timer> _timers = new List<Timer.Timer>();
        private GameInstance _gameInstance;
        private ProgressVisualizer _progressVisualizer;

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
            _progressVisualizer = Services.Get<ProgressVisualizer>();
        }

        public void RepeatGame()
        {
            Clear();
            StartGame();
        }

        public void StartGame()
        {
            SpawnPlayer();
            _levelData = _levelLoader.LoadLevel(1); //hardcode
            _enemiesCount = 0;
            _enemiesLess = _levelData.Enemies.ToList();
            _progressVisualizer.UpdateInfo(_enemiesCount + _enemiesLess.Count);
            SpawnEnemies();
            

            GameStatus = GameStatus.Running;
        }

        public void LoadGame(SaveData saveData)
        {
            //level
            _levelData = _levelLoader.LoadLevel(saveData.LevelData.LevelIndex);
            _enemiesLess = saveData.LevelData.EnemiesLess.ToList();

            //enemies
            _enemiesCount = 0;
            foreach (var enemyData in saveData.Enemies)
            {
                var vehicle = GetNewVehicle(enemyData.EnemyType);
                
                vehicle.transform.position = new Vector2(enemyData.VehicleSaveData.Position.Item1, enemyData.VehicleSaveData.Position.Item2);
                vehicle.transform.rotation = enemyData.VehicleSaveData.Rotation.ToQuaternion();
                
                if(vehicle.TryGetComponent<SimpleGun>(out SimpleGun simpleGun))
                {
                    simpleGun.LoadData(saveData.PlayerData.GunSaveData.Data);
                }
            }
            foreach (var timer in saveData.LevelData.NextEnemySpawners)
            {
                CreateSpawnEnemyTimer(timer);
            }

            //player
            var player = SpawnPlayer();
            player.transform.position = new Vector2(saveData.PlayerData.Position.Item1, saveData.PlayerData.Position.Item2);
            player.transform.rotation = saveData.PlayerData.Rotation.ToQuaternion();
            var gun = player.GetComponent<SimpleGun>();
            gun.LoadData(saveData.PlayerData.GunSaveData.Data);

            
            _progressVisualizer.UpdateInfo(_enemiesCount + _enemiesLess.Count);
            GameStatus = GameStatus.Running;
        }

        public void Pause()
        {
            GameStatus = GameStatus.Pause;
        }

        public void Resume()
        {
            if (GameStatus != GameStatus.Pause)
                return;
            GameStatus = GameStatus.Running;
        }

        public LevelSaveData GetLevelSaveData()
        {
            LevelSaveData levelSaveData = new LevelSaveData();
            levelSaveData.EnemiesLess = _enemiesLess.ToArray();
            levelSaveData.LevelIndex = _levelData.Index;
            levelSaveData.NextEnemySpawners = new float[_timers.Count];
            int j = 0;
            for (int i = 0; i < _timers.Count; i++)
            {
                if(_timers[i].IsRunning)
                {
                    levelSaveData.NextEnemySpawners[j] = _timers[i].Time;
                    j++;
                }
            }
            return levelSaveData;
        }

        private Vehicle SpawnPlayer()
        {
            Vehicle player = _playerSpawner.SpawnPlayer();
            player.Died += OnPlayerDie;
            return player;
        }

        private void SpawnEnemies()
        {
            while (_enemiesCount < _levelData.EnemiesMax && _enemiesLess.Count > 0)
            {
                SpawnEnemy();
            }
        }

        private void SpawnNewEnemies()
        {
            if (_enemiesCount < _levelData.EnemiesMax && _enemiesLess.Count > 0)
            {
                SpawnEnemy();
            }

            var timer = _timers[0];
            _timers.RemoveAt(0);
            _gameInstance.Unregister(timer);
        }

        private void SpawnEnemy()
        {
            EnemyType enemyType = _enemiesLess.Last();
            _enemiesLess.RemoveAt(_enemiesLess.Count - 1);
            var vehicle = GetNewVehicle(enemyType);
        }

        private Vehicle GetNewVehicle(EnemyType enemyType)
        {
            var enemy = _enemySpawner.SpawnEnemy(enemyType);
            enemy.Died += OnEnemyDie;
            _enemiesCount++;
            return enemy;
        }

        private void OnEnemyDie(Vehicle tank, bool byManual)
        {
            tank.Died -= OnEnemyDie;
            _enemiesCount--;
            _progressVisualizer.UpdateInfo(_enemiesCount + _enemiesLess.Count );
            if (byManual) return;

            if (_enemiesCount == 0 && _enemiesLess.Count  == 0)
            {
                GameStatus = GameStatus.Win;
                _gamePopupsProvider.TryOpen(PopupID.ResultPopup, true);
            }
            else
            {
                CreateSpawnEnemyTimer(1f);
            }
        }

        private void CreateSpawnEnemyTimer(float time)
        {
            var timer = _gameTimerService.AwaitGameOperation(time, SpawnNewEnemies);
            _timers.Add(timer);
            _gameInstance.Register(timer);
        }

        public void Clear()
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
            _gamePopupsProvider.TryOpen(PopupID.ResultPopup, true);
        }
    }
}