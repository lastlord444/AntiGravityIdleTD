using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using BlockForge.Core;
using BlockForge.Analytics;

namespace BlockForge.UI
{
    /// <summary>
    /// Home (Atölye) ekranı UI kontrolcüsü.
    /// Kontrat kartı, makineler paneli, upgrade, play button.
    /// </summary>
    public class HomeUIController : MonoBehaviour
    {
        [Header("Top Bar")]
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _noAdsButton;
        
        [Header("Kontrat Kartı")]
        [SerializeField] private GameObject _contractPanel;
        [SerializeField] private TextMeshProUGUI _contractTitleText;
        [SerializeField] private TextMeshProUGUI _contractDescText;
        [SerializeField] private Transform _requirementsContainer;
        [SerializeField] private GameObject _requirementItemPrefab;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private Button _deliverButton;
        
        [Header("Makineler Paneli")]
        [SerializeField] private Transform _machinesContainer;
        [SerializeField] private GameObject _machineSlotPrefab;
        
        [Header("Play Button")]
        [SerializeField] private Button _playButton;
        
        [Header("Referanslar")]
        [SerializeField] private List<BlockForge.Meta.ContractSO> _contractLibrary;
        [SerializeField] private List<BlockForge.Meta.MachineSO> _machineLibrary;
        
        // Sistemler
        private BlockForge.Meta.ContractSystem _contractSystem;
        private BlockForge.Meta.ProductionSystem _productionSystem;
        private BlockForge.Meta.InventoryModel _inventory;
        
