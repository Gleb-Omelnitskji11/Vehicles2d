using Core.ServiceLocator;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class LevelLoader : MonoBehaviour, IService
    {
        [SerializeField] private Map _map;
        [SerializeField] private GameObject _tankPrefab;
        
        public Map Map => _map;
        public GameObject TankPrefab => _tankPrefab;
    }
}