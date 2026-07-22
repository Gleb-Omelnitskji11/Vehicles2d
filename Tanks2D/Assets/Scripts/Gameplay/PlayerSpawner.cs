using Core.ServiceLocator;
using UnityEngine;

namespace Gameplay
{
    public class PlayerSpawner : IPlayerSpawner
    {
        private LevelLoader _levelLoader;
        private GameObject _player;

        public void Init()
        {
            _levelLoader = Services.Get<LevelLoader>();
        }

        public GameObject Spawn()
        {
            Vector2 position = _levelLoader.Map.GetRandomPoint();

            _player = Object.Instantiate(_levelLoader.TankPrefab, position, Quaternion.identity, _levelLoader.Map.transform);
            _player.AddComponent<PlayerInput>();

            Vector2 dir = -position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

            _player.transform.rotation =
                Quaternion.Euler(0, 0, angle);

            return _player;
        }
    }
}