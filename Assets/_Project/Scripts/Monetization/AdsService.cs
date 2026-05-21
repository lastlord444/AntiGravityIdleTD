using UnityEngine;
using System;

namespace BlockForge.Monetization
{
    /// <summary>
    /// Reklam servisi. Rewarded ve Interstitial reklam yönetimi.
    /// MVP: Unity Ads / LevelPlay placeholder. Gerçek SDK entegrasyonu sonra yapılır.
    /// </summary>
    public class AdsService
    {
        private bool _isInitialized;
        private bool _noAdsPurchased;
        
        // Simülasyon flag'i (test için)
        private bool _simulateRewardedReady = true;
        
        public AdsService()
        {
            Initialize();
        }
        
        /// <summary>
        /// Reklam SDK'sını başlatır.
        /// </summary>
        private void Initialize()
        {
            // MVP: SDK başlatma placeholder
            // Gerçek implementasyon için Unity Ads veya LevelPlay entegrasyonu gerekir
            _isInitialized = true;
            _noAdsPurchased = false;
            Debug.Log("[AdsService] Başlatıldı (MVP placeholder).");
        }
        
        /// <summary>
        /// Rewarded reklam hazır mı?
        /// </summary>
        public bool IsRewardedReady()
        {
            if (_noAdsPurchased) return false;
            
            // MVP: Her zaman hazır (test için)
            return _simulateRewardedReady;
        }
        
        /// <summary>
        /// Rewarded reklam gösterir.
        /// </summary>
        public async void ShowRewarded(string placementId, Action onReward, Action onFail)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("[AdsService] Henüz başlatılmamış!");
                onFail?.Invoke();
                return;
            }
            
            if (_noAdsPurchased)
            {
                // NoAds satın alınmış, direkt ödül ver
                Debug.Log("[AdsService] NoAds aktif, reklamsız ödül veriliyor.");
                onReward?.Invoke();
                return;
            }
            
            Debug.Log($"[AdsService] Rewarded reklam gösteriliyor: {placementId} (1 sn bekleme...)");
            
            // MVP: Reklam simülasyonu - 1 saniye bekle
            await System.Threading.Tasks.Task.Delay(1000);
            
            Debug.Log("[AdsService] Reklam izlendi!");
            onReward?.Invoke();
        }
        
        /// <summary>
        /// Interstitial reklam hazır mı?
        /// </summary>
        public bool IsInterstitialReady()
        {
            if (_noAdsPurchased) return false;
            return _isInitialized;
        }
        
        /// <summary>
        /// Interstitial reklam gösterir.
        /// </summary>
        public void ShowInterstitial(string placementId, Action onClosed = null)
        {
            if (_noAdsPurchased)
            {
                onClosed?.Invoke();
                return;
            }
            
            Debug.Log($"[AdsService] Interstitial gösteriliyor: {placementId}");
            
            // MVP: Placeholder
            onClosed?.Invoke();
        }
        
        /// <summary>
        /// NoAds satın alımını işler.
        /// </summary>
        public void SetNoAdsPurchased(bool purchased)
        {
            _noAdsPurchased = purchased;
            Debug.Log($"[AdsService] NoAds durumu: {purchased}");
        }
        
        public bool NoAdsPurchased => _noAdsPurchased;
    }
}
