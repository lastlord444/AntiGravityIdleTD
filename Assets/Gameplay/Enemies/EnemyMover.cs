using System;
using UnityEngine;
using AntiGravityTD.Gameplay.Path;

namespace AntiGravityTD.Gameplay.Enemies
{
    /// <summary>
    /// Düşman objelerinin WaypointPath boyunca hareket etmesini sağlar.
    /// Yol geçerliliğini denetler ve son noktaya ulaşıldığında nesneyi imha eder.
    /// hasReachedEnd bayrağı double-fire ve SetActive yarış koşulunu önler.
    /// </summary>
    public class EnemyMover : MonoBehaviour
    {
        /// <summary>
        /// Herhangi bir düşman yolun sonuna ulaştığında tetiklenir.
        /// GameLoopController tarafından lose koşulu olarak dinlenir.
        /// </summary>
        public static event Action<EnemyMover> OnAnyEnemyReachedEnd;

        [SerializeField] private WaypointPath path;
        [SerializeField] private float speed = 2.0f;
        [SerializeField] private float waypointReachThreshold = 0.05f;
        [SerializeField] private int baseDamage = 10;

        /// <summary>
        /// Düşman yolun sonuna ulaştığında base'e vereceği hasar miktarı.
        /// </summary>
        public int BaseDamage => baseDamage;

        private int currentWaypointIndex = 0;
        private bool isPathValid = false;
        private bool hasReachedEnd = false;

        private void Start()
        {
            ValidateAndInitialize();
        }

        private void Update()
        {
            if (!isPathValid || hasReachedEnd) return;

            // EnemyHealth tarafından zaten öldürüldüyse hareket etme
            var health = GetComponent<EnemyHealth>();
            if (health != null && health.IsDead) return;

            MoveAlongPath();
        }

        /// <summary>
        /// Yol referanslarını denetler ve düşmanı ilk waypoint noktasına yerleştirir.
        /// </summary>
        private void ValidateAndInitialize()
        {
            if (path == null)
            {
                Debug.LogWarning($"[EnemyMover] WaypointPath referansı atanmamış! Sahnede aranıyor... Object: {gameObject.name}");
                path = FindFirstObjectByType<WaypointPath>();
            }

            if (path == null || !path.IsValid)
            {
                Debug.LogError($"[EnemyMover] Geçerli bir WaypointPath bulunamadı! Hareket pasif hale getiriliyor. Object: {gameObject.name}");
                isPathValid = false;
                enabled = false;
                return;
            }

            isPathValid = true;
            currentWaypointIndex = 0;

            // Düşmanı ilk waypoint pozisyonuna yerleştir
            if (path.TryGetWaypointPosition(0, out Vector3 startPosition))
            {
                transform.position = startPosition;
            }
            else
            {
                Debug.LogError($"[EnemyMover] İlk waypoint pozisyonu alınamadı! Object: {gameObject.name}");
                isPathValid = false;
                enabled = false;
            }
        }

        /// <summary>
        /// Waypoint'ler arasında MoveTowards kullanarak hareket eder.
        /// </summary>
        private void MoveAlongPath()
        {
            if (currentWaypointIndex >= path.WaypointCount)
            {
                OnReachedEnd();
                return;
            }

            if (path.TryGetWaypointPosition(currentWaypointIndex, out Vector3 targetPosition))
            {
                // Hedef waypoint'e doğru ilerle
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                // Eşik değerine ulaşıldıysa bir sonraki waypoint'e geç
                if (Vector3.Distance(transform.position, targetPosition) < waypointReachThreshold)
                {
                    currentWaypointIndex++;
                }
            }
            else
            {
                Debug.LogWarning($"[EnemyMover] Waypoint {currentWaypointIndex} pozisyonu okunamadı! Bir sonraki waypoint'e geçiliyor. Object: {gameObject.name}");
                currentWaypointIndex++;
            }
        }

        /// <summary>
        /// Düşman yolun sonuna ulaştığında tetiklenir.
        /// hasReachedEnd bayrağı double-fire'ı önler.
        /// EnemyHealth.MarkDead() ile ölüm koordinasyonu sağlanır.
        /// </summary>
        private void OnReachedEnd()
        {
            if (hasReachedEnd) return;
            hasReachedEnd = true;

            Debug.Log($"[EnemyMover] Düşman son waypoint'e ulaştı ve yok ediliyor: {gameObject.name}");
            OnAnyEnemyReachedEnd?.Invoke(this);

            // EnemyHealth üzerinden koordineli ölüm — double-Destroy'u önler
            var health = GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.MarkDead();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

