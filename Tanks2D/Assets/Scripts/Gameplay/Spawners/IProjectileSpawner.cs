using Core.ServiceLocator;
using Enums;
using UnityEngine;

namespace Gameplay.Spawners
{
    public interface IProjectileSpawner : IService, IInit
    {
        BasicProjectile SpawnProjectile(ProjectileID projectileID);
    }
}