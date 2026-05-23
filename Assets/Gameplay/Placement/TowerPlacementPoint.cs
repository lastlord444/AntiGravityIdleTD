using UnityEngine;

namespace AntiGravityTD.Gameplay.Placement
{
    /// <summary>
    /// Sahne üzerinde kule yerleştirme noktasını temsil eden bileşen.
    /// Dolu/boş durumunu ve yerleştirilen kule referansını takip eder.
    /// Kendi başına kule oluşturmaz veya altın harcamaz.
    /// </summary>
    public class TowerPlacementPoint : MonoBehaviour
    {
        [Header("Durum (Runtime)")]
        [SerializeField, Tooltip("Editör debug için. Runtime'da otomatik yönetilir.")]
        private bool isOccupied;

        private GameObject placedTower;

        /// <summary>
        /// Bu noktaya zaten bir kule yerleştirilmiş mi?
        /// </summary>
        public bool IsOccupied => isOccupied;

        /// <summary>
        /// Bu noktaya yerleştirilmiş kule referansı. Boşsa null.
        /// </summary>
        public GameObject PlacedTower => placedTower;

        /// <summary>
        /// Bu noktaya kule yerleştirilebilir mi?
        /// Nokta dolu değilse true döner.
        /// </summary>
        public bool CanPlace()
        {
            return !isOccupied;
        }

        /// <summary>
        /// Bu noktayı dolu olarak işaretler ve yerleştirilen kuleyi kaydeder.
        /// </summary>
        /// <param name="tower">Yerleştirilen kule GameObject'i.</param>
        public void MarkOccupied(GameObject tower)
        {
            if (tower == null)
            {
                Debug.LogWarning($"[TowerPlacementPoint] MarkOccupied null tower ile çağrıldı: {gameObject.name}");
                return;
            }

            isOccupied = true;
            placedTower = tower;
        }

        /// <summary>
        /// Bu noktayı boşaltır ve kule referansını temizler.
        /// Kulenin kendisini yok etmez, sadece referansı sıfırlar.
        /// </summary>
        public void Clear()
        {
            isOccupied = false;
            placedTower = null;
        }
    }
}
