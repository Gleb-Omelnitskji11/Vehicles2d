using System;
using System.Collections.Generic;
using Config;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;

namespace Core
{
    public class ConfigProvider : IConfigProvider
    {
        private readonly Dictionary<ConfigType, BaseConfigScript> _configs =
            new Dictionary<ConfigType, BaseConfigScript>();
        
        public async UniTask LoadConfigAsync()
        {
            _configs.Clear();
            foreach (ConfigType type in Enum.GetValues(typeof(ConfigType)))
            {
                BaseConfigScript config = await BaseConfigScript.LoadAsync(type);
                if (config != null)
                    _configs[type] = config;
            }
        }

        public T GetConfig<T>(ConfigType type) where T : BaseConfigScript
        {
            if (_configs.TryGetValue(type, out BaseConfigScript config) && config is T typedConfig)
                return typedConfig;

            Debug.LogError($"[ConfigProvider] Config '{type}' of type '{typeof(T).Name}' is not loaded.");
            return null;
        }
    }
}
