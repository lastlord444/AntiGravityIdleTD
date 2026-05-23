using UnityEngine;
using TMPro;
using AntiGravityTD.Gameplay.Base;

namespace AntiGravityTD.Gameplay.UI
{
    /// <summary>
    /// Base HP durumunu TextMeshProUGUI bileşeni üzerinde gösteren presenter.
    /// </summary>
    public class BaseHealthTextPresenter : MonoBehaviour
    {
        [Header("UI Referansları")]
        [SerializeField] private TextMeshProUGUI healthText;

        [Header("Veri Kaynağı")]
        [SerializeField] private BaseHealth baseHealth;

        private int lastCurrentHealth = -1;
        private int lastMaxHealth = -1;

        private void Start()
        {
            if (baseHealth == null)
            {
                baseHealth = FindFirstObjectByType<BaseHealth>();
            }

            UpdateDisplay(true);
        }

        private void Update()
        {
            UpdateDisplay(false);
        }

        private void UpdateDisplay(bool forceUpdate)
        {
            if (healthText == null) return;

            if (baseHealth != null)
            {
                int current = baseHealth.CurrentHealth;
                int max = baseHealth.MaxHealth;

                if (forceUpdate || current != lastCurrentHealth || max != lastMaxHealth)
                {
                    healthText.text = $"HP: {current}/{max}";
                    lastCurrentHealth = current;
                    lastMaxHealth = max;
                }
            }
            else
            {
                if (forceUpdate || lastCurrentHealth != -2)
                {
                    healthText.text = "HP: N/A";
                    lastCurrentHealth = -2;
                }
            }
        }
    }
}
