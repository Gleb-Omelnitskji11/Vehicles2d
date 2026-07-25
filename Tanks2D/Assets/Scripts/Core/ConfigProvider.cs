using Config;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public class ConfigProvider : IConfigProvider
    {
        private readonly string _configPath;
        private VehicleConfigScript _vehicleConfig;
        private ProjectileConfigScript _projectileConfig;

        private const string VehicleConfigPath =  "VehicleConfig";
        private const string ProjectileConfigPath = "ProjectileConfig";
        
        public async UniTask LoadConfigAsync()
        {
            ResourceRequest tankRequest = Resources.LoadAsync<VehicleConfigScript>(VehicleConfigPath);
            ResourceRequest projectileRequest = Resources.LoadAsync<ProjectileConfigScript>(ProjectileConfigPath);
            
            await UniTask.WhenAll(tankRequest.ToUniTask(), projectileRequest.ToUniTask());

            _vehicleConfig = tankRequest.asset as VehicleConfigScript;
            _projectileConfig = projectileRequest.asset as ProjectileConfigScript;

            if (_vehicleConfig == null)
            {
                Debug.LogError($"[ConfigProvider] Failed to load TankConfig from path: Resources/{VehicleConfigPath}");
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

        public VehicleConfigScript VehicleConfig => _vehicleConfig;
        public ProjectileConfigScript ProjectileConfig => _projectileConfig;
    }
}