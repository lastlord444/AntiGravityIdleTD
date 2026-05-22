using UnityEngine;

namespace AntiGravityTD.Gameplay.Waves
{
    /// <summary>
    /// Düşman üretim soyutlaması.
    /// WaveController bu arayüz üzerinden düşman üretir.
    /// Implementasyonu sahne bağlantısı PR'ında (PR #9B) sağlanacaktır.
    /// </summary>
    public interface IEnemySpawner
    {
        /// <summary>
        /// Yeni bir düşman üretir ve döndürür.
        /// </summary>
        /// <returns>Üretilen düşman GameObject'i.</returns>
        GameObject SpawnEnemy();
    }
}
