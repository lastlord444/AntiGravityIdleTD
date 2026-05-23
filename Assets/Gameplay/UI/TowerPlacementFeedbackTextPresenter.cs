using UnityEngine;
using TMPro;
using AntiGravityTD.Gameplay.Placement;

namespace AntiGravityTD.Gameplay.UI
{
    /// <summary>
    /// Son kule yerleştirme sonucunu TextMeshProUGUI üzerinde gösteren presenter.
    /// TowerPlacementFeedbackState event'ini dinleyerek güncellenir.
    /// </summary>
    public class TowerPlacementFeedbackTextPresenter : MonoBehaviour
    {
        [Header("UI Referansları")]
        [SerializeField] private TextMeshProUGUI feedbackText;

        [Header("Veri Kaynağı")]
        [SerializeField] private TowerPlacementFeedbackState feedbackState;

        private void OnEnable()
        {
            if (feedbackState == null)
            {
                feedbackState = FindFirstObjectByType<TowerPlacementFeedbackState>();
            }

            if (feedbackState != null)
            {
                feedbackState.OnPlacementResultChanged += OnResultChanged;
            }

            UpdateDisplay();
        }

        private void OnDisable()
        {
            if (feedbackState != null)
            {
                feedbackState.OnPlacementResultChanged -= OnResultChanged;
            }
        }

        private void OnResultChanged(TowerPlacementResult result)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (feedbackText == null) return;

            if (feedbackState == null || !feedbackState.HasResult)
            {
                feedbackText.text = "Placement: Ready";
                return;
            }

            var result = feedbackState.LastResult;

            if (result.Success)
            {
                feedbackText.text = $"Tower placed! Gold left: {result.RemainingGold}";
                return;
            }

            feedbackText.text = result.FailureReason switch
            {
                TowerPlacementFailureReason.MissingTowerPrefab => "Placement failed: missing tower prefab",
                TowerPlacementFailureReason.MissingPlacementPoint => "Placement failed: no placement point selected",
                TowerPlacementFailureReason.PlacementPointOccupied => "Placement failed: point occupied",
                TowerPlacementFailureReason.MissingGoldWallet => "Placement failed: missing gold wallet",
                TowerPlacementFailureReason.InsufficientGold => "Placement failed: not enough gold",
                TowerPlacementFailureReason.InstantiateFailed => "Placement failed: instantiate failed",
                TowerPlacementFailureReason.MissingPlacementController => "Placement failed: missing placement controller",
                _ => $"Placement failed: {result.FailureReason}"
            };
        }
    }
}
