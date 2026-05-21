using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using BlockForge.Core;
using BlockForge.Monetization;

namespace BlockForge.Gameplay
{
    /// <summary>
    /// Run ekranı UI kontrolcüsü.
    /// Score, shapes, energy, continue panel, game over panel.
    /// </summary>
    public class RunUIController : MonoBehaviour
    {
        [Header("Üst Bar")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _linesClearedText;
        [SerializeField] private TextMeshProUGUI _energyText;
        
        [Header("Shape Slotları")]
        [SerializeField] private List<ShapeSlot> _shapeSlots = new List<ShapeSlot>();
        
        [Header("Üretim Pop-up")]
        [SerializeField] private GameObject _productionPopupPrefab;
        [SerializeField] private RectTransform _productionPopupContainer;
        
        [Header("Continue Panel")]
        [SerializeField] private GameObject _continuePanel;
        [SerializeField] private TextMeshProUGUI _continueScoreText;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _continueDeclineButton;
        
        [Header("Game Over Panel")]
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private TextMeshProUGUI _gameOverScoreText;
        [SerializeField] private TextMeshProUGUI _gameOverLinesText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _homeButton;
        
        [Header("Referanslar")]
        [SerializeField] private RunManager _runManager;
        

        
        private void Awake()
        {
            if (_runManager == null) _runManager = FindObjectOfType<RunManager>();
            
            if (_productionPopupContainer == null)
            {
                var containerObj = new GameObject("ProductionPopupContainer");
                containerObj.transform.SetParent(transform, false);
                _productionPopupContainer = containerObj.AddComponent<RectTransform>();
                _productionPopupContainer.anchorMin = Vector2.zero;
                _productionPopupContainer.anchorMax = Vector2.one;
                _productionPopupContainer.offsetMin = Vector2.zero;
                _productionPopupContainer.offsetMax = Vector2.zero;
            }
        }

        private void Start()
        {
            // Panelleri gizle
            if (_continuePanel != null) _continuePanel.SetActive(false);
            if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
            
            // Button listeners
            if (_continueButton != null)
                _continueButton.onClick.AddListener(OnContinueClicked);
            
            if (_continueDeclineButton != null)
                _continueDeclineButton.onClick.AddListener(OnContinueDeclined);
            
            if (_restartButton != null)
                _restartButton.onClick.AddListener(OnRestartClicked);
            
            if (_homeButton != null)
                _homeButton.onClick.AddListener(OnHomeClicked);
        }
        
        /// <summary>
        /// Score güncellemesi
        /// </summary>
        public void UpdateScore(int score)
        {
            if (_scoreText != null)
                _scoreText.text = score.ToString("N0");
        }
        
        /// <summary>
        /// Enerji güncellemesi
        /// </summary>
        public void UpdateEnergy(int energy)
        {
            if (_energyText != null)
                _energyText.text = $"E: {energy}";
        }
        
        /// <summary>
        /// Temizlenen satır sayısı
        /// </summary>
        public void UpdateLinesCleared(int lines)
        {
            if (_linesClearedText != null)
                _linesClearedText.text = $"Lines: {lines}";
        }
        
        /// <summary>
        /// Shape slotlarını günceller.
        /// </summary>
        public void UpdateShapeSlots(List<ShapeSO> shapes)
        {
            for (int i = 0; i < _shapeSlots.Count; i++)
            {
                if (i < shapes.Count && shapes[i] != null)
                {
                    _shapeSlots[i].SetShape(shapes[i]);
                }
                else
                {
                    _shapeSlots[i].Clear();
                }
            }
        }
        
        /// <summary>
        /// Üretim pop-up'ı gösterir ("Press produced +2 Bolt" gibi ABARTILI gösterim)
        /// </summary>
        public void ShowProductionPopup(string machineName, string itemName, int amount)
        {
            if (_productionPopupPrefab == null || _productionPopupContainer == null) return;
            
            GameObject popup = Instantiate(_productionPopupPrefab, _productionPopupContainer);
            
            TextMeshProUGUI popupText = popup.GetComponentInChildren<TextMeshProUGUI>();
            if (popupText != null)
            {
                // ABARTILI gösterim - hook'u ilk 5 saniyede göstermek
                popupText.text = $"<size=120%><color=#FFD700>{machineName}</color></size>\n<size=150%><b>+{amount} {itemName}!</b></size>";
            }
            
            // 3 saniye sonra sil
            Destroy(popup, 3f);
            
            Debug.Log($"[RunUI] Üretim pop-up: {machineName} → +{amount} {itemName}");
        }
        
        /// <summary>
        /// Continue teklifi göster (rewarded ad).
        /// </summary>
        public void ShowContinueOffer(int score, int linesCleared)
        {
            if (_continuePanel == null) return;
            
            _continuePanel.SetActive(true);
            
            if (_continueScoreText != null)
                _continueScoreText.text = $"Skor: {score}\nDevam etmek için reklam izle!";
        }
        
        /// <summary>
        /// Continue paneli gizle.
        /// </summary>
        public void HideContinueOffer()
        {
            if (_continuePanel != null)
                _continuePanel.SetActive(false);
        }
        
        /// <summary>
        /// Game over paneli göster.
        /// </summary>
        public void ShowGameOver(int finalScore, int totalLines)
        {
            if (_gameOverPanel == null) return;
            
            _gameOverPanel.SetActive(true);
            
            if (_gameOverScoreText != null)
                _gameOverScoreText.text = $"Skor: {finalScore}";
            
            if (_gameOverLinesText != null)
                _gameOverLinesText.text = $"Temizlenen Hat: {totalLines}";
        }
        
        // ======== Button Handlers ========
        
        private void OnContinueClicked()
        {
            Services.Get<BlockForge.AudioVfx.AudioService>()?.PlaySfx(BlockForge.AudioVfx.SfxType.ButtonClick);
            
            // Rewarded reklam göster
            var adsService = Services.Get<AdsService>();
            if (adsService != null && adsService.IsRewardedReady())
            {
                adsService.ShowRewarded("continue_rewarded",
                    onReward: () =>
                    {
                        if (_runManager != null)
                            _runManager.GrantContinue();
                    },
                    onFail: () =>
                    {
                        Debug.LogWarning("[RunUI] Rewarded ad başarısız!");
                        ShowGameOver(_runManager.Score, _runManager.TotalLinesCleared);
                    }
                );
            }
            else
            {
                // Reklam hazır değilse direkt continue (MVP test için)
                Debug.LogWarning("[RunUI] Rewarded ad hazır değil, direkt continue veriliyor (MVP).");
                if (_runManager != null)
                    _runManager.GrantContinue();
            }
        }
        
        private void OnContinueDeclined()
        {
            Services.Get<BlockForge.AudioVfx.AudioService>()?.PlaySfx(BlockForge.AudioVfx.SfxType.ButtonClick);
            HideContinueOffer();
            if (_runManager != null)
                ShowGameOver(_runManager.Score, _runManager.TotalLinesCleared);
        }
        
        private void OnRestartClicked()
        {
            Services.Get<BlockForge.AudioVfx.AudioService>()?.PlaySfx(BlockForge.AudioVfx.SfxType.ButtonClick);
            SceneLoader.LoadScene("Run");
        }
        
        private void OnHomeClicked()
        {
            Services.Get<BlockForge.AudioVfx.AudioService>()?.PlaySfx(BlockForge.AudioVfx.SfxType.ButtonClick);
            if (_runManager != null)
                _runManager.ReturnToHome();
        }
    }
}
