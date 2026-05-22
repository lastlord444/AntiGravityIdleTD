using UnityEngine;

namespace AntiGravityTD.Gameplay.Waves
{
    /// <summary>
    /// IEnemySpawner arayüzünün somut (concrete) MonoBehaviour uygulaması.
    /// Belirlenen prefab'ı spawn noktasında üretir.
    /// </summary>
    public class EnemySpawner : MonoBehaviour, IEnemySpawner
    {
        [Header("Spawn Settings")]
        [Tooltip("Spawn edilecek düşman prefab'ı.")]
        [SerializeField] private GameObject enemyPrefab;

        [Tooltip("Düşmanın spawn edileceği konum/yön.")]
        [SerializeField] private Transform spawnPoint;

        [Tooltip("Spawn edilen düşmanların hiyerarşide konumlanacağı parent (isteğe bağlı).")]
        [SerializeField] private Transform enemyParent;

        [Tooltip("Spawn edilen düşmanın otomatik aktif edilip edilmeyeceği.")]
        [SerializeField] private bool activateOnSpawn = true;

        /// <summary>
        /// Tanımlı düşman prefab'ını spawn noktasında instantiate eder.
        /// </summary>
        /// <returns>Oluşturulan düşman GameObject'i, geçersiz durumda null.</returns>
        public GameObject SpawnEnemy()
        {
            if (enemyPrefab == null)
            {
                Debug.LogError("[EnemySpawner] Enemy prefab is not assigned.");
                return null;
            }

            if (spawnPoint == null)
            {
                Debug.LogError("[EnemySpawner] Spawn point is not assigned.");
                return null;
            }

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, enemyParent);
            if (enemy != null)
            {
                enemy.SetActive(activateOnSpawn);
                Debug.Log($"[EnemySpawner] Spawned enemy: {enemy.name} at {spawnPoint.position}");
            }
            return enemy;
        }
    }
}
