using System;
using UnityEngine;
using AntiGravityTD.Core;

namespace AntiGravityTD.Gameplay.Economy
{
    /// <summary>
    /// Oyuncunun altın ekonomisini ve cüzdan durumunu yöneten bileşen/servis.
    /// </summary>
    public class GoldWallet : MonoBehaviour
    {
        [Header("Ekonomi Ayarları")]
        [SerializeField] private int startingGold = 100;

        [Header("Event Kanalları (Opsiyonel)")]
        [SerializeField] private GoldChangedEventChannel goldChangedChannel;

        private int currentGold;

        /// <summary>
        /// Cüzdandaki anlık altın miktarı.
        /// </summary>
        public int CurrentGold => currentGold;

        private void Awake()
        {
            currentGold = startingGold;

            // ServiceLocator kaydı
            if (ServiceLocator.Instance != null)
            {
                ServiceLocator.Instance.Register<GoldWallet>(this);
            }
        }

        private void Start()
        {
            // Başlangıç altın bilgisini yayınla
            RaiseGoldChanged();
        }

        /// <summary>
        /// Cüzdana altın ekler. Negatif değerleri güvenle yoksayar.
        /// </summary>
        public void AddGold(int amount)
        {
            if (amount <= 0) return;

            currentGold += amount;
            Debug.Log($"[GoldWallet] Altın eklendi: {amount}. Yeni Altın: {currentGold}");

            RaiseGoldChanged();
        }

        /// <summary>
        /// Belirtilen miktarda altını harcamaya çalışır.
        /// Altın yetersizse veya geçersiz miktar girildiyse işlemi reddeder.
        /// </summary>
        /// <param name="amount">Harcancak miktar.</param>
        /// <returns>Harcama başarılı ise true, değilse false.</returns>
        public bool TrySpend(int amount)
        {
            if (amount <= 0) return false;

            if (currentGold < amount)
            {
                Debug.LogWarning($"[GoldWallet] Altın yetersiz! Gereken: {amount}, Mevcut: {currentGold}");
                return false;
            }

            currentGold -= amount;
            Debug.Log($"[GoldWallet] Altın harcandı: {amount}. Kalan Altın: {currentGold}");

            RaiseGoldChanged();
            return true;
        }

        private void RaiseGoldChanged()
        {
            if (goldChangedChannel != null)
            {
                goldChangedChannel.Raise(currentGold);
            }
        }
    }
}
