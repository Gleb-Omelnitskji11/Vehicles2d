using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.ServiceLocator
{
    public class EntryBootstrap : MonoBehaviour
    {
        private void Start()
        {
            var serviceLocator = new ServiceLocator();
            serviceLocator.Register(serviceLocator);
            LoadNextScene();
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