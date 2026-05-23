using UnityEngine;
using TMPro;
using AntiGravityTD.Gameplay.Economy;

namespace AntiGravityTD.Gameplay.UI
{
    /// <summary>
    /// Oyuncunun mevcut altın miktarını TextMeshProUGUI bileşeni üzerinde gösteren presenter.
    /// </summary>
    public class GoldTextPresenter : MonoBehaviour
    {
        [Header("UI Referansları")]
        [SerializeField] private TextMeshProUGUI goldText;

        [Header("Veri Kaynağı")]
        [SerializeField] private GoldWallet goldWallet;

        private int lastGoldValue = -1;

        private void Start()
        {
            if (goldWallet == null)
            {
                goldWallet = FindFirstObjectByType<GoldWallet>();
            }

            UpdateDisplay(true);
        }

        private void Update()
        {
            UpdateDisplay(false);
        }

        private void UpdateDisplay(bool forceUpdate)
        {
            if (goldText == null) return;

            if (goldWallet != null)
            {
                int currentGold = goldWallet.CurrentGold;

                if (forceUpdate || currentGold != lastGoldValue)
                {
                    goldText.text = $"Gold: {currentGold}";
                    lastGoldValue = currentGold;
                }
            }
            else
            {
                if (forceUpdate || lastGoldValue != -2)
                {
                    goldText.text = "Gold: N/A";
                    lastGoldValue = -2;
                }
            }
        }
    }
}
