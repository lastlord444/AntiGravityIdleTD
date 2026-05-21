using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace BlockForge.Save
{
    /// <summary>
    /// Save/Load servisi. JSON formatında yerel dosyaya kayıt.
    /// Android: persistentDataPath kullanılır.
    /// </summary>
    public class SaveService
    {
        private const string SAVE_FILE_NAME = "blockforge_save.json";
        private string _savePath;
        
        public SaveService()
        {
            _savePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            Debug.Log($"[SaveService] Kayıt dosyası yolu: {_savePath}");
        }
        
        /// <summary>
        /// Veriyi kaydeder.
        /// </summary>
        public void Save(SaveModel data)
        {
            if (data == null)
            {
                Debug.LogError("[SaveService] Save verisi null!");
                return;
            }
            
            try
            {
                // Dictionary'leri serialize etmek için wrapper kullan
                SaveModelSerializable serializable = ConvertToSerializable(data);
                string json = JsonUtility.ToJson(serializable, true);
                File.WriteAllText(_savePath, json);
                Debug.Log($"[SaveService] Kayıt yapıldı. Boyut: {json.Length} byte.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveService] Kayıt hatası: {e.Message}");
            }
        }
        
        /// <summary>
        /// Kaydedilmiş veriyi yükler. Dosya yoksa null döner.
        /// </summary>
        public SaveModel Load()
        {
            if (!File.Exists(_savePath))
            {
                Debug.Log("[SaveService] Kayıt dosyası bulunamadı, yeni oyun.");
                return null;
            }
            
            try
            {
                string json = File.ReadAllText(_savePath);
                SaveModelSerializable serializable = JsonUtility.FromJson<SaveModelSerializable>(json);
                SaveModel data = ConvertFromSerializable(serializable);
                Debug.Log($"[SaveService] Kayıt yüklendi. Coins: {data.coins}");
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveService] Yükleme hatası: {e.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Kayıt dosyasını siler.
        /// </summary>
        public void DeleteSave()
        {
            if (File.Exists(_savePath))
            {
                File.Delete(_savePath);
                Debug.Log("[SaveService] Kayıt dosyası silindi.");
            }
        }
        
        /// <summary>
        /// Kayıt dosyası var mı?
        /// </summary>
        public bool HasSave()
        {
            return File.Exists(_savePath);
        }
        
        // ===============================
        // Serialization Helpers
        // (JsonUtility Dictionary desteklemiyor, ListWrapper kullanıyoruz)
        // ===============================
        
        private SaveModelSerializable ConvertToSerializable(SaveModel data)
        {
            var s = new SaveModelSerializable();
            s.coins = data.coins;
            s.gems = data.gems;
            s.noAdsPurchased = data.noAdsPurchased;
            s.soundEnabled = data.soundEnabled;
            s.musicEnabled = data.musicEnabled;
            s.activeContractId = data.activeContractId;
            s.deliveredContractCount = data.deliveredContractCount;
            s.saveVersion = data.saveVersion;
            s.machines = data.machines;
            
            // Dictionary → List dönüşümü
            s.inventoryKeys = new List<string>();
            s.inventoryValues = new List<int>();
            if (data.inventory != null)
            {
                foreach (var kvp in data.inventory)
                {
                    s.inventoryKeys.Add(kvp.Key);
                    s.inventoryValues.Add(kvp.Value);
                }
            }
            
            s.boosterKeys = new List<string>();
            s.boosterValues = new List<int>();
            if (data.boosters != null)
            {
                foreach (var kvp in data.boosters)
                {
                    s.boosterKeys.Add(kvp.Key);
                    s.boosterValues.Add(kvp.Value);
                }
            }
            
            return s;
        }
        
        private SaveModel ConvertFromSerializable(SaveModelSerializable s)
        {
            var data = new SaveModel();
            data.coins = s.coins;
            data.gems = s.gems;
            data.noAdsPurchased = s.noAdsPurchased;
            data.soundEnabled = s.soundEnabled;
            data.musicEnabled = s.musicEnabled;
            data.activeContractId = s.activeContractId;
            data.deliveredContractCount = s.deliveredContractCount;
            data.saveVersion = s.saveVersion;
            data.machines = s.machines;
            
            // List → Dictionary dönüşümü
            data.inventory = new Dictionary<string, int>();
            if (s.inventoryKeys != null)
            {
                for (int i = 0; i < s.inventoryKeys.Count; i++)
                {
                    data.inventory[s.inventoryKeys[i]] = s.inventoryValues[i];
                }
            }
            
            data.boosters = new Dictionary<string, int>();
            if (s.boosterKeys != null)
            {
                for (int i = 0; i < s.boosterKeys.Count; i++)
                {
                    data.boosters[s.boosterKeys[i]] = s.boosterValues[i];
                }
            }
            
            return data;
        }
    }
    
    /// <summary>
    /// JsonUtility uyumlu serialize modeli (Dictionary yerine List kullanır)
    /// </summary>
    [System.Serializable]
    public class SaveModelSerializable
    {
        public int coins;
        public int gems;
        public bool noAdsPurchased;
        public bool soundEnabled;
        public bool musicEnabled;
        public string activeContractId;
        public int deliveredContractCount;
        public int saveVersion;
        public List<MachineSaveData> machines = new List<MachineSaveData>();
        
        // Inventory (key-value ayrı listeler)
        public List<string> inventoryKeys = new List<string>();
        public List<int> inventoryValues = new List<int>();
        
        // Boosters
        public List<string> boosterKeys = new List<string>();
        public List<int> boosterValues = new List<int>();
    }

}
