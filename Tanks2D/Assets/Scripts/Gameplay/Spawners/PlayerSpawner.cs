using Enums;
using UnityEngine;

namespace Gameplay.Spawners
{
    public class PlayerSpawner : TankSpawner, IPlayerSpawner
    {
        public Vehicle SpawnPlayer()
        {
            Vehicle player = SpawnVehicle(VehicleType.Tank);
            SimpleGun gun = player.gameObject.AddComponent<SimpleGun>();
            player.gameObject.AddComponent<PlayerInput>();
            gun.Init(1f, ProjectileID.Bullet, Team.Ally); //hardCode
            
            player.name = "Player";
            player.tag = "Player";
            player.gameObject.layer = LayerMask.NameToLayer("Player");
            
            player.Init(new BasicUnitHealth(1), Team.Ally);

            return player;
        }
        
    }
}