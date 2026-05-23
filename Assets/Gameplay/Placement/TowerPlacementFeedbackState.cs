using System;
using UnityEngine;

namespace AntiGravityTD.Gameplay.Placement
{
    /// <summary>
    /// Son kule yerleştirme sonucunu saklayan ve UI/debug sistemlerine bildiren durum bileşeni.
    /// Event kanalı üzerinden veya doğrudan SetResult ile beslenebilir.
    /// </summary>
    public class TowerPlacementFeedbackState : MonoBehaviour
    {
        [Header("Event Kanalları (Opsiyonel)")]
        [SerializeField, Tooltip("Yerleştirme event kanalı. Atanmışsa otomatik dinler.")]
        private TowerPlacementCompletedEventChannel placementCompletedChannel;

        private TowerPlacementResult lastResult;
        private bool hasResult;

        /// <summary>
        /// Son yerleştirme sonucu.
        /// </summary>
        public TowerPlacementResult LastResult => lastResult;

        /// <summary>
        /// En az bir yerleştirme sonucu kaydedilmiş mi?
        /// </summary>
        public bool HasResult => hasResult;

        /// <summary>
        /// Yerleştirme sonucu değiştiğinde tetiklenen event.
        /// UI presenter'lar bu event'i dinler.
        /// </summary>
        public event Action<TowerPlacementResult> OnPlacementResultChanged;

        private void OnEnable()
        {
            if (placementCompletedChannel != null)
            {
                placementCompletedChannel.AddListener(OnPlacementCompleted);
            }
        }

        private void OnDisable()
        {
            if (placementCompletedChannel != null)
            {
                placementCompletedChannel.RemoveListener(OnPlacementCompleted);
            }
        }

        /// <summary>
        /// Yerleştirme sonucunu günceller ve event'i tetikler.
        /// TowerPlacementInputHandler veya diğer sistemler tarafından çağrılır.
        /// </summary>
        /// <param name="result">Yerleştirme sonucu.</param>
        public void SetResult(TowerPlacementResult result)
        {
            lastResult = result;
            hasResult = true;
            OnPlacementResultChanged?.Invoke(result);
        }

        /// <summary>
        /// Son sonucu temizler.
        /// </summary>
        public void Clear()
        {
            lastResult = default;
            hasResult = false;
            OnPlacementResultChanged?.Invoke(lastResult);
        }

        /// <summary>
        /// Event kanalından gelen payload'ı TowerPlacementResult'a dönüştürüp saklar.
        /// </summary>
        private void OnPlacementCompleted(TowerPlacementResultPayload payload)
        {
            TowerPlacementResult result;

            if (payload.Success)
            {
                result = TowerPlacementResult.Succeed(
                    payload.PlacedTower,
                    payload.Cost,
                    payload.RemainingGold
                );
            }
            else
            {
                result = TowerPlacementResult.Fail(payload.FailureReason);
            }

            SetResult(result);
        }
    }
}
