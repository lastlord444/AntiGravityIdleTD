using UnityEngine;
using AntiGravityTD.Core.Events;

namespace AntiGravityTD.Gameplay.Economy
{
    /// <summary>
    /// Altın miktarı değiştiğinde güncel değeri yayınlayan event kanalı.
    /// </summary>
    [CreateAssetMenu(fileName = "GoldChangedEventChannel", menuName = "AntiGravityTD/Events/GoldChangedEventChannel")]
    public class GoldChangedEventChannel : EventChannel<int>
    {
    }
}
