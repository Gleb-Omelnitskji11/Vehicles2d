using Core.ServiceLocator;
using Enums;
using Gameplay.Attacking;
using UnityEngine;

namespace Gameplay.Spawners
{
    public interface IProjectileSpawner : IService, IInit
    {
        BasicProjectile SpawnProjectile(ProjectileID projectileID);
    }
}