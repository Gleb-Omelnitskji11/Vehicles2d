using Core.ServiceLocator;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.SaveLoadSystem
{
    public class PlayerPrefsService : ISaveSystem
    {
        private GameDataSerialization _gameDataSerialization;
        private const string SaveKey = "SaveKey";

        public void OnEntryGameScene()
        {
            _gameDataSerialization = Services.Get<GameDataSerialization>();
        }

        public void Save()
        {
            var data = _gameDataSerialization.GetSaveDate();
            string json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public bool HasSaved()
        {
            return PlayerPrefs.HasKey(SaveKey);
        }

        public SaveData GetSaveData()
        {
            var json = PlayerPrefs.GetString(SaveKey, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            var data = JsonConvert.DeserializeObject<SaveData>(json);
            return data;
        }
    }
}