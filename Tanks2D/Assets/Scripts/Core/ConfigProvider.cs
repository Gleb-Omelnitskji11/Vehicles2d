using System.Threading.Tasks;
using Config;
using Core.ServiceLocator;
using UnityEngine;

namespace Core
{
    public class ConfigProvider : IService, IConfigProvider
    {
        private readonly string _configPath;
        private TankConfigScript _loadedConfig;

        public ConfigProvider(string configPath = "TankConfig")
        {
            _configPath = configPath;
        }
        
        public async Task<TankConfigScript> LoadConfigAsync()
        {
            ResourceRequest request = Resources.LoadAsync<TankConfigScript>(_configPath);

            var tcs = new TaskCompletionSource<bool>();
            request.completed += _ => tcs.SetResult(true);
            await tcs.Task;

            _loadedConfig = request.asset as TankConfigScript;

            if (_loadedConfig != null)
            {
                return _loadedConfig;
            }
        
            Debug.LogError($"[ConfigInstaller] Failed to load TankConfig from path: Resources/{_configPath}");
            return null;
        }

        public TankConfigScript TankConfig => _loadedConfig;
    }
}