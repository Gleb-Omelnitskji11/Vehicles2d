using System;
using System.Threading.Tasks;
using Core.ServiceLocator;
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

        private async Task Binding()
        {
            IServiceLocator serviceLocator = new ServiceLocator.ServiceLocator();
            serviceLocator.Register<IServiceLocator>(serviceLocator);
            
            ConfigProvider configProvider = new ConfigProvider();
            serviceLocator.Register<IConfigProvider>(configProvider);

            await configProvider.LoadConfigAsync();
        }

        private void LoadNextScene()
        {
            // int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // string sceneName = SceneManager.GetSceneByBuildIndex(currentSceneIndex + 1).name;
            string sceneName = "GameScene";
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            if (operation == null)
            {
                throw new InvalidOperationException($"Failed to start loading scene '{sceneName}'.");
            }
        }
    }
}