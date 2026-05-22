using System;
using UnityEngine;

namespace AntiGravityTD.Gameplay.Enemies
{
    /// <summary>
    /// Düşman nesnelerinin can değerlerini ve hasar alma mantığını yönetir.
    /// Canı sıfıra ulaştığında düşman nesnesini imha eder.
    /// </summary>
    public class EnemyHealth : MonoBehaviour
    {
        /// <summary>
        /// Herhangi bir düşman yok edildiğinde tetiklenir.
        /// WaveController ve gelecekteki ekonomi sistemi tarafından dinlenir.
        /// </summary>
        public static event Action<EnemyHealth> OnAnyEnemyDefeated;

        [SerializeField] private float maxHealth = 10.0f;
        [SerializeField] private int goldReward = 10;
        
        private float currentHealth;

        /// <summary>
        /// Düşman yenildiğinde oyuncunun kazanacağı altın miktarı.
        /// </summary>
        public int GoldReward => goldReward;

        /// <summary>
        /// Düşmanın hayatta olup olmadığını belirtir.
        /// </summary>
        public bool IsAlive => currentHealth > 0.0f;

        /// <summary>
        /// Düşmanın anlık can değerini döner.
        /// </summary>
        public float CurrentHealth => currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        /// <summary>
        /// Düşmana belirtilen miktarda hasar uygular.
        /// </summary>
        /// <param name="amount">Hasar miktarı.</param>
        public void TakeDamage(float amount)
        {
            if (amount <= 0.0f || !IsAlive) return;

            currentHealth -= amount;

            if (currentHealth <= 0.0f)
            {
                Die();
            }
        }

        /// <summary>
        /// Düşman öldüğünde tetiklenir ve nesneyi imha eder.
        /// </summary>
        private void Die()
        {
            Debug.Log($"[EnemyHealth] Enemy defeated: {gameObject.name}");
            OnAnyEnemyDefeated?.Invoke(this);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
