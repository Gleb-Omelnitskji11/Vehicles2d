using Core.ServiceLocator;
using Enums;
using UnityEngine;

namespace Gameplay.Spawners
{
    public interface IProjectileSpawner : IService, IInit
    {
        BasicProjectile Spawn(ProjectileID projectileID, Vector2 position, Quaternion rotation);
    }
}