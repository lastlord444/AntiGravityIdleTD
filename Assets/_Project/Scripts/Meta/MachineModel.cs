using UnityEngine;
using BlockForge.Save;

namespace BlockForge.Meta
{
    /// <summary>
    /// Makine runtime modeli. Bir makineye ait runtime state (level, upgrade progress).
    /// </summary>
    public class MachineModel
    {
        private MachineSO _machineData;
        private int _currentLevel;
        
        // Events
        public System.Action<int> OnLevelChanged;
        
        public MachineModel(MachineSO machineData, int startingLevel = 0)
        {
            _machineData = machineData;
            _currentLevel = Mathf.Clamp(startingLevel, 0, machineData.MaxLevel);
        }
        
        public MachineSO MachineData => _machineData;
        public int CurrentLevel => _currentLevel;
        public MachineLevel CurrentLevelData => _machineData.GetLevel(_currentLevel);
        
        /// <summary>
        /// Makineyi bir seviye yükseltir.
        /// </summary>
        public bool Upgrade(InventoryModel inventory)
        {
            if (_currentLevel >= _machineData.MaxLevel)
            {
                Debug.LogWarning($"[MachineModel] '{_machineData.Id}' maksimum seviyede!");
                return false;
            }
            
            var currentData = CurrentLevelData;
            int upgradeCost = currentData.upgradeCostCoins;
            
            if (!inventory.SpendCoins(upgradeCost))
            {
                Debug.LogWarning($"[MachineModel] '{_machineData.Id}' yükseltmesi için yetersiz coin! Gerekli: {upgradeCost}");
                return false;
            }
            
            _currentLevel++;
            OnLevelChanged?.Invoke(_currentLevel);
            
            Debug.Log($"[MachineModel] '{_machineData.Id}' Level {_currentLevel}'e yükseltildi!");
            Debug.Log($"[MachineModel] '{_machineData.Id}' Level {_currentLevel}'e yükseltildi!");
            return true;
        }

        public void RestoreLevel(int level)
        {
            _currentLevel = Mathf.Clamp(level, 0, _machineData.MaxLevel);
            OnLevelChanged?.Invoke(_currentLevel);
        }
        
        /// <summary>
        /// Makineyi tetikler, ürün üretir (ProductionSystem tarafından çağrılır).
        /// </summary>
        public void Produce(InventoryModel inventory)
        {
            var levelData = CurrentLevelData;
            
            // Kritik şans kontrolü (MVP'de düşük/0)
            bool isCrit = Random.value < levelData.critChance;
            int multiplier = isCrit ? 2 : 1;
            
            // Üretim
            foreach (var output in levelData.outputs)
            {
                if (output.item == null) continue;
                int amount = output.amount * multiplier;
                inventory.AddItem(output.item.Id, amount);
                
                Debug.Log($"[MachineModel] '{_machineData.Id}' üretti: +{amount} {output.item.DisplayName}{(isCrit ? " (CRIT!)" : "")}");
            }
        }
        
        /// <summary>
        /// Save verisi üretir.
        /// </summary>
        public MachineSaveData ToSaveData()
        {
            return new MachineSaveData
            {
                machineId = _machineData.Id,
                level = _currentLevel
            };
        }
    }
}
