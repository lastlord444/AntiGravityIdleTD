using UnityEngine;

namespace AntiGravityTD.Gameplay.Waves
{
    /// <summary>
    /// Tek bir dalganın tanım verilerini içerir.
    /// WaveController tarafından serialized array olarak kullanılır.
    /// </summary>
    [System.Serializable]
    public class WaveDefinition
    {
        [Tooltip("Bu dalgada üretilecek düşman sayısı.")]
        [Min(1)]
        public int enemyCount = 3;

        [Tooltip("Düşman üretimleri arasındaki bekleme süresi (saniye).")]
        [Min(0.1f)]
        public float spawnInterval = 1.0f;

        [Tooltip("Dalga başlamadan önceki bekleme süresi (saniye).")]
        [Min(0f)]
        public float preWaveDelay = 2.0f;
    }
}
