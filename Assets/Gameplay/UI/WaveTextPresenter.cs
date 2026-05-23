using UnityEngine;
using TMPro;
using AntiGravityTD.Gameplay.Waves;

namespace AntiGravityTD.Gameplay.UI
{
    /// <summary>
    /// Dalga durumunu (Mevcut Dalga / Toplam Dalga veya Tamamlandı) TextMeshProUGUI bileşeni üzerinde gösteren presenter.
    /// </summary>
    public class WaveTextPresenter : MonoBehaviour
    {
        [Header("UI Referansları")]
        [SerializeField] private TextMeshProUGUI waveText;

        [Header("Veri Kaynağı")]
        [SerializeField] private WaveController waveController;

        private bool allWavesCompleted = false;

        private void Start()
        {
            if (waveController == null)
            {
                waveController = FindFirstObjectByType<WaveController>();
            }

            if (waveController != null)
            {
                waveController.OnWaveStarted += HandleWaveStarted;
                waveController.OnWaveCompleted += HandleWaveCompleted;
                waveController.OnAllWavesCompleted += HandleAllWavesCompleted;
                UpdateDisplay(waveController.CurrentWaveIndex);
            }
            else
            {
                UpdateDisplayNull();
            }
        }

        private void OnDisable()
        {
            if (waveController != null)
            {
                waveController.OnWaveStarted -= HandleWaveStarted;
                waveController.OnWaveCompleted -= HandleWaveCompleted;
                waveController.OnAllWavesCompleted -= HandleAllWavesCompleted;
            }
        }

        private void HandleWaveStarted(int waveIndex)
        {
            allWavesCompleted = false;
            UpdateDisplay(waveIndex);
        }

        private void HandleWaveCompleted(int waveIndex)
        {
            if (!allWavesCompleted)
            {
                UpdateDisplay(waveIndex);
            }
        }

        private void HandleAllWavesCompleted()
        {
            allWavesCompleted = true;
            UpdateDisplayAllCompleted();
        }

        private void UpdateDisplay(int waveIndex)
        {
            if (waveText == null) return;

            if (waveController != null)
            {
                if (allWavesCompleted)
                {
                    UpdateDisplayAllCompleted();
                    return;
                }

                int totalWaves = waveController.TotalWaveCount;
                int displayWave = waveIndex >= 0 ? waveIndex + 1 : 0;
                waveText.text = $"Wave: {displayWave}/{totalWaves}";
            }
            else
            {
                waveText.text = "Wave: N/A";
            }
        }

        private void UpdateDisplayAllCompleted()
        {
            if (waveText == null) return;
            waveText.text = "Waves Complete";
        }

        private void UpdateDisplayNull()
        {
            if (waveText == null) return;
            waveText.text = "Wave: N/A";
        }
    }
}
