using System;
using Core.ServiceLocator;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.SaveLoadSystem
{
    public class PlayerPrefsService : ISaveSystem
    {
        private GameDataSerialization _gameDataSerialization;
        private const string SaveKey = "SaveKey";
        private const string InvalidSaveBackupKey = "SaveKey.InvalidBackup";

        public void OnEntryGameScene()
        {
            _gameDataSerialization = Services.Get<GameDataSerialization>();
        }

        public void Save()
        {
            if (_gameDataSerialization == null)
            {
                Debug.LogError("[SaveSystem] Cannot save before the game scene is initialized.");
                return;
            }

            try
            {
                SaveData data = _gameDataSerialization.GetSaveData();
                if (data == null)
                {
                    Debug.LogError("[SaveSystem] Refusing to write invalid save data: data is empty.");
                    return;
                }

                if (!data.TryValidateAndNormalize(out string error))
                {
                    Debug.LogError($"[SaveSystem] Refusing to write invalid save data: {error}");
                    return;
                }

                string json = JsonConvert.SerializeObject(data);
                PlayerPrefs.SetString(SaveKey, json);
                PlayerPrefs.Save();
            }
            catch (JsonException exception)
            {
                Debug.LogError($"[SaveSystem] Could not serialize save data: {exception.Message}");
            }
        }

        public bool HasSaved()
        {
            return GetSaveData() != null;
        }

        public SaveData GetSaveData()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
                return null;

            string json = PlayerPrefs.GetString(SaveKey, string.Empty);
            if (string.IsNullOrWhiteSpace(json))
            {
                DeleteInvalidSave("save data is empty");
                return null;
            }

            try
            {
                SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
                if (data == null)
                {
                    DeleteInvalidSave("JSON contains null");
                    return null;
                }

                if (data.TryValidateAndNormalize(out string error))
                    return data;

                DeleteInvalidSave(error);
            }
            catch (Exception exception) when (IsSaveFormatException(exception))
            {
                DeleteInvalidSave($"JSON could not be parsed: {exception.Message}");
            }

            return null;
        }

        private static void DeleteInvalidSave(string reason)
        {
            Debug.LogWarning($"[SaveSystem] Removing invalid save: {reason}");
            if (!PlayerPrefs.HasKey(InvalidSaveBackupKey))
                PlayerPrefs.SetString(InvalidSaveBackupKey, PlayerPrefs.GetString(SaveKey, string.Empty));
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
        }

        private static bool IsSaveFormatException(Exception exception)
        {
            return exception is JsonException || exception is FormatException ||
                   exception is OverflowException || exception is InvalidCastException;
        }
    }
}
