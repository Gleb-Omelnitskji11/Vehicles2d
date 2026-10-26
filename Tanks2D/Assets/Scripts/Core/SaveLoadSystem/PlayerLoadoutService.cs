using System;
using Config;
using Core.ServiceLocator;
using Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.SaveLoadSystem
{
    public sealed class PlayerLoadoutService : IPlayerLoadoutService, IInit
    {
        private const string LoadoutKey = "PlayerLoadout";
        private const int CurrentVersion = 1;

        private PlayerLoadoutData _loadout;

        public VehicleType SelectedVehicle => ParseVehicle(_loadout.VehicleId);
        public ProjectileID SelectedProjectile => ParseProjectile(_loadout.ProjectileId);

        public void Init()
        {
            StartDataScript startConfig = Services.Get<IConfigProvider>()
                .GetConfig<StartDataScript>(ConfigType.StartDataConfig);

            if (startConfig == null)
                throw new InvalidOperationException("StartDataConfig is required to initialize the player loadout.");

            if (PlayerPrefs.HasKey(LoadoutKey))
            {
                try
                {
                    PlayerLoadoutData savedLoadout = JsonConvert.DeserializeObject<PlayerLoadoutData>(
                        PlayerPrefs.GetString(LoadoutKey));

                    if (IsValid(savedLoadout))
                    {
                        _loadout = savedLoadout;
                        return;
                    }
                }
                catch (JsonException exception)
                {
                    Debug.LogWarning($"[PlayerLoadout] Stored loadout could not be read: {exception.Message}");
                }

                Debug.LogWarning("[PlayerLoadout] Stored loadout is invalid; using the start config.");
            }

            _loadout = new PlayerLoadoutData
            {
                Version = CurrentVersion,
                VehicleId = Enum.GetName(typeof(VehicleType), startConfig.StartPlayerVehicle),
                ProjectileId = Enum.GetName(typeof(ProjectileID), startConfig.StartPlayerProjectile)
            };

            if (!IsValid(_loadout))
                throw new InvalidOperationException("StartDataConfig contains an unsupported vehicle or projectile ID.");

            Save();
        }

        public void SetLoadout(VehicleType vehicle, ProjectileID projectile)
        {
            if (!Enum.IsDefined(typeof(VehicleType), vehicle) || !Enum.IsDefined(typeof(ProjectileID), projectile))
                throw new ArgumentOutOfRangeException(nameof(vehicle), "The selected loadout contains an unsupported ID.");

            _loadout = new PlayerLoadoutData
            {
                Version = CurrentVersion,
                VehicleId = Enum.GetName(typeof(VehicleType), vehicle),
                ProjectileId = Enum.GetName(typeof(ProjectileID), projectile)
            };
            Save();
        }

        private static bool IsValid(PlayerLoadoutData data)
        {
            return data != null && data.Version == CurrentVersion &&
                   Enum.TryParse(data.VehicleId, false, out VehicleType vehicle) &&
                   Enum.IsDefined(typeof(VehicleType), vehicle) &&
                   Enum.TryParse(data.ProjectileId, false, out ProjectileID projectile) &&
                   Enum.IsDefined(typeof(ProjectileID), projectile);
        }

        private static VehicleType ParseVehicle(string id)
        {
            Enum.TryParse(id, false, out VehicleType vehicle);
            return vehicle;
        }

        private static ProjectileID ParseProjectile(string id)
        {
            Enum.TryParse(id, false, out ProjectileID projectile);
            return projectile;
        }

        private void Save()
        {
            PlayerPrefs.SetString(LoadoutKey, JsonConvert.SerializeObject(_loadout));
            PlayerPrefs.Save();
        }

        [Serializable]
        private sealed class PlayerLoadoutData
        {
            public int Version;
            public string VehicleId;
            public string ProjectileId;
        }
    }
}
