using Core.ServiceLocator;

namespace Core.SaveLoadSystem
{
    public interface ISaveSystem : IService
    {
        void OnEntryGameScene();
        //void Init();
        void Save();
        bool HasSaved();
        SaveData GetSaveData();
    }
}