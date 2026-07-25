using Core.ServiceLocator;
namespace Gameplay.Spawners
{
    public interface IPlayerSpawner : IService, IInit
    {
        Vehicle SpawnPlayer();
    }
}