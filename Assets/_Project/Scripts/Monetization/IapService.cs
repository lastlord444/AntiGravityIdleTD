using UnityEngine;
using System;

namespace BlockForge.Monetization
{
    /// <summary>
    /// IAP (In-App Purchase) servisi.
    /// MVP: Unity IAP placeholder. Remove Ads + Starter Pack.
    /// </summary>
    public class IapService
    {
        // Ürün ID'leri
        public const string PRODUCT_REMOVE_ADS = "remove_ads";
        public const string PRODUCT_STARTER_PACK = "starter_pack";
        
        private bool _isInitialized;
        
        // Events
        public Action<string> OnPurchaseSuccess;
        public Action<string, string> OnPurchaseFailed; // productId, error
        
        public IapService()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            // MVP: Placeholder
            // Gerçek implementasyon Unity IAP package gerektirir
            _isInitialized = true;
            Debug.Log("[IapService] Başlatıldı (MVP placeholder).");
        }
        
        /// <summary>
        /// Satın alma başlatır.
        /// </summary>
        public void Purchase(string productId)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("[IapService] Henüz başlatılmamış!");
                OnPurchaseFailed?.Invoke(productId, "Not initialized");
                return;
            }
            
            Debug.Log($"[IapService] Satın alma başlatılıyor: {productId}");
            
            // MVP: Simülasyon - başarılı
            ProcessPurchase(productId);
        }
        
        /// <summary>
        /// Satın alma işlemini gerçekleştirir.
        /// </summary>
        private void ProcessPurchase(string productId)
        {
            switch (productId)
            {
                case PRODUCT_REMOVE_ADS:
                    HandleRemoveAds();
                    break;
                    
                case PRODUCT_STARTER_PACK:
                    HandleStarterPack();
                    break;
                    
                default:
                    Debug.LogWarning($"[IapService] Bilinmeyen ürün: {productId}");
                    OnPurchaseFailed?.Invoke(productId, "Unknown product");
                    return;
            }
            
            // Analytics
            var analytics = BlockForge.Core.Services.Get<BlockForge.Analytics.AnalyticsService>();
            analytics?.LogEvent("iap_purchase", "product_id", productId);
            
            OnPurchaseSuccess?.Invoke(productId);
        }

        /// <summary>
        /// "Reklamsız" özelliğini satın alır.
        /// </summary>
        public void PurchaseNoAds()
        {
            Purchase(PRODUCT_REMOVE_ADS);
        }

        /// <summary>
        /// "Starter Pack" satın alır.
        /// </summary>
        public void PurchaseStarterPack()
        {
            Purchase(PRODUCT_STARTER_PACK);
        }
        
        private void HandleRemoveAds()
        {
            var adsService = BlockForge.Core.Services.Get<AdsService>();
            if (adsService != null)
            {
                adsService.SetNoAdsPurchased(true);
            }
            
            // Save'e yaz
            var saveService = BlockForge.Core.Services.Get<BlockForge.Save.SaveService>();
            if (saveService != null)
            {
                var data = saveService.Load();
                if (data == null) data = new BlockForge.Save.SaveModel();
                data.noAdsPurchased = true;
                saveService.Save(data);
            }
            
            Debug.Log("[IapService] Remove Ads satın alındı!");
        }
        
        private void HandleStarterPack()
        {
            var inventory = BlockForge.Core.Services.Get<BlockForge.Meta.InventoryModel>();
            if (inventory != null)
            {
                inventory.AddCoins(500);
                inventory.AddGems(50);
                Debug.Log("[IapService] Starter Pack verildi: +500 Coin, +50 Gem.");
            }
        }
        
        /// <summary>
        /// Restore purchases (iOS gereksinimi).
        /// </summary>
        public void RestorePurchases()
        {
            Debug.Log("[IapService] Satın almalar geri yükleniyor (placeholder).");
            // MVP: Placeholder
        }
    }
}
