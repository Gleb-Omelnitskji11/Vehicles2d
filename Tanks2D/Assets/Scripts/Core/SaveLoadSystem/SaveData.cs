using System;
using Enums;
using UnityEngine;

namespace Core.SaveLoadSystem
{
    [Serializable]
    public class SaveData
    {
        public LevelSaveData LevelData;
        
        public PlayerSaveData PlayerData;
        public EnemySaveData[] Enemies;
        public ProjectileSaveData[] ProjectilesData;
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
        public (float, float) Position;
        public SerializableQuaternion Rotation;
        public VehicleType VehicleType;
        public GunSaveData GunSaveData;
    }

    public class GunSaveData
    {
        public Type GunType;
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
        public (float, float) Position;
        public SerializableQuaternion Rotation;
        public Team Team;
        public ProjectileID ProjectileID;
        public float Lifetime;
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