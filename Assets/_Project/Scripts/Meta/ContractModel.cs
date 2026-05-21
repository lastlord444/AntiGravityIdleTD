using UnityEngine;
using BlockForge.Save;

namespace BlockForge.Meta
{
    /// <summary>
    /// Kontrat runtime modeli. Aktif kontrat durumu, progress, teslimat sayısı.
    /// </summary>
    public class ContractModel
    {
        private ContractSO _contractData;
        private int _deliveredCount;
        
        public ContractModel(ContractSO contractData)
        {
            _contractData = contractData;
            _deliveredCount = 0;
        }
        
        public ContractSO ContractData => _contractData;
        public int DeliveredCount => _deliveredCount;
        
        /// <summary>
        /// Kontrat ID'si.
        /// </summary>
        public string ContractId => _contractData.Id;
        
        /// <summary>
        /// Kontrat başlığı.
        /// </summary>
        public string Title => _contractData.Title;
        
        /// <summary>
        /// Gereksinimler listesi.
        /// </summary>
        public System.Collections.Generic.List<ItemAmount> Requirements => _contractData.Requirements;
        
        /// <summary>
        /// Ödül.
        /// </summary>
        public Reward Reward => _contractData.Reward;
        
        /// <summary>
        /// Kontrat ilerlemesini hesaplar (0-1 arası).
        /// </summary>
        public float CalculateProgress(InventoryModel inventory)
        {
            if (_contractData.Requirements.Count == 0) return 0f;
            
            float totalProgress = 0f;
            
            foreach (var req in _contractData.Requirements)
            {
                if (req.item == null) continue;
                
                int current = inventory.GetItemCount(req.item.Id);
                int required = req.amount;
                
                float itemProgress = Mathf.Clamp01((float)current / required);
                totalProgress += itemProgress;
            }
            
            return totalProgress / _contractData.Requirements.Count;
        }
        
        /// <summary>
        /// Kontrat teslim edilebilir mi?
        /// </summary>
        public bool CanDeliver(InventoryModel inventory)
        {
            return inventory.HasItems(_contractData.Requirements);
        }
        
        /// <summary>
        /// Kontratı teslim eder.
        /// </summary>
        public bool Deliver(InventoryModel inventory)
        {
            if (!CanDeliver(inventory))
            {
                Debug.LogWarning($"[ContractModel] '{_contractData.Id}' teslim edilemez, gereksinimler karşılanmadı!");
                return false;
            }
            
            // Itemları harca
            inventory.ConsumeItems(_contractData.Requirements);
            
            // Ödül ver
            inventory.AddCoins(_contractData.Reward.coins);
            inventory.AddGems(_contractData.Reward.gems);
            
            foreach (var bonusItem in _contractData.Reward.bonusItems)
            {
                if (bonusItem.item != null)
                {
                    inventory.AddItem(bonusItem.item.Id, bonusItem.amount);
                }
            }
            
            _deliveredCount++;
            
            Debug.Log($"[ContractModel] '{_contractData.Id}' teslim edildi! +{_contractData.Reward.coins} Coin");
            return true;
        }
        
        /// <summary>
        /// Save verisi üretir.
        /// </summary>
        public ContractSaveData ToSaveData()
        {
            return new ContractSaveData
            {
                contractId = _contractData.Id,
                deliveredCount = _deliveredCount
            };
        }
    }
}
