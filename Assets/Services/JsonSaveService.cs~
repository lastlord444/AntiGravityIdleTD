using System;
using System.IO;
using System.Threading.Tasks;
using AntiGravityTD.Core.Save;
using UnityEngine;

namespace AntiGravityTD.Services
{
    /// <summary>
    /// JSON tabanlı, local dosyaya save/ yapan implementasyon.
    /// PersistentDataPath kullanır (platform bağımsız).
    /// </summary>
    public class JsonSaveService : ISaveService
    {
        private const string SAVE_FILE_NAME = "antigravity_save.json";
        private readonly string _savePath;

        public JsonSaveService()
        {
            _savePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            Debug.Log($"[JsonSaveService] Save path: {_savePath}");
        }

        public string GetSavePath() => _savePath;

        public bool HasSaveData()
        {
            return File.Exists(_savePath);
        }

        public async Task<bool> SaveAsync(SaveSchema data)
        {
            if (data == null)
            {
                Debug.LogError("[JsonSaveService] Kaydedilecek veri null!");
                return false;
            }

            // lastPlayTimestamp güncelle
            data.UpdateLastPlayTime();
            data.saveTimestamp = DateTime.UtcNow.Ticks;

            try
            {
                var json = JsonUtility.ToJson(data, prettyPrint: true);
                
                // File write async simulation (Unity'de Task.Run ile)
                await Task.Run(() =>
                {
                    var directory = Path.GetDirectoryName(_savePath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    File.WriteAllText(_savePath, json);
                });

                Debug.Log($"[JsonSaveService] Save başarılı. Schema v{data.schemaVersion}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonSaveService] Save hatası: {ex.Message}");
                return false;
            }
        }

        public async Task<(bool success, SaveSchema data)> LoadAsync()
        {
            if (!HasSaveData())
            {
                Debug.LogWarning("[JsonSaveService] Kayıtlı save dosyası bulunamadı.");
                return (false, null);
            }

            try
            {
                string json = null;
                
                await Task.Run(() =>
                {
                    json = File.ReadAllText(_savePath);
                });

                var data = JsonUtility.FromJson<SaveSchema>(json);

                // Schema version kontrolü (migration için hazırlık)
                if (data.schemaVersion != 1)
                {
                    Debug.LogWarning($"[JsonSaveService] Schema version uyumsuz: {data.schemaVersion}. Expected: 1");
                    // İleride: MigrateSchema(data);
                }

                Debug.Log($"[JsonSaveService] Load başarılı. Schema v{data.schemaVersion}, Gold: {data.gold}");
                return (true, data);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonSaveService] Load hatası: {ex.Message}");
                return (false, null);
            }
        }

        public async Task<bool> DeleteSaveAsync()
        {
            if (!HasSaveData())
            {
                return true; // Zaten yok, başarılı say
            }

            try
            {
                await Task.Run(() =>
                {
                    File.Delete(_savePath);
                });

                Debug.Log("[JsonSaveService] Save dosyası silindi.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonSaveService] Silme hatası: {ex.Message}");
                return false;
            }
        }
    }
}
