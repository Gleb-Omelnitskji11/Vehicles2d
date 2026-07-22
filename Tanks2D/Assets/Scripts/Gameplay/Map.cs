using UnityEngine;

namespace Gameplay
{
    public class Map : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _size;
        
        public Vector2 Size => _size;
        
        public Vector2 GetRandomPoint()
        {
            return new Vector2(
                Random.Range(-_size.x, _size.x),
                Random.Range(-_size.y, _size.y)
            );
        }
    }
}
