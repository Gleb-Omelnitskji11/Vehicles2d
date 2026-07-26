using Core.ServiceLocator;
using Enums;
using Gameplay.Attacking;
using Gameplay.UpdatingInstance;
using UnityEngine;

namespace Gameplay.Spawners
{
    public class PlayerSpawner : IPlayerSpawner
    {
        private VehicleSpawner _vehicleSpawner;
        protected TanksRegister VehicleRegister;
        private GameInstance _gameInstance;

        public void Init()
        {
            VehicleRegister = Services.Get<TanksRegister>();
            _vehicleSpawner = Services.Get<VehicleSpawner>();
            _gameInstance = Services.Get<GameInstance>();
        }

        public Vehicle SpawnPlayer()
        {
            if (VehicleRegister.Player != null)
            {
                ClearOldPlayer();
            }

            Vehicle player = _vehicleSpawner.SpawnVehicle(VehicleType.Tank, true); //hardCode
            SimpleGun gun = player.gameObject.AddComponent<SimpleGun>(); //move to AttackFactory
            var playerInput = player.gameObject.AddComponent<PlayerInput>();
            gun.Init(1f, ProjectileID.Bullet, Team.Ally); //hardCode

            string playerString = "Player";
            player.name = playerString;
            player.tag = playerString;
            player.gameObject.layer = LayerMask.NameToLayer(playerString);

            player.Init(new BasicUnitHealth(1), Team.Ally);
            VehicleRegister.RegisterPlayer(player);
            _gameInstance.Register(playerInput);

            return player;
        }

        private void ClearOldPlayer()
        {
            var input = VehicleRegister.Player.GetComponent<PlayerInput>();
            var movement = VehicleRegister.Player.GetComponent<TankMovement>();
            Object.Destroy(VehicleRegister.Player.gameObject);
            _gameInstance.Unregister(input);
            _gameInstance.Unregister(movement);
            VehicleRegister.UnRegisterPlayer();
        }
    }
}