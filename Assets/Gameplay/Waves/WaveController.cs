using System;
using System.Collections;
using UnityEngine;
using AntiGravityTD.Gameplay.Enemies;

namespace AntiGravityTD.Gameplay.Waves
{
    /// <summary>
    /// Dalga akış kontrolcüsü.
    /// WaveDefinition dizisine göre düşmanları sırayla üretir,
    /// dalga geçişlerini yönetir ve tamamlanma olaylarını bildirir.
    /// </summary>
    public class WaveController : MonoBehaviour
    {
        [Header("Dalga Tanımları")]
        [SerializeField] private WaveDefinition[] waves;

        [Header("Düşman Üretici")]
        [Tooltip("IEnemySpawner arayüzünü uygulayan MonoBehaviour bileşeni.")]
        [SerializeField] private MonoBehaviour enemySpawnerRef;

        private IEnemySpawner enemySpawner;
        private int currentWaveIndex = -1;
        private bool isSpawning;
        private Coroutine waveCoroutine;

        /// <summary>Mevcut dalga indeksi (0-tabanlı). Başlamadıysa -1.</summary>
        public int CurrentWaveIndex => currentWaveIndex;

        /// <summary>Toplam dalga sayısı.</summary>
        public int TotalWaveCount => waves != null ? waves.Length : 0;

        /// <summary>Şu anda düşman üretimi yapılıp yapılmadığı.</summary>
        public bool IsSpawning => isSpawning;

        /// <summary>Bir dalga başladığında tetiklenir. Parametre: dalga indeksi.</summary>
        public event Action<int> OnWaveStarted;

        /// <summary>Bir dalga tamamlandığında tetiklenir. Parametre: dalga indeksi.</summary>
        public event Action<int> OnWaveCompleted;

        /// <summary>Tüm dalgalar tamamlandığında tetiklenir.</summary>
        public event Action OnAllWavesCompleted;

        private void Awake()
        {
            ValidateSpawner();
        }

        /// <summary>
        /// Dalga dizisini başlatır. İlk dalgadan itibaren sırayla ilerler.
        /// </summary>
        public void StartWaves()
        {
            if (waveCoroutine != null)
            {
                Debug.LogWarning("[WaveController] StartWaves() çağrıldı fakat zaten çalışan bir dalga coroutine'i var!");
                return;
            }

            if (waves == null || waves.Length == 0)
            {
                Debug.LogError("[WaveController] Dalga tanımları boş! Waves dizisi atanmamış.");
                return;
            }

            if (!ValidateSpawner())
            {
                Debug.LogError("[WaveController] IEnemySpawner geçerli değil! Dalgalar başlatılamıyor.");
                return;
            }

            currentWaveIndex = 0;
            waveCoroutine = StartCoroutine(RunWaves());
        }

        /// <summary>
        /// Aktif dalga üretimini durdurur.
        /// </summary>
        public void StopWaves()
        {
            if (waveCoroutine != null)
            {
                StopCoroutine(waveCoroutine);
                waveCoroutine = null;
            }
            isSpawning = false;
        }

        /// <summary>
        /// Sahnedeki aktif (canlı) düşman sayısını döndürür.
        /// </summary>
        public int GetAliveEnemyCount()
        {
            EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);
            int count = 0;
            foreach (EnemyHealth enemy in enemies)
            {
                if (enemy != null && enemy.IsAlive)
                {
                    count++;
                }
            }
            return count;
        }

        private bool ValidateSpawner()
        {
            if (enemySpawnerRef != null)
            {
                enemySpawner = enemySpawnerRef as IEnemySpawner;
                if (enemySpawner == null)
                {
                    Debug.LogError($"[WaveController] Atanan bileşen ({enemySpawnerRef.GetType().Name}) IEnemySpawner arayüzünü uygulamıyor!");
                    return false;
                }
                return true;
            }

            Debug.LogWarning("[WaveController] EnemySpawner referansı atanmamış. PR #9B'de sahne bağlantısı yapılmalı.");
            return false;
        }

        private IEnumerator RunWaves()
        {
            for (int i = 0; i < waves.Length; i++)
            {
                currentWaveIndex = i;
                WaveDefinition wave = waves[i];

                // Dalga öncesi bekleme
                if (wave.preWaveDelay > 0f)
                {
                    Debug.Log($"[WaveController] Dalga {i + 1}/{waves.Length} başlamadan önce {wave.preWaveDelay}s bekleniyor...");
                    yield return new WaitForSeconds(wave.preWaveDelay);
                }

                // Dalga başladı
                Debug.Log($"[WaveController] Dalga {i + 1}/{waves.Length} başladı! Düşman sayısı: {wave.enemyCount}");
                isSpawning = true;
                OnWaveStarted?.Invoke(i);

                // Düşmanları üret
                for (int j = 0; j < wave.enemyCount; j++)
                {
                    if (enemySpawner != null)
                    {
                        GameObject enemy = enemySpawner.SpawnEnemy();
                        if (enemy != null)
                        {
                            Debug.Log($"[WaveController] Düşman üretildi: {enemy.name} ({j + 1}/{wave.enemyCount})");
                        }
                    }

                    // Son düşmandan sonra bekleme yapma
                    if (j < wave.enemyCount - 1)
                    {
                        yield return new WaitForSeconds(wave.spawnInterval);
                    }
                }

                isSpawning = false;

                // Tüm düşmanlar yok edilene kadar bekle
                Debug.Log($"[WaveController] Dalga {i + 1} spawn tamamlandı. Aktif düşmanlar temizlenmesi bekleniyor...");
                yield return StartCoroutine(WaitForAllEnemiesDefeated());

                // Dalga tamamlandı
                Debug.Log($"[WaveController] Dalga {i + 1}/{waves.Length} tamamlandı!");
                OnWaveCompleted?.Invoke(i);
            }

            // Tüm dalgalar tamamlandı
            Debug.Log("[WaveController] Tüm dalgalar tamamlandı!");
            OnAllWavesCompleted?.Invoke();
            waveCoroutine = null;
        }

        private IEnumerator WaitForAllEnemiesDefeated()
        {
            // Kısa bir gecikme ile düşmanların Destroy edilmesini bekle
            yield return new WaitForSeconds(0.2f);

            while (GetAliveEnemyCount() > 0)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
