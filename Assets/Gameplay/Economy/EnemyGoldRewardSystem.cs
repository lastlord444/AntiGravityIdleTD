using UnityEngine;
using AntiGravityTD.Gameplay.Enemies;
using AntiGravityTD.Core;

namespace AntiGravityTD.Gameplay.Economy
{
    /// <summary>
    /// Sahne seviyesinde çalışan ve düşman ölümlerini dinleyerek
    /// oyuncunun GoldWallet cüzdanına altın ödülü ekleyen sistem bileşeni.
    /// Sahnede tek bir yönetici GameObject'e eklenmelidir.
    /// </summary>
    public class EnemyGoldRewardSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            EnemyHealth.OnAnyEnemyDefeated += HandleEnemyDefeated;
        }

        private void OnDisable()
        {
            EnemyHealth.OnAnyEnemyDefeated -= HandleEnemyDefeated;
        }

        private void HandleEnemyDefeated(EnemyHealth defeatedEnemy)
        {
            if (defeatedEnemy == null) return;

            // ServiceLocator ve GoldWallet kontrolü
            if (ServiceLocator.Instance == null || !ServiceLocator.Instance.IsRegistered<GoldWallet>())
            {
                // Sessizce no-op kalır
                return;
            }

            var wallet = ServiceLocator.Instance.Get<GoldWallet>();
            if (wallet != null)
            {
                wallet.AddGold(defeatedEnemy.GoldReward);
            }
        }
    }
}
