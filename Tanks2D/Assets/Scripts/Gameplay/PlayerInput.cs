using Gameplay.Attacking;
using Gameplay.UpdatingInstance;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(VehicleMovement))]
    public class PlayerInput : MonoBehaviour, IUpdatable
    {
        private VehicleMovement _movement;
        private BaseGun _weapon;

        private void Awake()
        {
            _movement = GetComponent<VehicleMovement>();
            _weapon = GetComponent<BaseGun>();
        }

        public void OnUpdate()
        {
            float move = Input.GetAxisRaw("Vertical");
            float turn = Input.GetAxisRaw("Horizontal");
            if(move <= 0f) turn *= -1f; //not Invert
            _movement.Move(move, turn);
            
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _weapon.Fire();
            }
        }
    }
}