using UnityEngine;
using System.Collections.Generic;
using BlockForge.Core;
using BlockForge.Meta;
using BlockForge.Analytics;
using BlockForge.AudioVfx;

namespace BlockForge.Gameplay
{
    /// <summary>
    /// Run yöneticisi. Bir oyun turunu başlangıctan sona yönetir.
    /// Score, shape management, continue logic, game over.
    /// </summary>
    public class RunManager : MonoBehaviour
    {
        [Header("Referanslar")]
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private LineClearSystem _lineClearSystem;
        [SerializeField] private RunUIController _runUI;
        
        [Header("Shape Havuzu")]
        [SerializeField] private List<ShapeSO> _shapeLibrary = new List<ShapeSO>();
        
        [Header("Shape Renkleri")]
        [SerializeField] private Color[] _shapeColors = new Color[]
        {
            new Color(0.2f, 0.6f, 1f),   // Mavi
            new Color(1f, 0.4f, 0.3f),   // Kırmızı
            new Color(0.3f, 0.9f, 0.4f), // Yeşil
            new Color(1f, 0.8f, 0.2f),   // Sarı
            new Color(0.8f, 0.3f, 0.9f), // Mor
        };
        
        // State
        private ShapeGenerator _shapeGenerator;
        private List<ShapeSO> _currentShapes = new List<ShapeSO>();
        private int _score;
        private int _totalLinesCleared;
        private int _totalEnergy;
        private int _movesMade;
        private bool _isRunActive;
        private bool _hasContinued;
        
        // Events
        public System.Action<int> OnScoreChanged;
        public System.Action OnRunEnded;
        public System.Action<int> OnEnergyGained;
        
        private void Awake()
        {
            if (_gridManager == null) _gridManager = FindObjectOfType<GridManager>();
            if (_lineClearSystem == null) _lineClearSystem = FindObjectOfType<LineClearSystem>();
            if (_runUI == null) _runUI = FindObjectOfType<RunUIController>();
            
            // Auto-load shapes if missing (MANDATORY FIX)
            if (_shapeLibrary == null || _shapeLibrary.Count == 0)
            {
                var shapes = Resources.LoadAll<ShapeSO>("Shapes");
                if (shapes != null && shapes.Length > 0)
                {
                    _shapeLibrary = new List<ShapeSO>(shapes);
                    Debug.Log($"[RunManager] {_shapeLibrary.Count} shape Resources'dan otomatik yüklendi.");
                }
                else
                {
                    Debug.LogError("[RunManager] Shape library boş ve Resources/Shapes altında shape bulunamadı!");
                }
            }
        }

        private void Start()
        {
            StartRun();
        }
        
        /// <summary>
        /// Yeni bir run başlatır.
        /// </summary>
        public void StartRun()
        {
            Debug.Log("[RunManager] Run başlıyor...");
            
            // Grid'i başlat
            _gridManager.Initialize();
            
            // Shape generator'ı kur
            _shapeGenerator = new ShapeGenerator();
            _shapeGenerator.Initialize(_shapeLibrary, _gridManager);
            
            // State reset
            _score = 0;
            _totalLinesCleared = 0;
            _totalEnergy = 0;
            _movesMade = 0;
            _isRunActive = true;
            _hasContinued = false;
            
            // Events
            if (_lineClearSystem != null)
            {
                _lineClearSystem.OnEnergyProduced += HandleEnergyProduced;
            }
            
            // İlk 3 shape üret
            GenerateNewShapes();
            
            // UI güncelle
            UpdateUI();
            
            // Analytics
            var analytics = Services.Get<AnalyticsService>();
            analytics?.LogEvent("run_start");
            
            Debug.Log("[RunManager] Run başladı!");
        }
        
        /// <summary>
        /// Bir shape'i grid'e yerleştirir (ShapeDragController tarafından çağrılır).
        /// </summary>
        public void PlaceShape(ShapeSO shape, Vector2Int gridPos)
        {
            if (!_isRunActive) return;
            
            Color color = GetShapeColor(shape);
            PlaceResult result = _gridManager.PlaceShape(shape, gridPos, color);
            
            if (result.success)
            {
                _movesMade++;
                
                // Score hesapla
                int placementScore = shape.BlockCount;
                int clearBonus = result.clearResult.linesClearedCount * 10;
                int totalScore = placementScore + clearBonus;
                _score += totalScore;
                _totalLinesCleared += result.clearResult.linesClearedCount;
                
                OnScoreChanged?.Invoke(_score);
                
                // Analytics
                var analytics = Services.Get<AnalyticsService>();
                analytics?.LogEvent("shape_place", "shape_id", shape.Id);
                
                if (result.clearResult.linesClearedCount > 0)
                {
                    analytics?.LogEvent("lines_cleared", "count", result.clearResult.linesClearedCount.ToString());
                }
                
                // Mevcut shape'i sil ve kontrol et
                RemoveUsedShape(shape);
                
                // Tüm shape'ler kullanıldıysa yeni set üret
                if (GetRemainingShapeCount() == 0)
                {
                    GenerateNewShapes();
                }
                
                // Game over kontrolü
                CheckGameOver();
                
                // VFX & SFX
                var vfx = Services.Get<VFXManager>();
                if (vfx != null)
                {
                   // vfx.PlayMergeVFX(_gridManager.GridToWorld(gridPos)); // Optional: Effect where shape is placed
                }
                
                var audio = Services.Get<AudioService>();
                audio?.PlaySfx(SfxType.Place);

                UpdateUI();
            }
        }
        


