using UnityEngine;

namespace AntiGravityTD.Gameplay.Path
{
    /// <summary>
    /// Sahnede tanımlanan düşman hareket yollarını yönetir.
    /// Waypoint'leri saklar, indeks kontrolü yapar ve Gizmos ile yolu çizer.
    /// </summary>
    public class WaypointPath : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;

        /// <summary>
        /// Yoldaki toplam waypoint sayısı.
        /// </summary>
        public int WaypointCount => waypoints != null ? waypoints.Length : 0;

        /// <summary>
        /// Yolun geçerli olup olmadığını kontrol eder.
        /// En az 1 waypoint tanımlanmış olmalıdır.
        /// </summary>
        public bool IsValid => waypoints != null && waypoints.Length > 0;

        /// <summary>
        /// Verilen indeksteki waypoint pozisyonunu güvenle almaya çalışır.
        /// </summary>
        /// <param name="index">Alınmak istenen waypoint indeksi.</param>
        /// <param name="position">Bulunan pozisyon (hata durumunda Vector3.zero).</param>
        /// <returns>İndeks geçerli ve transform null değilse true, aksi halde false.</returns>
        public bool TryGetWaypointPosition(int index, out Vector3 position)
        {
            position = Vector3.zero;

            if (waypoints == null || index < 0 || index >= waypoints.Length)
            {
                return false;
            }

            Transform waypoint = waypoints[index];
            if (waypoint == null)
            {
                return false;
            }

            position = waypoint.position;
            return true;
        }

        /// <summary>
        /// Verilen indeksteki waypoint pozisyonunu döner.
        /// İndeks geçersizse veya transform null ise warning log basarak Vector3.zero döner.
        /// </summary>
        /// <param name="index">Alınmak istenen waypoint indeksi.</param>
        /// <returns>Waypoint pozisyonu veya safe fallback olarak Vector3.zero.</returns>
        public Vector3 GetWaypointPosition(int index)
        {
            if (TryGetWaypointPosition(index, out Vector3 position))
            {
                return position;
            }

            Debug.LogWarning($"[WaypointPath] Geçersiz waypoint isteği. Index: {index}, Toplam: {WaypointCount}. Fallback olarak Vector3.zero dönülüyor.");
            return Vector3.zero;
        }

        /// <summary>
        /// Editor ekranında waypoint noktalarını ve aralarındaki yolu yeşil çizgilerle çizer.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (waypoints == null || waypoints.Length == 0) return;

            Gizmos.color = Color.green;

            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null)
                {
                    // Waypoint noktası için küçük bir küre
                    Gizmos.DrawSphere(waypoints[i].position, 0.2f);

                    // Bir sonraki noktaya giden çizgi
                    if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                    }
                }
            }
        }
    }
}
