using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(TankMovement))]
    public class PlayerInput : MonoBehaviour
    {
        private TankMovement _movement;

        private void Awake()
        {
            _movement = GetComponent<TankMovement>();
        }

        private void Update()
        {
            float move = Input.GetAxisRaw("Vertical");

            float turn = Input.GetAxisRaw("Horizontal");

            _movement.SetInput(move, turn);
        }
    }
}