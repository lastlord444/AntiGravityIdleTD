using System.Collections.Generic;

namespace BlockForge.Save
{
    /// <summary>
    /// Save veri modeli. JSON serialize edilir.
    /// </summary>
    [System.Serializable]
    public class SaveModel
    {
        // Currencies
        public int coins;
        public int gems;
        
        // Envanter (item_id → count)
        public Dictionary<string, int> inventory = new Dictionary<string, int>();
        
        // Boosters (booster_id → count)
        public Dictionary<string, int> boosters = new Dictionary<string, int>();
        
        // Makineler
        public List<MachineSaveData> machines = new List<MachineSaveData>();
        
        // Aktif kontrat
        public string activeContractId;
        public int deliveredContractCount;
        
        // Ayarlar
        public bool noAdsPurchased;
        public bool soundEnabled = true;
        public bool musicEnabled = true;
        
        // Versiyon (ileride migration için)
        public int saveVersion = 1;
    }
    
    /// <summary>
    /// Makine kayıt verisi
    /// </summary>
    [System.Serializable]
    public class MachineSaveData
    {
        public string machineId;
        public int level;
    }
    
    /// <summary>
    /// Kontrat kayıt verisi
    /// </summary>
    [System.Serializable]
    public class ContractSaveData
    {
        public string contractId;
        public int deliveredCount;
    }
}
