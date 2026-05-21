using UnityEngine;
using System.Collections.Generic;

namespace BlockForge.Meta
{
    /// <summary>
    /// Üretim sistemi. Line clear → Energy → Makineler tetiklenir → Inventory'ye item eklenir.
    /// </summary>
    public class ProductionSystem
    {
        private InventoryModel _inventory;
        private List<MachineModel> _machines = new List<MachineModel>();
        
        // Events
        public System.Action<string, string, int> OnProduction; // machineName, itemName, amount
        
        public ProductionSystem(InventoryModel inventory)
        {
            _inventory = inventory;
        }
        
        /// <summary>
        /// Makine ekler.
        /// </summary>
        public void AddMachine(MachineModel machine)
        {
            if (machine == null) return;
            _machines.Add(machine);
            Debug.Log($"[ProductionSystem] Makine eklendi: {machine.MachineData.Id}");
        }
        
        /// <summary>
        /// Energy geldiğinde tetiklenir. Her makine tetikleme maliyeti kadar energy harcar.
        /// </summary>
        public void ProcessEnergy(int energyAmount)
        {
            if (energyAmount <= 0) return;
            
            int remainingEnergy = energyAmount;
            
            foreach (var machine in _machines)
            {
                if (remainingEnergy <= 0) break;
                
                var levelData = machine.CurrentLevelData;
                int triggerCost = levelData.triggerCostEnergy;
                
                if (triggerCost <= 0) triggerCost = 1; // Fallback
                
                // Bu makine kaç kere tetiklenebilir?
                int triggerCount = remainingEnergy / triggerCost;
                
                for (int i = 0; i < triggerCount; i++)
                {
                    machine.Produce(_inventory);
                    remainingEnergy -= triggerCost;
                    
                    // Event tetikle (UI için)
                    foreach (var output in levelData.outputs)
                    {
                        if (output.item != null)
                        {
                            OnProduction?.Invoke(
                                machine.MachineData.DisplayName,
                                output.item.DisplayName,
                                output.amount
                            );
                            
                            // SFX
                             BlockForge.Core.Services.Get<BlockForge.AudioVfx.AudioService>()?.PlaySfx(BlockForge.AudioVfx.SfxType.ProductionPop);
                        }

                    }
                }
            }
            
            Debug.Log($"[ProductionSystem] {energyAmount} enerji işlendi, kalan: {remainingEnergy}");
        }
        
        /// <summary>
        /// Tüm makineleri döner.
        /// </summary>
        public List<MachineModel> GetAllMachines() => _machines;
        
        /// <summary>
        /// Belirli bir makineyi ID ile bulur.
        /// </summary>
        public MachineModel GetMachine(string machineId)
        {
            return _machines.Find(m => m.MachineData.Id == machineId);
        }

        public void LoadFromSave(List<BlockForge.Save.MachineSaveData> savedMachines)
        {
            if (savedMachines == null) return;

            foreach (var savedData in savedMachines)
            {
                var machine = GetMachine(savedData.machineId);
                if (machine != null)
                {
                    // MachineModel needs a way to set level, or we just recreate it?
                    // Better to update existing model if possible, or re-init.
                    // MachineModel has private _currentLevel. Let's add a public SetLevel or similar?
                    // Or just use reflection/public field for now. 
                    // MVP approach: MachineModel needs a RestoreLevel method.
                    machine.RestoreLevel(savedData.level);
                }
            }
            Debug.Log($"[ProductionSystem] {savedMachines.Count} makine verisi yüklendi.");
        }
    }
}
