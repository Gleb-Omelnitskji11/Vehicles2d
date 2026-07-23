using Core.ServiceLocator;
using UnityEngine;

namespace Gameplay
{
    public class MapLoader : MonoBehaviour, IService
    {
        [SerializeField] private Map _map;
        
        public Map Map => _map;
    }
}