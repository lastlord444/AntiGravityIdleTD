using UnityEngine;

namespace AntiGravityTD.Gameplay.Placement
{
    /// <summary>
    /// Kule yerleştirme event kanalı ile taşınan veri paketi.
    /// Başarılı ve başarısız yerleştirme sonuçlarını event sistemi üzerinden iletir.
    /// </summary>
    public struct TowerPlacementResultPayload
    {
        /// <summary>Yerleştirme başarılı mı?</summary>
        public bool Success;

        /// <summary>Başarısızlık nedeni. Başarılıysa None.</summary>
        public TowerPlacementFailureReason FailureReason;

        /// <summary>Harcanan altın miktarı.</summary>
        public int Cost;

        /// <summary>İşlem sonrası kalan altın.</summary>
        public int RemainingGold;

        /// <summary>Yerleştirme yapılan nokta referansı.</summary>
        public TowerPlacementPoint PlacementPoint;

        /// <summary>Yerleştirilen kule GameObject referansı. Başarısızsa null.</summary>
        public GameObject PlacedTower;
    }
}
