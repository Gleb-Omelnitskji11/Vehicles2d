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
            try
            {
                await Binding();
                LoadNextScene();
            }
            catch (Exception exception)
            {
                Debug.LogError($"[EntryBootstrap] Preloading failed. The game scene was not loaded.\n{exception}");
            }
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
