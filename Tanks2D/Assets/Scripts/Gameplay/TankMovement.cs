using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TankMovement : MonoBehaviour, IFixedUpdatable
    {
        [SerializeField] private float _moveSpeed = 4;
        [SerializeField] private float _rotationSpeed = 120;

        private Rigidbody2D _rb;

        private float _moveInput;
        private float _turnInput;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Move(float move, float turn)
        {
            if (move == 0f) turn = 0f; // from test description: "no move withous W/S"
            if(move <= 0f) turn *= -1f; //not Invert
            _moveInput = move;
            _turnInput = turn;
        }

        public void OnFixedUpdate()
        {
            _rb.MoveRotation(_rb.rotation - _turnInput * _rotationSpeed * Time.fixedDeltaTime);
            
            Vector2 movement = (Vector2)transform.up * (_moveInput * _moveSpeed * Time.fixedDeltaTime);
            _rb.MovePosition(_rb.position + movement);
        }
    }
}