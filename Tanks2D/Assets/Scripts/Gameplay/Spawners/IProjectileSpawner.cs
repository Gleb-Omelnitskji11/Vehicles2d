using Core.SaveLoadSystem;
using Core.ServiceLocator;
using Enums;
using Gameplay.Attacking;

namespace Gameplay.Spawners
{
    public interface IProjectileSpawner : IService, IInit
    {
        BasicProjectile SpawnProjectile(ProjectileID projectileID);
        BasicProjectile SpawnProjectileFromData(ProjectileSaveData projectileID);
    }
}