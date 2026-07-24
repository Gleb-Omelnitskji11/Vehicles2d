using System;
using Enums;
using UnityEngine;

namespace Gameplay.Spawners
{
    public class PlayerSpawner : TankSpawner, IPlayerSpawner
    {
        public Tank SpawnPlayer()
        {
            var config = TankConfigScript;
            GameObject tankPrefab = config.GetTankPrefab(TankModel.Default);
            Tank player = Spawn(tankPrefab);
            TankWeapon gun = player.gameObject.AddComponent<TankWeapon>();
            player.gameObject.AddComponent<PlayerInput>();
            gun.Init(1f, ProjectileID.Bullet, Team.Ally); //hardCode
            
            player.name = "Player";
            player.tag = "Player";
            player.gameObject.layer = LayerMask.NameToLayer("Player");
            TanksRegister.RegisterTank(player);
            
            player.Init(new BasicUnitHealth(1), Team.Ally);

            return player;
        }
        
    }
}