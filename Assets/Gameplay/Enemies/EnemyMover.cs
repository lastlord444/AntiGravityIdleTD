using UnityEngine;
using AntiGravityTD.Gameplay.Path;

namespace AntiGravityTD.Gameplay.Enemies
{
    /// <summary>
    /// Düşman objelerinin WaypointPath boyunca hareket etmesini sağlar.
    /// Yol geçerliliğini denetler ve son noktaya ulaşıldığında nesneyi imha eder.
    /// </summary>
    public class EnemyMover : MonoBehaviour
    {
        [SerializeField] private WaypointPath path;
        [SerializeField] private float speed = 2.0f;
        [SerializeField] private float waypointReachThreshold = 0.05f;

        private int currentWaypointIndex = 0;
        private bool isPathValid = false;

        private void Start()
        {
            ValidateAndInitialize();
        }

        private void Update()
        {
            if (!isPathValid) return;

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
        /// </summary>
        private void OnReachedEnd()
        {
            Debug.Log($"[EnemyMover] Düşman son waypoint'e ulaştı ve yok ediliyor: {gameObject.name}");
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
