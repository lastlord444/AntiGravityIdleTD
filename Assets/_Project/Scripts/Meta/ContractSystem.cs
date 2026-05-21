using UnityEngine;
using System.Collections.Generic;
using BlockForge.Core;
using BlockForge.Analytics;

namespace BlockForge.Meta
{
    /// <summary>
    /// Kontrat sistemi. Kontrat seçimi, yenisiyle değiştirme, teslimat yönetimi.
    /// </summary>
    public class ContractSystem
    {
        private List<ContractSO> _availableContracts;
        private ContractModel _activeContract;
        private InventoryModel _inventory;
        
        // Events
        public System.Action<ContractModel> OnContractChanged;
        public System.Action OnContractDelivered;
        
        public ContractSystem(List<ContractSO> contractLibrary, InventoryModel inventory)
        {
            _availableContracts = new List<ContractSO>(contractLibrary);
            _inventory = inventory;
        }

        public void RegisterContract(ContractSO contract)
        {
            if (contract != null && !_availableContracts.Contains(contract))
            {
                _availableContracts.Add(contract);
            }
        }
        
        /// <summary>
        /// Aktif kontrat.
        /// </summary>
        public ContractModel ActiveContract => _activeContract;
        
        /// <summary>
        /// Yeni bir kontrat seçer ve aktif yapar.
        /// </summary>
        public void SelectNewContract()
        {
            if (_availableContracts.Count == 0)
            {
                Debug.LogWarning("[ContractSystem] Kontrat library'si boş!");
                return;
            }
            
            // Rastgele seç (MVP - basit)
            ContractSO selectedContract = _availableContracts[Random.Range(0, _availableContracts.Count)];
            _activeContract = new ContractModel(selectedContract);
            
            Debug.Log($"[ContractSystem] Yeni kontrat seçildi: {selectedContract.Title}");
            OnContractChanged?.Invoke(_activeContract);
            
            // Analytics
            var analytics = Services.Get<AnalyticsService>();
            analytics?.LogEvent("contract_selected", "contract_id", selectedContract.Id);
        }
        
        /// <summary>
        /// Belirli bir kontratı ID ile bulur ve aktif yapar.
        /// </summary>
        public void LoadContract(string contractId)
        {
            ContractSO contractData = _availableContracts.Find(c => c.Id == contractId);
            if (contractData == null)
            {
                Debug.LogWarning($"[ContractSystem] Kontrat bulunamadı: {contractId}");
                return;
            }
            
            _activeContract = new ContractModel(contractData);
            OnContractChanged?.Invoke(_activeContract);
        }
        
        /// <summary>
        /// Aktif kontratı teslim eder.
        /// </summary>
        public bool DeliverActiveContract()
        {
            if (_activeContract == null)
            {
                Debug.LogWarning("[ContractSystem] Aktif kontrat yok!");
                return false;
            }
            
            if (!_activeContract.CanDeliver(_inventory))
            {
                Debug.LogWarning("[ContractSystem] Kontrat teslim edilemez, gereksinimler karşılanmadı!");
                return false;
            }
            
            string contractId = _activeContract.ContractId;
            bool success = _activeContract.Deliver(_inventory);
            
            if (success)
            {
                // Analytics
                var analytics = Services.Get<AnalyticsService>();
                analytics?.LogEvent("contract_delivered", "contract_id", contractId);
                
                OnContractDelivered?.Invoke();
                
                // Otomatik yeni kontrat seç
                SelectNewContract();
            }
            
            return success;
        }
        
        /// <summary>
        /// Kontrat progress hesaplar (0-1 arası).
        /// </summary>
        public float GetProgress()
        {
            if (_activeContract == null) return 0f;
            return _activeContract.CalculateProgress(_inventory);
        }
        
        /// <summary>
        /// Belirli bir gereksinim için ilerleme durumunu döner.
        /// </summary>
        public RequirementProgress GetRequirementProgress(int index)
        {
            if (_activeContract == null || index < 0 || index >= _activeContract.Requirements.Count)
            {
                return new RequirementProgress { current = 0, required = 0, isMet = false };
            }
            
            var req = _activeContract.Requirements[index];
            if (req.item == null)
            {
                return new RequirementProgress { current = 0, required = 0, isMet = false };
            }
            
            int current = _inventory.GetItemCount(req.item.Id);
            int required = req.amount;
            
            return new RequirementProgress
            {
                current = current,
                required = required,
                isMet = current >= required
            };
        }
    }
    
    /// <summary>
    /// Gereksinim ilerleme durumu.
    /// </summary>
    public struct RequirementProgress
    {
        public int current;
        public int required;
        public bool isMet;
    }
}
