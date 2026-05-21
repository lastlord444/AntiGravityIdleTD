using UnityEngine;
using System.Collections.Generic;

namespace BlockForge.Gameplay
{
    /// <summary>
    /// 10x10 Grid yöneticisi. Hücre durumları, yerleştirme doğrulama, satır/sütun temizleme.
    /// Bloklar yerçekimiyle düşmez (klasik 10x10 block puzzle).
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        public const int GRID_WIDTH = 10;
        public const int GRID_HEIGHT = 10;
        
        [Header("Grid Ayarları")]
        [SerializeField] private float _cellSize = 64f;
        [SerializeField] private float _cellSpacing = 2f;
        [SerializeField] private RectTransform _gridContainer;
        [SerializeField] private GameObject _cellPrefab;
        
        // Grid durumu: true = dolu, false = boş
        private bool[,] _occupied = new bool[GRID_WIDTH, GRID_HEIGHT];
        
        // Görsel hücre referansları
        private CellView[,] _cellViews = new CellView[GRID_WIDTH, GRID_HEIGHT];
        
        // Events
        public System.Action<LineClearResult> OnLinesCleared;
        public System.Action OnGridChanged;
        
        private void Awake()
        {
            if (_gridContainer == null)
            {
                // Try to find a child named "GridContainer" or use self
                var child = transform.Find("GridContainer");
                if (child != null) _gridContainer = child.GetComponent<RectTransform>();
                else _gridContainer = GetComponent<RectTransform>();
            }

            // Fallback: Load Cell Prefab from Resources
            if (_cellPrefab == null)
            {
                _cellPrefab = Resources.Load<GameObject>("Prefabs/Cell");
                if (_cellPrefab == null) Debug.LogError("[GridManager] Cell prefab could not be found in Resources/Prefabs/Cell!");
            }
        }

        /// <summary>
        /// Grid'i oluşturur ve hücreleri yerleştirir.
        /// </summary>
        public void Initialize()
        {
            ClearGrid();
            CreateCellViews();
            Debug.Log("[GridManager] Grid başlatıldı (10x10).");
        }
        
