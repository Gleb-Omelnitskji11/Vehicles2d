using System;
using Enums;
using Gameplay.Spawners;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "TankConfig", menuName = "Configs/TankConfig")]
    public class TankConfigScript : ScriptableObject
    {
        [SerializeField] private TankPrefab[] _tankPrefabs = new TankPrefab[1];

        public GameObject GetTankPrefab(TankID tankID)
        {
            foreach (var tankPrefab in _tankPrefabs)
            {
                if (tankPrefab.TankID == tankID)
                {
                    return tankPrefab.Prefab;
                }
            }
            
            Debug.LogError($"Tank Prefab {tankID} not found");
            throw new Exception("Tank Prefab Not Found");
        }
        
        
        [Serializable]
        public class TankPrefab
        {
            public TankID TankID;
            public GameObject Prefab;
        }
    }
}