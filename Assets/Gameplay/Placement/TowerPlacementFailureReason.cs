namespace AntiGravityTD.Gameplay.Placement
{
    /// <summary>
    /// Kule yerleştirme başarısızlık nedenlerini tanımlayan enum.
    /// None değeri başarılı yerleştirme anlamına gelir.
    /// </summary>
    public enum TowerPlacementFailureReason
    {
        /// <summary>Yerleştirme başarılı, hata yok.</summary>
        None = 0,

        /// <summary>Tower prefab referansı atanmamış.</summary>
        MissingTowerPrefab,

        /// <summary>Yerleştirme noktası (TowerPlacementPoint) null geldi.</summary>
        MissingPlacementPoint,

        /// <summary>Yerleştirme noktası zaten dolu.</summary>
        PlacementPointOccupied,

        /// <summary>GoldWallet bulunamadı (null).</summary>
        MissingGoldWallet,

        /// <summary>Altın yetersiz.</summary>
        InsufficientGold,

        /// <summary>Instantiate işlemi beklenmedik şekilde başarısız oldu.</summary>
        InstantiateFailed
    }
}
