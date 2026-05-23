using UnityEngine;
using AntiGravityTD.Core.Events;

namespace AntiGravityTD.Gameplay.Placement
{
    /// <summary>
    /// Kule yerleştirme tamamlandığında (başarılı veya başarısız) tetiklenen event kanalı.
    /// ScriptableObject asset olarak oluşturulup ilgili bileşenlere atanır.
    /// </summary>
    [CreateAssetMenu(
        fileName = "TowerPlacementCompletedEventChannel",
        menuName = "AntiGravityTD/Events/TowerPlacementCompletedEventChannel")]
    public class TowerPlacementCompletedEventChannel : EventChannel<TowerPlacementResultPayload>
    {
    }
}
