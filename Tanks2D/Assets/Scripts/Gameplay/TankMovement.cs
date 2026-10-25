using System;
using Core.ServiceLocator;
using Gameplay.UpdatingInstance;
using UnityEngine;

namespace Gameplay
{
    public class TankMovement : VehicleMovement, IUpdatable
    {
        [SerializeField] private float _moveSpeed = 4;
        [SerializeField] private float _rotationSpeed = 120;

        private float _moveInput;
        private float _turnInput;
        private GameInstance _gameInstance;

        public bool IgnorePause { get; } = false;

        private void Start()
        {
            _gameInstance = Services.Get<GameInstance>();
            _gameInstance.TryAddNewUpdatable(this);
        }

        private void OnDestroy()
        {
            _gameInstance.AddToUnsubUpdate(this);
        }

        public override void Move(float move, float turn)
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