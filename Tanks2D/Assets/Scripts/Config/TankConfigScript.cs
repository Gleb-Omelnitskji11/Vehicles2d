using System;
using Enums;
using Gameplay;
using Gameplay.Spawners;
using UnityEngine;
using UnityEngine.Serialization;

namespace Config
{
    [CreateAssetMenu(fileName = "TankConfig", menuName = "Configs/TankConfig")]
    public class TankConfigScript : ScriptableObject
    {
        [SerializeField] private TankPrefab[] _tankPrefabs = new TankPrefab[1];

        public GameObject GetTankPrefab(TankModel tankModel)
        {
            foreach (var tankPrefab in _tankPrefabs)
            {
                if (tankPrefab.TankModel == tankModel)
                {
                    return tankPrefab.Prefab;
                }
            }
            
            Debug.LogError($"Tank Prefab {tankModel} not found");
            throw new Exception("Tank Prefab Not Found");
        }
        
        
        [Serializable]
        public class TankPrefab
        {
            public TankModel TankModel;
            public GameObject Prefab;
        }
    }
}