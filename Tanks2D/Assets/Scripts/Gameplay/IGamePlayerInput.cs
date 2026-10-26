using Core.ServiceLocator;

namespace Gameplay
{
    public interface IGamePlayerInput : IService
    {
        float Speed { get; }
        float Rotation { get; }
        bool IsShooting { get; }
        bool PausePressing { get; }
    }
}