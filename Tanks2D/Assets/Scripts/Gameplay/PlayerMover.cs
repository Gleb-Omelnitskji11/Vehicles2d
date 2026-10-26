using Core.ServiceLocator;
using Gameplay.Attacking;
using Gameplay.UpdatingInstance;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(VehicleMovement))]
    public class PlayerMover : MonoBehaviour, IUpdatable
    {
        private VehicleMovement _movement;
        private BaseGun _weapon;
        private IGamePlayerInput _gamePlayerInput;

        public bool IgnorePause { get; } = false;

        private void Awake()
        {
            _movement = GetComponent<VehicleMovement>();
            _weapon = GetComponent<BaseGun>();
            _gamePlayerInput = Services.Get<IGamePlayerInput>();
            GameInstance gameInstance = Services.Get<GameInstance>();
            gameInstance.TryAddNewUpdatable(this);
        }

        public void OnUpdate()
        {
            float move = _gamePlayerInput.Speed;
            float turn = _gamePlayerInput.Rotation;
            
            if (move < 0f)
                turn *= -1f; //not Invert
            
            _movement.Move(move, turn);

            if (_gamePlayerInput.IsShooting)
            {
                _weapon.Fire();
            }
        }
    }
}