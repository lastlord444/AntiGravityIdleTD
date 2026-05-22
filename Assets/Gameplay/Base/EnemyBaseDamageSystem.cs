using UnityEngine;
using AntiGravityTD.Gameplay.Enemies;
using AntiGravityTD.Core;

namespace AntiGravityTD.Gameplay.Base
{
    /// <summary>
    /// Sahne seviyesinde çalışan ve düşmanların yolun sonuna (base'e) ulaşmasını dinleyerek
    /// BaseHealth bileşenine hasar uygulayan sistem bileşeni.
    /// Sahnede tek bir yönetici GameObject'e eklenmelidir.
    /// </summary>
    public class EnemyBaseDamageSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            EnemyMover.OnAnyEnemyReachedEnd += HandleEnemyReachedEnd;
        }

        private void OnDisable()
        {
            EnemyMover.OnAnyEnemyReachedEnd -= HandleEnemyReachedEnd;
        }

        private void HandleEnemyReachedEnd(EnemyMover reachedEnemy)
        {
            if (reachedEnemy == null) return;

            // ServiceLocator ve BaseHealth kontrolü
            if (ServiceLocator.Instance == null || !ServiceLocator.Instance.IsRegistered<BaseHealth>())
            {
                // Sessizce no-op kalır
                return;
            }

            var baseHealth = ServiceLocator.Instance.Get<BaseHealth>();
            if (baseHealth != null)
            {
                baseHealth.ApplyDamage(reachedEnemy.BaseDamage);
            }
        }
    }
}
