using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(TankMovement))]
    public class PlayerInput : MonoBehaviour
    {
        private TankMovement _movement;
        private SimpleGun _weapon;

        private void Awake()
        {
            _movement = GetComponent<TankMovement>();
            _weapon = GetComponent<SimpleGun>();
        }

        private void Update()
        {
            float move = Input.GetAxisRaw("Vertical");
            float turn = Input.GetAxisRaw("Horizontal");
            _movement.Move(move, turn);
            
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _weapon.Fire();
            }
        }
    }
}