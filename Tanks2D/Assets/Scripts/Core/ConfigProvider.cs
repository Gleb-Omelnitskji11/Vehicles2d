using Config;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public class ConfigProvider : IConfigProvider
    {
        private readonly string _configPath;
        private TankConfigScript _tankConfig;
        private ProjectileConfigScript _projectileConfig;

        private const string TankConfigPath =  "TankConfig";
        private const string ProjectileConfigPath = "ProjectileConfig";
        
        public async UniTask LoadConfigAsync()
        {
            ResourceRequest tankRequest = Resources.LoadAsync<TankConfigScript>(TankConfigPath);
            ResourceRequest projectileRequest = Resources.LoadAsync<ProjectileConfigScript>(ProjectileConfigPath);
            
            await UniTask.WhenAll(tankRequest.ToUniTask(), projectileRequest.ToUniTask());

            _tankConfig = tankRequest.asset as TankConfigScript;
            _projectileConfig = projectileRequest.asset as ProjectileConfigScript;

            if (_tankConfig == null)
            {
                Debug.LogError($"[ConfigProvider] Failed to load TankConfig from path: Resources/{TankConfigPath}");
            }
            else
            {
                Debug.Log($"[ConfigProvider] TankConfig successfully loaded!");
            }

            if (_projectileConfig == null)
            {
                Debug.LogError($"[ConfigProvider] Failed to load ProjectileConfig from path: Resources/{ProjectileConfigPath}");
            }
            else
            {
                Debug.Log($"[ConfigProvider] ProjectileConfig successfully loaded!");
            }
        }

        public TankConfigScript TankConfig => _tankConfig;
        public ProjectileConfigScript ProjectileConfig => _projectileConfig;
    }
}