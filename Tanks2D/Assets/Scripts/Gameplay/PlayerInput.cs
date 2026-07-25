using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(TankMovement))]
    public class PlayerInput : MonoBehaviour, IUpdatable
    {
        private TankMovement _movement;
        private SimpleGun _weapon;

        private void Awake()
        {
            _movement = GetComponent<TankMovement>();
            _weapon = GetComponent<SimpleGun>();
        }

        public void OnUpdate()
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