using UnityEngine;
using UnityEngine.SceneManagement;
using BlockForge.Save;
using BlockForge.Analytics;
using BlockForge.Monetization;
using BlockForge.AudioVfx;
using BlockForge.Meta;

namespace BlockForge.Core
{
    /// <summary>
    /// Oyun başlangıç noktası. Boot sahnesinde çalışır.
    /// Service'leri başlatır, save yükler, Home sahnesine geçer.
    /// </summary>
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("Başlangıç Ayarları")]
        [SerializeField] private string _homeSceneName = "Home";
        
        [Header("Referanslar (Opsiyonel)")]
        [SerializeField] private SfxLibrarySO _sfxLibrary;
        
        private void Awake()
        {
            // Singleton davranışı - sahne geçişlerinde yok olma
            DontDestroyOnLoad(gameObject);
            
            InitializeServices();
            LoadSavedData();
            GoToHome();
        }
        
        /// <summary>
        /// Tüm servisleri başlatır ve kaydeder.
        /// </summary>
        private void InitializeServices()
        {
            Debug.Log("[GameBootstrapper] Servisler başlatılıyor...");
            
            // Save Service
            var saveService = new SaveService();
            Services.Register<SaveService>(saveService);
            
            // Analytics Service
            var analyticsService = new AnalyticsService();
            Services.Register<AnalyticsService>(analyticsService);
            analyticsService.LogEvent("session_start");
            
            // Ads Service
            var adsService = new AdsService();
            Services.Register<AdsService>(adsService);
            
            // IAP Service
            var iapService = new IapService();
            Services.Register<IapService>(iapService);
            
            // Audio Service
            var audioService = gameObject.AddComponent<AudioService>();
            
            // Auto-load SfxLibrary if missing
            if (_sfxLibrary == null)
            {
                _sfxLibrary = Resources.Load<SfxLibrarySO>("Audio/SfxLibrary");
                if (_sfxLibrary == null) Debug.LogWarning("[GameBootstrapper] SfxLibrary Resources/Audio/SfxLibrary altında bulunamadı!");
            }
            
            if (_sfxLibrary != null) audioService.Initialize(_sfxLibrary);
            Services.Register<AudioService>(audioService);
            
            // VFX Manager
            var vfxManager = gameObject.AddComponent<VFXManager>();
            Services.Register<VFXManager>(vfxManager);
            vfxManager.Initialize();
            
            // Inventory Model (global, sahneler arası)
            var inventoryModel = new InventoryModel();
            Services.Register<InventoryModel>(inventoryModel);
            
            // Production System
            var productionSystem = new ProductionSystem(inventoryModel);
            Services.Register<ProductionSystem>(productionSystem);
            
            // Contract System
            var contractSystem = new ContractSystem(new System.Collections.Generic.List<ContractSO>(), inventoryModel);
            Services.Register<ContractSystem>(contractSystem);
            
            // Mock Data (MVP Hook)
            CreateMockData(productionSystem, contractSystem);
            
            Debug.Log("[GameBootstrapper] Tüm servisler hazır.");
        }
        
        /// <summary>
        /// Kayıtlı veriyi yükler.
        /// </summary>
        private void LoadSavedData()
        {
            Debug.Log("[GameBootstrapper] Kayıtlı veri yükleniyor...");
            var saveService = Services.Get<SaveService>();
            var saveData = saveService.Load();
            
            if (saveData != null)
            {
                var inventory = Services.Get<InventoryModel>();
                inventory.LoadFromSave(saveData);
                Debug.Log($"[GameBootstrapper] Kayıt yüklendi. Coins: {saveData.coins}");
                
                // Load Machines
                var production = Services.Get<ProductionSystem>();
                if (production != null && saveData.machines != null)
                {
                    production.LoadFromSave(saveData.machines);
                }
            }
            else
            {
                Debug.Log("[GameBootstrapper] Yeni oyun başlatılıyor.");
            }
        }
        
        /// <summary>
        /// Home sahnesine geçiş yapar.
        /// </summary>
        private void GoToHome()
        {
            SceneLoader.LoadScene(_homeSceneName);
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                AutoSave();
            }
        }
        
        private void OnApplicationQuit()
        {
            AutoSave();
        }
        
        /// <summary>
        /// Otomatik kaydetme
        /// </summary>
        private void AutoSave()
        {
            if (!Services.Has<SaveService>()) return;
            
            var saveService = Services.Get<SaveService>();
            var inventory = Services.Get<InventoryModel>();
            var production = Services.Get<ProductionSystem>();
            
            if (inventory != null)
            {
                var saveData = inventory.ToSaveData();
                
                // Add Machine Data
                if (production != null)
                {
                    saveData.machines = new System.Collections.Generic.List<MachineSaveData>();
                    foreach (var m in production.GetAllMachines())
                    {
                        saveData.machines.Add(m.ToSaveData());
                    }
                }
                
                saveService.Save(saveData);
                Debug.Log("[GameBootstrapper] Otomatik kayıt yapıldı.");
            }
        }
        
        private void CreateMockData(ProductionSystem productionSystem, ContractSystem contractSystem)
        {
            // Civata Eşyası
            var boltItem = ScriptableObject.CreateInstance<ItemSO>();
            boltItem.Init("bolt", "Civata", Rarity.Common, 10, true);
            
            // Makine Seviyesi (1 Enerji -> 2 Civata)
            var level0 = new MachineLevel
            {
                level = 0,
                triggerCostEnergy = 1,
                outputs = new System.Collections.Generic.List<ItemAmount> 
                { 
                    new ItemAmount(boltItem, 2) 
                },
                upgradeCostCoins = 100,
                critChance = 0.1f
            };
            
            // Hidrolik Pres Makinesi
            var pressMachine = ScriptableObject.CreateInstance<MachineSO>();
            pressMachine.Init("press", "Hidrolik Pres", new System.Collections.Generic.List<MachineLevel> { level0 });
            
            // Model Oluştur ve Ekle
            var pressModel = new MachineModel(pressMachine);
            productionSystem.AddMachine(pressModel);
            
            // Mock Kontrat
            var mockContract = ScriptableObject.CreateInstance<ContractSO>();
            var reqs = new System.Collections.Generic.List<ItemAmount> { new ItemAmount(boltItem, 10) };
            var reward = new Reward { coins = 50 };
            mockContract.Init("contract_1", "İlk Sipariş", "10 Civata Teslim Et", reqs, reward);
            
            contractSystem.RegisterContract(mockContract);
            contractSystem.SelectNewContract();
            
            Debug.Log("[GameBootstrapper] Mock Veri (Hidrolik Pres & Civata & Kontrat) oluşturuldu.");
        }
    }
}