        /// <summary>
        /// Tüm hücreleri boşaltır.
        /// </summary>
        public void ClearGrid()
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    _occupied[x, y] = false;
                }
            }
        }
        
        /// <summary>
        /// Grid hücre görsellerini oluşturur.
        /// </summary>
        private void CreateCellViews()
        {
            if (_gridContainer == null || _cellPrefab == null)
            {
                Debug.LogWarning("[GridManager] Grid container veya cell prefab atanmamış!");
                return;
            }
            
            // Mevcut çocukları temizle
            foreach (Transform child in _gridContainer)
            {
                Destroy(child.gameObject);
            }
            
            float totalWidth = GRID_WIDTH * (_cellSize + _cellSpacing) - _cellSpacing;
            float totalHeight = GRID_HEIGHT * (_cellSize + _cellSpacing) - _cellSpacing;
            float startX = -totalWidth * 0.5f + _cellSize * 0.5f;
            float startY = totalHeight * 0.5f - _cellSize * 0.5f;
            
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    GameObject cellObj = Instantiate(_cellPrefab, _gridContainer);
                    cellObj.name = $"Cell_{x}_{y}";
                    
                    RectTransform rect = cellObj.GetComponent<RectTransform>();
                    if (rect != null)
                    {
                        float posX = startX + x * (_cellSize + _cellSpacing);
                        float posY = startY - y * (_cellSize + _cellSpacing);
                        rect.anchoredPosition = new Vector2(posX, posY);
                        rect.sizeDelta = new Vector2(_cellSize, _cellSize);
                    }
                    
                    CellView cellView = cellObj.GetComponent<CellView>();
                    if (cellView == null)
                    {
                        cellView = cellObj.AddComponent<CellView>();
                    }
                    cellView.Setup(x, y);
                    _cellViews[x, y] = cellView;
                }
            }
        }
        
        // ============================
        // YERLEŞTİRME
        // ============================
        
        /// <summary>
        /// Bir shape'in belirtilen grid konumuna yerleştirilebilir olup olmadığını kontrol eder.
        /// </summary>
        public bool CanPlace(ShapeSO shape, Vector2Int gridPos)
        {
            if (shape == null) return false;
            
            foreach (var block in shape.Blocks)
            {
                int checkX = gridPos.x + block.x;
                int checkY = gridPos.y + block.y;
                
                // Sınır kontrolü
                if (checkX < 0 || checkX >= GRID_WIDTH || checkY < 0 || checkY >= GRID_HEIGHT)
                    return false;
                
                // Doluluk kontrolü
                if (_occupied[checkX, checkY])
                    return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Bir shape'in grid'in herhangi bir yerine yerleştirilebilir olup olmadığını kontrol eder.
        /// ShapeGenerator'ın "imkansız üçlü" kontrolü için kullanılır.
        /// </summary>
        public bool CanPlaceAnywhere(ShapeSO shape)
        {
            if (shape == null) return false;
            
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    if (CanPlace(shape, new Vector2Int(x, y)))
                        return true;
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// Shape'i grid'e yerleştirir. Doğrulama + commit.
        /// Yerleştirme sonrası otomatik satır/sütun kontrolü yapar.
        /// </summary>
        public PlaceResult PlaceShape(ShapeSO shape, Vector2Int gridPos, Color color)
        {
            if (!CanPlace(shape, gridPos))
            {
                return new PlaceResult { success = false };
            }
            
            // Blokları yerleştir
            foreach (var block in shape.Blocks)
            {
                int cellX = gridPos.x + block.x;
                int cellY = gridPos.y + block.y;
                _occupied[cellX, cellY] = true;
                
                if (_cellViews[cellX, cellY] != null)
                {
                    _cellViews[cellX, cellY].SetOccupied(true, color);
                }
            }
            
            OnGridChanged?.Invoke();
            
            // Satır/sütun temizleme kontrolü
            LineClearResult clearResult = CheckAndClearLines();
            
            return new PlaceResult
            {
                success = true,
                clearResult = clearResult
            };
        }
        
        // ============================
        // SATIR/SÜTUN TEMİZLEME
        // ============================
        
        /// <summary>
        /// Dolu satır ve sütunları kontrol eder, varsa temizler.
        /// Aynı hamlede hem row hem col temizlenebilir.
        /// </summary>
        public LineClearResult CheckAndClearLines()
        {
            List<int> clearedRows = new List<int>();
            List<int> clearedCols = new List<int>();
            
            // Satır kontrolü
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                bool rowFull = true;
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    if (!_occupied[x, y])
                    {
                        rowFull = false;
                        break;
                    }
                }
                if (rowFull) clearedRows.Add(y);
            }
            
            // Sütun kontrolü
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                bool colFull = true;
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    if (!_occupied[x, y])
                    {
                        colFull = false;
                        break;
                    }
                }
                if (colFull) clearedCols.Add(x);
            }
            
            // Temizle (önce hepsini işaretle, sonra temizle - çapraz kesişimler doğru olsun)
            int cellsCleared = 0;
            HashSet<Vector2Int> cellsToClear = new HashSet<Vector2Int>();
            
            foreach (int row in clearedRows)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    cellsToClear.Add(new Vector2Int(x, row));
                }
            }
            
            foreach (int col in clearedCols)
            {
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    cellsToClear.Add(new Vector2Int(col, y));
                }
            }
            
            // Hücreleri temizle
            foreach (var cell in cellsToClear)
            {
                _occupied[cell.x, cell.y] = false;
                if (_cellViews[cell.x, cell.y] != null)
                {
                    _cellViews[cell.x, cell.y].SetOccupied(false, Color.clear);
                }
                cellsCleared++;
            }
            
            int totalLines = clearedRows.Count + clearedCols.Count;
            
            LineClearResult result = new LineClearResult
            {
                linesClearedCount = totalLines,
                cellsClearedCount = cellsCleared,
                clearedRows = clearedRows,
                clearedCols = clearedCols
            };
            
            if (totalLines > 0)
            {
                Debug.Log($"[GridManager] {clearedRows.Count} satır + {clearedCols.Count} sütun temizlendi ({cellsCleared} hücre).");
                OnLinesCleared?.Invoke(result);
                OnGridChanged?.Invoke();
            }
            
            return result;
        }
        
        /// <summary>
        /// Belirtilen grid koordinatını ekran koordinatına çevirir.
        /// </summary>
        public Vector2 GridToScreenPosition(Vector2Int gridPos)
        {
            if (_cellViews[gridPos.x, gridPos.y] != null)
            {
                RectTransform rect = _cellViews[gridPos.x, gridPos.y].GetComponent<RectTransform>();
                return rect.anchoredPosition;
            }
            return Vector2.zero;
        }
        
        /// <summary>
        /// Ekran/Canvas koordinatını en yakın grid koordinatına çevirir.
        /// </summary>
        public Vector2Int ScreenToGridPosition(Vector2 localPos)
        {
            if (_gridContainer == null) return new Vector2Int(-1, -1);
            
            float totalWidth = GRID_WIDTH * (_cellSize + _cellSpacing) - _cellSpacing;
            float totalHeight = GRID_HEIGHT * (_cellSize + _cellSpacing) - _cellSpacing;
            float startX = -totalWidth * 0.5f;
            float startY = totalHeight * 0.5f;
            
            int gridX = Mathf.FloorToInt((localPos.x - startX) / (_cellSize + _cellSpacing));
            int gridY = Mathf.FloorToInt((startY - localPos.y) / (_cellSize + _cellSpacing));
            
            gridX = Mathf.Clamp(gridX, 0, GRID_WIDTH - 1);
            gridY = Mathf.Clamp(gridY, 0, GRID_HEIGHT - 1);
            
            return new Vector2Int(gridX, gridY);
        }
        
        /// <summary>
        /// Grid doluluk oranını döner (debug / analytics için).
        /// </summary>
        public float GetOccupancyRate()
        {
            int filledCount = 0;
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    if (_occupied[x, y]) filledCount++;
                }
            }
            return (float)filledCount / (GRID_WIDTH * GRID_HEIGHT);
        }
        
        /// <summary>
        /// Belirtilen hücrenin dolu olup olmadığını döner.
        /// </summary>
        public bool IsCellOccupied(int x, int y)
        {
            if (x < 0 || x >= GRID_WIDTH || y < 0 || y >= GRID_HEIGHT) return true;
            return _occupied[x, y];
        }
        
        /// <summary>
        /// CellView referansını döner.
        /// </summary>
        public CellView GetCellView(int x, int y)
        {
            if (x < 0 || x >= GRID_WIDTH || y < 0 || y >= GRID_HEIGHT) return null;
            return _cellViews[x, y];
        }
    }
    
    /// <summary>
    /// Yerleştirme sonucu
    /// </summary>
    public struct PlaceResult
    {
        public bool success;
        public LineClearResult clearResult;
    }
    
    /// <summary>
    /// Satır/sütun temizleme sonucu
    /// </summary>
    public struct LineClearResult
    {
        public int linesClearedCount;
        public int cellsClearedCount;
        public List<int> clearedRows;
        public List<int> clearedCols;
    }
}
