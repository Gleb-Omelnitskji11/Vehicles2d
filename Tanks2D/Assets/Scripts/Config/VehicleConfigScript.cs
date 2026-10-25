using System;
using Enums;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "VehicleConfig", menuName = "Configs/VehicleConfig")]
    public class VehicleConfigScript : ScriptableObject
    {
        [SerializeField] private VehicleData[] _vehicleData = new VehicleData[1];

        public GameObject GetVehiclePrefab(VehicleType vehicleType)
        {
            foreach (var vehicleData in _vehicleData)
            {
                if (vehicleData.VehicleType == vehicleType)
                {
                    return vehicleData.Prefab;
                }
            }
            
            Debug.LogError($"Tank Data {vehicleType} not found");
            throw new Exception("Tank Data Not Found");
        }
        
        
        [Serializable]
        public class VehicleData
        {
            public VehicleType VehicleType;
            public GameObject Prefab;
        }
    }
}