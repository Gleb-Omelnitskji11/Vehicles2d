using Core.ServiceLocator;

namespace Gameplay
{
    public class GameProgresor : IService, IInit
    {
        private LevelLoader _levelLoader;
        private PlayerSpawner _playerSpawner;

        public void Init()
        {
            _levelLoader = Services.Get<LevelLoader>();
            _playerSpawner = Services.Get<PlayerSpawner>();
        }

        public void StartGame()
        {
            _playerSpawner.Spawn();
        }
    }
}