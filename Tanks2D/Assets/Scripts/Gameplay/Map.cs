using UnityEngine;

namespace Gameplay
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private Vector2 _size;
        [SerializeField] private Vector2 _padding = new Vector2(3f, 4f);
        
        public Vector2 Size => _size;
        
        public Vector2 GetRandomPoint()
        {
            return new Vector2(
                Random.Range(-_size.x, _size.x),
                Random.Range(-_size.y, _size.y)
            );
        }
        
        public Vector2 GetRandomPointOnBorder()
        {
            int side = Random.Range(0, 4);

            float xBound = Mathf.Max(-_size.x + _padding.x, _size.x - _padding.x);
            float yBound = Mathf.Max(-_size.y + _padding.x, _size.y - _padding.y);

            return side switch
            {
                0 => new Vector2(-xBound, Random.Range(-yBound, yBound)),
                1 => new Vector2(xBound, Random.Range(-yBound, yBound)),
                2 => new Vector2(Random.Range(-xBound, xBound), -yBound),
                3 => new Vector2(Random.Range(-xBound, xBound), yBound),
                _ => Vector2.zero
            };
        }
    }
}
