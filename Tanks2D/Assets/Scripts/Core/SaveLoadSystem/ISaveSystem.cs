using Core.ServiceLocator;

namespace Core.SaveLoadSystem
{
    public interface ISaveSystem : IService
    {
        void OnEntryGameScene();
        void Save();
        bool HasSaved();
        SaveData GetSaveData();
    }
}