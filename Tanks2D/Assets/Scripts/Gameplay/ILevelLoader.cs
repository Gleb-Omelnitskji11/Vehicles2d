using Core.ServiceLocator;

namespace Gameplay
{
    public interface ILevelLoader : IService
    {
        LevelData LoadLevel(int levelIndex);
    }
}