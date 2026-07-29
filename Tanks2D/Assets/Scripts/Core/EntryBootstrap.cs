using System;
using Core.SaveLoadSystem;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class EntryBootstrap : MonoBehaviour
    {
        private void Start()
        {
            Preloading();
        }

        private async void Preloading()
        {
            await Binding();
            LoadNextScene();
        }

        private async UniTask Binding()
        {
            await UniTask.DelayFrame(1);
            IServiceLocator serviceLocator = new ServiceLocator.ServiceLocator();
            serviceLocator.Register<IServiceLocator>(serviceLocator);
            
            ConfigProvider configProvider = new ConfigProvider();
            serviceLocator.Register<IConfigProvider>(configProvider);

            ISaveSystem saveSystem = new PlayerPrefsService();
            serviceLocator.Register<ISaveSystem>(saveSystem);

            await configProvider.LoadConfigAsync();
        }

        private void LoadNextScene()
        {
            string sceneName = Constants.GameScene;
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            if (operation == null)
            {
                throw new InvalidOperationException($"Failed to start loading scene '{sceneName}'.");
            }
        }
    }
}