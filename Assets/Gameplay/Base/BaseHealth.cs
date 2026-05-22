using System;
using UnityEngine;
using AntiGravityTD.Core;

namespace AntiGravityTD.Gameplay.Base
{
    /// <summary>
    /// Base can değerlerini yöneten ve hasar alma mantığını uygulayan bileşen/servis.
    /// Canı sıfıra ulaştığında base'in yok edildiğini ve oyunun kaybedildiğini bildirir.
    /// </summary>
    public class BaseHealth : MonoBehaviour
    {
        /// <summary>
        /// Herhangi bir base yok edildiğinde tetiklenen statik event.
        /// GameLoopController tarafından lose koşulu olarak dinlenir.
        /// </summary>
        public static event Action<BaseHealth> OnAnyBaseDestroyed;

        [Header("Can Ayarları")]
        [SerializeField] private int maxHealth = 100;
        
        [Header("Event Kanalları (Opsiyonel)")]
        [SerializeField] private BaseHealthChangedEventChannel healthChangedChannel;
        [SerializeField] private BaseDestroyedEventChannel baseDestroyedChannel;

        private int currentHealth;

        /// <summary>
        /// Base'in anlık can değeri.
        /// </summary>
        public int CurrentHealth => currentHealth;

        /// <summary>
        /// Base'in maksimum can değeri.
        /// </summary>
        public int MaxHealth => maxHealth;

        /// <summary>
        /// Base'in imha edilip edilmediği.
        /// </summary>
        public bool IsDestroyed => currentHealth <= 0;

        private void Awake()
        {
            currentHealth = maxHealth;
            
            // ServiceLocator kaydı
            if (ServiceLocator.Instance != null)
            {
                ServiceLocator.Instance.Register<BaseHealth>(this);
            }
        }

        private void Start()
        {
            // Başlangıç can bilgisini yayınla
            RaiseHealthChanged();
        }

        /// <summary>
        /// Base'e hasar uygular. Canı 0 ile max arasında sınırlar.
        /// </summary>
        public void ApplyDamage(int amount)
        {
            if (amount <= 0 || IsDestroyed) return;

            currentHealth = Mathf.Max(0, currentHealth - amount);
            Debug.Log($"[BaseHealth] Base hasar aldı: {amount}. Kalan Can: {currentHealth}/{maxHealth}");

            RaiseHealthChanged();

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Base'i iyileştirir. Canı maxHealth sınırını aşamaz.
        /// </summary>
        public void Heal(int amount)
        {
            if (amount <= 0 || IsDestroyed) return;

            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            Debug.Log($"[BaseHealth] Base iyileşti: {amount}. Mevcut Can: {currentHealth}/{maxHealth}");

            RaiseHealthChanged();
        }

        private void RaiseHealthChanged()
        {
            if (healthChangedChannel != null)
            {
                healthChangedChannel.Raise(new BaseHealthChangedPayload
                {
                    CurrentHealth = currentHealth,
                    MaxHealth = maxHealth
                });
            }
        }

        private void Die()
        {
            Debug.Log("[BaseHealth] Base yok edildi!");
            
            if (baseDestroyedChannel != null)
            {
                baseDestroyedChannel.Raise();
            }

            OnAnyBaseDestroyed?.Invoke(this);
        }
    }
}