        private void Start()
        {
            // Servisleri al (Global)
            _inventory = Services.Get<BlockForge.Meta.InventoryModel>();
            _contractSystem = Services.Get<BlockForge.Meta.ContractSystem>();
            _productionSystem = Services.Get<BlockForge.Meta.ProductionSystem>();
            
            // Auto-find NoAdsButton if missing (MANDATORY FIX)
            if (_noAdsButton == null)
            {
                var uiCanvas = GameObject.Find("UICanvas");
                if (uiCanvas != null)
                {
                    // Path: TopBar/NoAdsButton
                    var btnTransform = uiCanvas.transform.Find("TopBar/NoAdsButton");
                    if (btnTransform != null)
                    {
                        _noAdsButton = btnTransform.GetComponent<Button>();
                        Debug.Log("[HomeUI] NoAdsButton otomatik bulundu ve atandı.");
                    }
                    else
                    {
                         // Try searching recursively in children if path failed or layout changed
                        _noAdsButton = uiCanvas.GetComponentInChildren<Button>(); // Risky if multiple buttons, but better than nothing for now
                        // Actually, let's stick to specific search or warning
                        Debug.LogWarning("[HomeUI] NoAdsButton 'UICanvas/TopBar/NoAdsButton' yolunda bulunamadı! Lütfen Inspector'dan atayın.");
                    }
                }
            }

            if (_inventory == null || _contractSystem == null || _productionSystem == null)
            {
                Debug.LogError("[HomeUI] Global sistemler bulunamadı! Boot sahnesinden başladınız mı?");
                return;
            }
            
            // Makineleri yükle (Artık ProductionSystem'den okuyoruz, yaratmıyoruz)
            LoadMachines();
            
            // İlk kontratı seç
            _contractSystem.SelectNewContract();
            
            // Event subscriptions
            SubscribeToEvents();
            
            // UI güncelle
            UpdateUI();
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        
        private void SubscribeToEvents()
        {
            if (_inventory != null)
            {
                _inventory.OnCoinsChanged += UpdateCoins;
            }
            
            if (_contractSystem != null)
            {
                _contractSystem.OnContractChanged += OnContractChanged;
                _contractSystem.OnContractDelivered += OnContractDelivered;
            }
            
            if (_deliverButton != null)
                _deliverButton.onClick.AddListener(OnDeliverClicked);
            
            // Force-find PlayButton (Debug)
            // if (_playButton == null) <--- REMOVED CONDITION
            {
                var go = GameObject.Find("PlayButton"); // Direct name search in scene
                if (go != null) 
                {
                    _playButton = go.GetComponent<Button>();
                    Debug.Log($"[HomeUI] PlayButton bulundu: {go.name} (InstanceID: {go.GetInstanceID()})");
                }
                
                if (_playButton == null)
                {
                    // Fallback: Check inside UICanvas
                    var canvas = GameObject.Find("UICanvas");
                    if (canvas != null)
                    {
                        var btn = canvas.transform.Find("PlayButton")?.GetComponent<Button>(); // Direct child
                        if (btn == null) btn = canvas.transform.Find("TopBar/PlayButton")?.GetComponent<Button>(); // Nested
                        _playButton = btn;
                    }
                }
            }

            if (_playButton != null)
            {
                _playButton.onClick.RemoveAllListeners(); // Clear just in case
                _playButton.onClick.AddListener(OnPlayClicked);
                Debug.Log("[HomeUI] PlayButton listener eklendi.");
            }
            else
                Debug.LogError("[HomeUI] PlayButton BULUNAMADI! Lütfen sahne hiyerarşisini kontrol edin.");
                
             // Force-find NoAdsButton
             // if (_noAdsButton == null) <-- REMOVED CONDITION
            {
                 var go = GameObject.Find("NoAdsButton");
                 if (go != null) _noAdsButton = go.GetComponent<Button>();
                 
                 if (_noAdsButton != null) Debug.Log("[HomeUI] NoAdsButton bulundu.");
            }
            
            if (_noAdsButton != null)
                _noAdsButton.onClick.AddListener(OnNoAdsClicked);
                
            var iapService = Services.Get<BlockForge.Monetization.IapService>();
            if (iapService != null)
            {
                iapService.OnPurchaseSuccess += OnPurchaseSuccess;
            }
            
            // Check button state
            if (_playButton != null)
            {
                Debug.Log($"[HomeUI] PlayButton State - Active: {_playButton.gameObject.activeInHierarchy}, Enabled: {_playButton.enabled}, Interactable: {_playButton.interactable}");
            }
            
            CheckNoAdsStatus();
        }
        
        private void UnsubscribeFromEvents()
        {
            if (_inventory != null)
            {
                _inventory.OnCoinsChanged -= UpdateCoins;
            }
            
            if (_contractSystem != null)
            {
                _contractSystem.OnContractChanged -= OnContractChanged;
                _contractSystem.OnContractDelivered -= OnContractDelivered;
            }
            
            var iapService = Services.Get<BlockForge.Monetization.IapService>();
            if (iapService != null)
            {
                iapService.OnPurchaseSuccess -= OnPurchaseSuccess;
            }
        }
        
        // --- IAP Handlers ---

        private void OnPurchaseSuccess(string productId)
        {
            if (productId == BlockForge.Monetization.IapService.PRODUCT_REMOVE_ADS)
            {
                CheckNoAdsStatus();
            }
        }

        private void CheckNoAdsStatus()
        {
            if (_noAdsButton == null) return;
            
            var adsService = Services.Get<BlockForge.Monetization.AdsService>();
            if (adsService != null && adsService.NoAdsPurchased)
            {
                _noAdsButton.gameObject.SetActive(false);
            }
            else
            {
                _noAdsButton.gameObject.SetActive(true);
            }
        }

        private void OnNoAdsClicked()
        {
            var iapService = Services.Get<BlockForge.Monetization.IapService>();
            iapService?.PurchaseNoAds();
        }

        // --- Existing Methods ---
        
        /// <summary>
        /// Makineleri yükler ve ProductionSystem'e ekler.
        /// </summary>
        private void LoadMachines()
        {
            // Global sistemdeki makineleri çek
            var machines = _productionSystem.GetAllMachines();
            
            foreach (var machineModel in machines)
            {
                // UI slot oluştur
                CreateMachineSlot(machineModel);
            }
        }
        
        /// <summary>
        /// Makine slot UI'ı oluşturur.
        /// </summary>
        private void CreateMachineSlot(BlockForge.Meta.MachineModel machine)
        {
            if (_machineSlotPrefab == null || _machinesContainer == null) return;
            
            GameObject slotObj = Instantiate(_machineSlotPrefab, _machinesContainer);
            
            // Text component'leri güncelle
            TextMeshProUGUI[] texts = slotObj.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length > 0) texts[0].text = machine.MachineData.DisplayName;
            if (texts.Length > 1) texts[1].text = $"Level {machine.CurrentLevel}";
            
            // Upgrade button
            Button upgradeBtn = slotObj.GetComponentInChildren<Button>();
            if (upgradeBtn != null)
            {
                upgradeBtn.onClick.AddListener(() => OnUpgradeClicked(machine));
            }
        }
        