        private void OnEnable()
        {
            // Subscribe to ProductionSystem
            var production = Services.Get<ProductionSystem>();
            if (production != null)
            {
                production.OnProduction += HandleProduction;
            }
        }

        private void OnDisable()
        {
             var production = Services.Get<ProductionSystem>();
             if (production != null)
             {
                 production.OnProduction -= HandleProduction;
             }
        }

        private void HandleProduction(string machineName, string itemName, int amount)
        {
            if (_runUI != null)
            {
                _runUI.ShowProductionPopup(machineName, itemName, amount);
            }
        }
        
        /// <summary>
        /// Enerji kazanıldığında (line clear tetiklediğinde).
        /// </summary>
        private void HandleEnergyProduced(int energy)
        {
            _totalEnergy += energy;
            OnEnergyGained?.Invoke(energy);
            
            // Üretim sistemi tetikle
            var production = Services.Get<ProductionSystem>();
            if (production != null)
            {
                production.ProcessEnergy(energy);
                Debug.Log($"[RunManager] +{energy} Enerji → Üretim tetiklendi.");
            }
            
            UpdateUI();
        }
        
        /// <summary>
        /// Yeni 3 shape üretir.
        /// </summary>
        private void GenerateNewShapes()
        {
            _currentShapes = _shapeGenerator.GenerateShapes();
            
            if (_runUI != null)
            {
                _runUI.UpdateShapeSlots(_currentShapes);
            }
        }
        
        /// <summary>
        /// Kullanılmış shape'i listeden çıkarır.
        /// </summary>
        private void RemoveUsedShape(ShapeSO shape)
        {
            _currentShapes.Remove(shape);
        }
        
        /// <summary>
        /// Kalan shape sayısını döner.
        /// </summary>
        private int GetRemainingShapeCount()
        {
            int count = 0;
            foreach (var shape in _currentShapes)
            {
                if (shape != null) count++;
            }
            return count;
        }
        
        /// <summary>
        /// Game over kontrolü - hiçbir shape yerleştirilemezse.
        /// </summary>
        private void CheckGameOver()
        {
            if (!_isRunActive) return;
            
            bool anyPlaceable = false;
            foreach (var shape in _currentShapes)
            {
                if (shape != null && _gridManager.CanPlaceAnywhere(shape))
                {
                    anyPlaceable = true;
                    break;
                }
            }
            
            if (!anyPlaceable)
            {
                EndRun();
            }
        }
        
        /// <summary>
        /// Run'ı bitirir.
        /// </summary>
        private void EndRun()
        {
            _isRunActive = false;
            
            Debug.Log($"[RunManager] Run bitti! Score: {_score}, Lines: {_totalLinesCleared}");
            
            // Analytics
            var analytics = Services.Get<AnalyticsService>();
            analytics?.LogEvent("run_end", "score", _score.ToString());
            
            // Continue teklifi (1 kere)
            if (!_hasContinued)
            {
                if (_runUI != null)
                {
                    _runUI.ShowContinueOffer(_score, _totalLinesCleared);
                    analytics?.LogEvent("continue_offer_shown");
                }
            }
            else
            {
                // Gerçek game over
                var audio = Services.Get<AudioService>();
                audio?.PlaySfx(SfxType.GameOver);
                ShowGameOver();
            }
            
            OnRunEnded?.Invoke();
        }
        
        /// <summary>
        /// Rewarded continue - reklam izlendikten sonra çağrılır.
        /// </summary>
        public void GrantContinue()
        {
            if (_hasContinued) return;
            
            _hasContinued = true;
            _isRunActive = true;
            
            // Yeni 3 shape üret
            GenerateNewShapes();
            
            // Analytics
            var analytics = Services.Get<AnalyticsService>();
            analytics?.LogEvent("continue_ad_watched_success");
            
            Debug.Log("[RunManager] Continue! Yeni shape'ler üretildi.");
            
            // Continue panel'i kapat, oyuna devam
            if (_runUI != null)
            {
                _runUI.HideContinueOffer();
            }
            
            UpdateUI();
        }
        
        /// <summary>
        /// Game over ekranını gösterir.
        /// </summary>
        private void ShowGameOver()
        {
            if (_runUI != null)
            {
                _runUI.ShowGameOver(_score, _totalLinesCleared);
            }
        }
        
        /// <summary>
        /// UI'ı günceller.
        /// </summary>
        private void UpdateUI()
        {
            if (_runUI != null)
            {
                _runUI.UpdateScore(_score);
                _runUI.UpdateEnergy(_totalEnergy);
                _runUI.UpdateLinesCleared(_totalLinesCleared);
            }
        }
        
        /// <summary>
        /// Shape için renk atar.
        /// </summary>
        private Color GetShapeColor(ShapeSO shape)
        {
            if (shape.Color != Color.white)
                return shape.Color;
            
            if (_shapeColors.Length > 0)
            {
                int hash = Mathf.Abs(shape.Id.GetHashCode());
                return _shapeColors[hash % _shapeColors.Length];
            }
            
            return Color.cyan;
        }
        
        /// <summary>
        /// Home'a dön.
        /// </summary>
        public void ReturnToHome()
        {
            // Cleanup
            if (_lineClearSystem != null)
            {
                _lineClearSystem.OnEnergyProduced -= HandleEnergyProduced;
            }
            
            SceneLoader.LoadScene("Home");
        }
        
        // Properties
        public int Score => _score;
        public int TotalLinesCleared => _totalLinesCleared;
        public int TotalEnergy => _totalEnergy;
        public bool IsRunActive => _isRunActive;
        public bool HasContinued => _hasContinued;
    }
}
