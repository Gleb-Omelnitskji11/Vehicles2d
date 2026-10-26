using Core.ServiceLocator;
using Enums;

namespace Core.SaveLoadSystem
{
    public interface IPlayerLoadoutService : IService
    {
        VehicleType SelectedVehicle { get; }
        ProjectileID SelectedProjectile { get; }
        void SetLoadout(VehicleType vehicle, ProjectileID projectile);
    }
}
