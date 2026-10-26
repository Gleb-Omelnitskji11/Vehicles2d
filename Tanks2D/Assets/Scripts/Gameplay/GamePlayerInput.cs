using Core.ServiceLocator;
using Gameplay.UpdatingInstance;
using UnityEngine;

namespace Gameplay
{
    public class GamePlayerInput : IInit, IUpdatable, IGamePlayerInput
    {
        public float Speed { get; private set; }
        public float Rotation { get; private set; }
        public bool IsShooting { get; private set; }
        public bool PausePressing { get; private set; }
        public bool IgnorePause { get; } = true;

        public void Init()
        {
            GameInstance gameInstance = Services.Get<GameInstance>();
            gameInstance.TryAddNewUpdatable(this);
        }

        public void OnUpdate()
        {
            Speed = Input.GetAxisRaw("Vertical");
            Rotation = Input.GetAxisRaw("Horizontal");
            IsShooting = Input.GetKey(KeyCode.Mouse0);
            PausePressing = Input.GetKeyDown(KeyCode.Escape);
        }
    }
}