        private void OnUpgradeClicked(BlockForge.Meta.MachineModel machine)
        {
            bool success = machine.Upgrade(_inventory);
            
            if (success)
            {
                // Analytics
                var analytics = Services.Get<AnalyticsService>();
                analytics?.LogEvent("machine_upgraded", "machine_id", machine.MachineData.Id);
                
                Services.Get<BlockForge.AudioVfx.AudioService>()?.PlaySfx(BlockForge.AudioVfx.SfxType.Upgrade);
                
                UpdateUI();
            }
        }
        
        private void OnContractChanged(BlockForge.Meta.ContractModel contract)
        {
            // Kontrat panelini güncelle
            if (_contractTitleText != null)
                _contractTitleText.text = contract.Title;
            
            if (_contractDescText != null)
                _contractDescText.text = contract.ContractData.Description;
            
            // Gereksinimleri güncelle
            UpdateRequirements(contract);
            
            // Progress güncelle
            UpdateProgress(contract);
            
            // Ödülü güncelle
            if (_rewardText != null)
                _rewardText.text = $"+{contract.Reward.coins} Altın";
        }
        
        private void UpdateRequirements(BlockForge.Meta.ContractModel contract)
        {
            // Önceki gereksinim item'larını temizle
            if (_requirementsContainer != null)
            {
                foreach (Transform child in _requirementsContainer)
                {
                    Destroy(child.gameObject);
                }
            }
            
            if (_requirementItemPrefab == null) return;
            
            foreach (var req in contract.Requirements)
            {
                if (req.item == null) continue;
                
                GameObject reqObj = Instantiate(_requirementItemPrefab, _requirementsContainer);
                TextMeshProUGUI reqText = reqObj.GetComponentInChildren<TextMeshProUGUI>();
                
                if (reqText != null)
                {
                    int current = _inventory.GetItemCount(req.item.Id);
                    reqText.text = $"{req.item.DisplayName}: {current}/{req.amount}";
                }
            }
        }
        
        private void UpdateProgress(BlockForge.Meta.ContractModel contract)
        {
            float progress = _contractSystem.GetProgress();
            
            if (_progressSlider != null)
                _progressSlider.value = progress;
            
            if (_progressText != null)
                _progressText.text = $"{Mathf.FloorToInt(progress * 100)}%";
            
            // Deliver button state
            if (_deliverButton != null)
                _deliverButton.interactable = contract.CanDeliver(_inventory);
        }
        
        private void OnDeliverClicked()
        {
            bool success = _contractSystem.DeliverActiveContract();
            
            if (success)
            {
                Debug.Log("[HomeUI] Kontrat teslim edildi!");
                Services.Get<BlockForge.AudioVfx.AudioService>()?.PlaySfx(BlockForge.AudioVfx.SfxType.ContractDeliver);
                UpdateUI();
            }
        }
        
        private void OnContractDelivered()
        {
            // Kontrat teslim edildiğinde yapılacaklar
            Debug.Log("[HomeUI] Kontrat tamamlandı, yenisi seçildi!");
        }
        
        private void OnPlayClicked()
        {
            Debug.Log("[HomeUI] Play clicked! Loading Run scene...");
            Services.Get<BlockForge.AudioVfx.AudioService>()?.PlaySfx(BlockForge.AudioVfx.SfxType.ButtonClick);
            SceneLoader.LoadScene("Run");
        }
        
        private void Update()
        {
            if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("[HomeUI] Mouse Click Detected via InputSystem");
            }
             // For Old Input to compare (will error if disabled, but let's try safely if possible. No, just stick to new input since we confirmed it)
        }
        
        private void UpdateCoins(int coins)
        {
            if (_coinsText != null)
                _coinsText.text = coins.ToString("N0");
        }
        
        private void UpdateUI()
        {
            UpdateCoins(_inventory?.Coins ?? 0);
            
            if (_contractSystem?.ActiveContract != null)
            {
                OnContractChanged(_contractSystem.ActiveContract);
            }
        }
    }
}
