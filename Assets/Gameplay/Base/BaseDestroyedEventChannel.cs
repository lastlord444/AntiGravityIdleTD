using UnityEngine;
using AntiGravityTD.Core.Events;

namespace AntiGravityTD.Gameplay.Base
{
    /// <summary>
    /// Base canı sıfıra inip imha edildiğinde tetiklenen parametresiz event kanalı.
    /// </summary>
    [CreateAssetMenu(fileName = "BaseDestroyedEventChannel", menuName = "AntiGravityTD/Events/BaseDestroyedEventChannel")]
    public class BaseDestroyedEventChannel : VoidEventChannel
    {
    }
}
