using UnityEngine;
using TMPro;
using AntiGravityTD.Gameplay.Core;

namespace AntiGravityTD.Gameplay.UI
{
    /// <summary>
    /// Oyun durumunu (NotStarted, Playing, Won, Lost) TextMeshProUGUI bileşeni üzerinde gösteren presenter.
    /// </summary>
    public class GameStateTextPresenter : MonoBehaviour
    {
        [Header("UI Referansları")]
        [SerializeField] private TextMeshProUGUI stateText;

        [Header("Veri Kaynağı")]
        [SerializeField] private GameLoopController gameLoopController;

        private bool isSubscribed = false;

        private void OnEnable()
        {
            if (gameLoopController == null)
            {
                gameLoopController = FindFirstObjectByType<GameLoopController>();
            }

            if (gameLoopController != null)
            {
                if (!isSubscribed)
                {
                    gameLoopController.OnGameStateChanged += HandleGameStateChanged;
                    isSubscribed = true;
                }
                UpdateDisplay(gameLoopController.CurrentState);
            }
            else
            {
                UpdateDisplayNull();
            }
        }

        private void OnDisable()
        {
            if (gameLoopController != null && isSubscribed)
            {
                gameLoopController.OnGameStateChanged -= HandleGameStateChanged;
                isSubscribed = false;
            }
        }

        private void HandleGameStateChanged(GameState newState)
        {
            UpdateDisplay(newState);
        }

        private void UpdateDisplay(GameState state)
        {
            if (stateText == null) return;

            switch (state)
            {
                case GameState.NotStarted:
                    stateText.text = "State: NotStarted";
                    break;
                case GameState.Playing:
                    stateText.text = "State: Playing";
                    break;
                case GameState.Won:
                    stateText.text = "Victory!";
                    break;
                case GameState.Lost:
                    stateText.text = "Defeat!";
                    break;
            }
        }

        private void UpdateDisplayNull()
        {
            if (stateText == null) return;
            stateText.text = "State: N/A";
        }
    }
}
