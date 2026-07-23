using Core.ServiceLocator;
using Enums;
using UnityEngine;

namespace Gameplay.Spawners
{
    public class PlayerSpawner : TankSpawner, IPlayerSpawner
    {
        public GameObject SpawnPlayer()
        {
            var config = _configProvider.TankConfig;
            GameObject tankPrefab = config.GetTankPrefab(TankID.Default);
            GameObject player = Spawn(tankPrefab);
            player.AddComponent<PlayerInput>();
            player.name = "Player";
            player.tag = "Player";
            player.layer = LayerMask.NameToLayer("Player");
            _tanksRegister.RegisterPlayer(player);

            return player;
        }
        
    }
}