using UnityEngine;
using AntiGravityTD.Core.Events;

namespace AntiGravityTD.Gameplay.Base
{
    /// <summary>
    /// Base can değişimi olaylarında taşınacak veri paketi.
    /// </summary>
    public struct BaseHealthChangedPayload
    {
        public int CurrentHealth;
        public int MaxHealth;
    }

    /// <summary>
    /// Base can değeri değiştiğinde tetiklenen event kanalı.
    /// </summary>
    [CreateAssetMenu(fileName = "BaseHealthChangedEventChannel", menuName = "AntiGravityTD/Events/BaseHealthChangedEventChannel")]
    public class BaseHealthChangedEventChannel : EventChannel<BaseHealthChangedPayload>
    {
    }
}
