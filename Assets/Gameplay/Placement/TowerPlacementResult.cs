using UnityEngine;

namespace AntiGravityTD.Gameplay.Placement
{
    /// <summary>
    /// Kule yerleştirme girişiminin sonucunu taşıyan değer tipi.
    /// Immutable yapıda olup, sonuç oluşturulduktan sonra değiştirilemez.
    /// </summary>
    public readonly struct TowerPlacementResult
    {
        /// <summary>Yerleştirme başarılı mı?</summary>
        public readonly bool Success;

        /// <summary>Başarısızlık nedeni. Başarılıysa None.</summary>
        public readonly TowerPlacementFailureReason FailureReason;

        /// <summary>Yerleştirilen kule GameObject referansı. Başarısızsa null.</summary>
        public readonly GameObject PlacedTower;

        /// <summary>Harcanan altın miktarı.</summary>
        public readonly int Cost;

        /// <summary>İşlem sonrası kalan altın.</summary>
        public readonly int RemainingGold;

        /// <summary>
        /// Başarılı yerleştirme sonucu oluşturur.
        /// </summary>
        public static TowerPlacementResult Succeed(GameObject placedTower, int cost, int remainingGold)
        {
            return new TowerPlacementResult(
                success: true,
                failureReason: TowerPlacementFailureReason.None,
                placedTower: placedTower,
                cost: cost,
                remainingGold: remainingGold
            );
        }

        /// <summary>
        /// Başarısız yerleştirme sonucu oluşturur.
        /// </summary>
        public static TowerPlacementResult Fail(TowerPlacementFailureReason reason)
        {
            return new TowerPlacementResult(
                success: false,
                failureReason: reason,
                placedTower: null,
                cost: 0,
                remainingGold: -1
            );
        }

        private TowerPlacementResult(
            bool success,
            TowerPlacementFailureReason failureReason,
            GameObject placedTower,
            int cost,
            int remainingGold)
        {
            Success = success;
            FailureReason = failureReason;
            PlacedTower = placedTower;
            Cost = cost;
            RemainingGold = remainingGold;
        }
    }
}
