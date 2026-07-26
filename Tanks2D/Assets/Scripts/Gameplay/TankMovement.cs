using Gameplay.UpdatingInstance;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TankMovement : MonoBehaviour, IUpdatable
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
            _moveInput = move;
            _turnInput = turn;
        }

        public void OnUpdate()
        {
            Transform tr = transform;

            tr.Rotate(0f, 0f, -_turnInput * _rotationSpeed * Time.deltaTime);

            tr.position += tr.up * (_moveInput * _moveSpeed * Time.deltaTime);
        }
    }
}