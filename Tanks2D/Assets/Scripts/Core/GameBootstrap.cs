using Core.ServiceLocator;
using Gameplay;
using UnityEngine;

namespace Core
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private LevelLoader _levelLoader;

        private void Awake()
        {
            IServiceLocator serviceLocator;
            if(!Services.TryGet<IServiceLocator>(out serviceLocator))
            {
                Debug.LogError("Service Locator could not be found.");
                return;
            }
            
            serviceLocator.Register<LevelLoader>(_levelLoader);
            serviceLocator.Register<PlayerSpawner>(new PlayerSpawner());
            serviceLocator.Register<GameProgresor>(new GameProgresor());

            StartGame();
        }

        private void StartGame()
        {
            Services.Get<GameProgresor>().StartGame();
        }
    }
}