using System;
using Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Core.SaveLoadSystem
{
    [Serializable]
    public class SaveData
    {
        public const int CurrentVersion = 2;

        public int Version = CurrentVersion;
        public LevelSaveData LevelData;
        
        public PlayerSaveData PlayerData;
        public EnemySaveData[] Enemies;
        public ProjectileSaveData[] ProjectilesData;

        public bool TryValidateAndNormalize(out string error)
        {
            if (Version > CurrentVersion || Version < 0)
            {
                error = $"Unsupported save version: {Version}.";
                return false;
            }

            if (LevelData == null || LevelData.LevelIndex < 1 || PlayerData?.VehicleSaveData == null)
            {
                error = "Save is missing required level or player data.";
                return false;
            }

            LevelData.EnemiesLess ??= Array.Empty<EnemyType>();
            LevelData.NextEnemySpawners ??= Array.Empty<float>();
            Enemies ??= Array.Empty<EnemySaveData>();
            ProjectilesData ??= Array.Empty<ProjectileSaveData>();

            foreach (EnemyType enemyType in LevelData.EnemiesLess)
            {
                if (!Enum.IsDefined(typeof(EnemyType), enemyType))
                {
                    error = "Pending enemy type is invalid.";
                    return false;
                }
            }

            if (!IsValidVehicle(PlayerData.VehicleSaveData))
            {
                error = "Player position, rotation, or vehicle type is invalid.";
                return false;
            }

            foreach (EnemySaveData enemy in Enemies)
            {
                if (enemy?.VehicleSaveData == null || !Enum.IsDefined(typeof(EnemyType), enemy.EnemyType) ||
                    !IsValidVehicle(enemy.VehicleSaveData))
                {
                    error = "Enemy data is incomplete or invalid.";
                    return false;
                }
            }

            foreach (ProjectileSaveData projectile in ProjectilesData)
            {
                if (projectile == null || !Enum.IsDefined(typeof(ProjectileID), projectile.ProjectileID) ||
                    !Enum.IsDefined(typeof(Team), projectile.Team) || !IsValidPosition(projectile.Position) ||
                    !IsValidRotation(projectile.Rotation) || float.IsNaN(projectile.Lifetime) ||
                    float.IsInfinity(projectile.Lifetime) || projectile.Lifetime < 0f)
                {
                    error = "Projectile data is incomplete or invalid.";
                    return false;
                }
            }

            foreach (float timer in LevelData.NextEnemySpawners)
            {
                if (float.IsNaN(timer) || float.IsInfinity(timer) || timer < 0f)
                {
                    error = "Enemy spawn timer data is invalid.";
                    return false;
                }
            }

            Version = CurrentVersion;
            error = null;
            return true;
        }

        private static bool IsValidVehicle(VehicleSaveData vehicle)
        {
            if (vehicle == null || !Enum.IsDefined(typeof(VehicleType), vehicle.VehicleType) ||
                !IsValidPosition(vehicle.Position) || !IsValidRotation(vehicle.Rotation))
                return false;

            if (vehicle.GunSaveData == null)
                return true;

            float cooldown = vehicle.GunSaveData.Data?.Cooldawn ?? -1f;
            return !float.IsNaN(cooldown) && !float.IsInfinity(cooldown) && cooldown >= 0f;
        }

        private static bool IsValidPosition(SerializableVector2 position)
        {
            return !float.IsNaN(position.X) && !float.IsInfinity(position.X) &&
                   !float.IsNaN(position.Y) && !float.IsInfinity(position.Y);
        }

        private static bool IsValidRotation(SerializableQuaternion rotation)
        {
            if (float.IsNaN(rotation.x) || float.IsInfinity(rotation.x) ||
                float.IsNaN(rotation.y) || float.IsInfinity(rotation.y) ||
                float.IsNaN(rotation.z) || float.IsInfinity(rotation.z) ||
                float.IsNaN(rotation.w) || float.IsInfinity(rotation.w))
                return false;

            float magnitude = rotation.x * rotation.x + rotation.y * rotation.y +
                              rotation.z * rotation.z + rotation.w * rotation.w;
            return magnitude > 0.0001f;
        }
    }
    
    [Serializable]
    public class LevelSaveData
    {
        public EnemyType[] EnemiesLess;
        public int LevelIndex;
        public float[] NextEnemySpawners;
    }
    
    [Serializable]
    public class PlayerSaveData
    {
        public VehicleSaveData VehicleSaveData;
    }
    
    [Serializable]
    public class EnemySaveData
    {
        public VehicleSaveData VehicleSaveData;
        public EnemyType EnemyType;
    }
    
    [Serializable]
    public class VehicleSaveData
    {
        [JsonConverter(typeof(SerializableVector2Converter))]
        public SerializableVector2 Position;
        public SerializableQuaternion Rotation;
        public VehicleType VehicleType;
        public GunSaveData GunSaveData;
    }

    public class GunSaveData
    {
        public SimpleGunSaveData Data;
    }
    
    [Serializable]
    public class SimpleGunSaveData
    {
        public float Cooldawn;
    }

    [Serializable]
    public class ProjectileSaveData
    {
        [JsonConverter(typeof(SerializableVector2Converter))]
        public SerializableVector2 Position;
        public SerializableQuaternion Rotation;
        public Team Team;
        public ProjectileID ProjectileID;
        public float Lifetime;
    }

    [Serializable]
    public struct SerializableVector2
    {
        public float X;
        public float Y;

        public SerializableVector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2 ToVector2() => new Vector2(X, Y);
    }

    public sealed class SerializableVector2Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(SerializableVector2);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                JArray values = (JArray)token;
                return new SerializableVector2(ReadFloat(values, 0), ReadFloat(values, 1));
            }

            if (token.Type == JTokenType.Object)
            {
                JObject values = (JObject)token;
                return new SerializableVector2(
                    ReadFloat(values, "X", "x", "Item1", "item1"),
                    ReadFloat(values, "Y", "y", "Item2", "item2"));
            }

            throw new JsonSerializationException("Expected a coordinate object or array.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            SerializableVector2 position = (SerializableVector2)value;
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(position.X);
            writer.WritePropertyName("y");
            writer.WriteValue(position.Y);
            writer.WriteEndObject();
        }

        private static float ReadFloat(JArray values, int index)
        {
            return values.Count > index && values[index].Type != JTokenType.Null ? values[index].Value<float>() : 0f;
        }

        private static float ReadFloat(JObject values, params string[] names)
        {
            foreach (JProperty property in values.Properties())
            {
                foreach (string name in names)
                {
                    if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase) &&
                        property.Value.Type != JTokenType.Null)
                        return property.Value.Value<float>();
                }
            }

            return 0f;
        }
    }
    
    [Serializable]
    public struct SerializableQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerializableQuaternion(Quaternion q)
        {
            x = q.x;
            y = q.y;
            z = q.z;
            w = q.w;
        }

        public Quaternion ToQuaternion() => new (x, y, z, w);
    }
}
