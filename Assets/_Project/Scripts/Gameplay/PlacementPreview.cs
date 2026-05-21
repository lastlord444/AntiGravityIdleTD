using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BlockForge.Gameplay
{
    /// <summary>
    /// Placement preview - shape grid üzerinde gösterildiğinde "ghost" preview.
    /// Valid ise yeşil, invalid ise kırmızı.
    /// </summary>
    public class PlacementPreview : MonoBehaviour
    {
        [Header("Referanslar")]
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private RectTransform _previewContainer;
        
        [Header("Preview Ayarları")]
        [SerializeField] private GameObject _previewBlockPrefab;
        [SerializeField] private Color _validColor = new Color(0, 1, 0, 0.5f);
        [SerializeField] private Color _invalidColor = new Color(1, 0, 0, 0.5f);
        
        private List<GameObject> _previewBlocks = new List<GameObject>();
        private List<CellView> _affectedCells = new List<CellView>();
        
        private void Awake()
        {
            Hide();
        }
        
        /// <summary>
        /// Preview'i gösterir (valid/invalid durumuna göre).
        /// </summary>
        public void Show(ShapeSO shape, Vector2Int gridPos, bool isValid)
        {
            ClearPreviousPreview();
            
            if (shape == null || _previewBlockPrefab == null) return;
            
            Color previewColor = isValid ? _validColor : _invalidColor;
            
            // Her blok için preview oluştur
            foreach (var block in shape.Blocks)
            {
                int cellX = gridPos.x + block.x;
                int cellY = gridPos.y + block.y;
                
                // Sınır kontrolü
                if (cellX < 0 || cellX >= GridManager.GRID_WIDTH || 
                    cellY < 0 || cellY >= GridManager.GRID_HEIGHT)
                    continue;
                
                // Preview block'u oluştur
                GameObject previewObj = Instantiate(_previewBlockPrefab, _previewContainer);
                RectTransform rect = previewObj.GetComponent<RectTransform>();
                
                // Grid'de doğru pozisyona yerleştir
                CellView cellView = _gridManager.GetCellView(cellX, cellY);
                if (cellView != null)
                {
                    RectTransform cellRect = cellView.GetComponent<RectTransform>();
                    if (cellRect != null)
                    {
                        rect.anchoredPosition = cellRect.anchoredPosition;
                        rect.sizeDelta = cellRect.sizeDelta;
                    }
                }
                
                // Rengi ayarla
                Image img = previewObj.GetComponent<Image>();
                if (img != null)
                {
                    img.color = previewColor;
                }
                
                _previewBlocks.Add(previewObj);
                
                // Hücrenin preview'ini göster
                if (cellView != null && !_affectedCells.Contains(cellView))
                {
                    cellView.ShowPreview(isValid);
                    _affectedCells.Add(cellView);
                }
            }
            
            _previewContainer.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Preview'i gizler.
        /// </summary>
        public void Hide()
        {
            ClearPreviousPreview();
            _previewContainer.gameObject.SetActive(false);
        }
        
        private void ClearPreviousPreview()
        {
            // Preview block'ları yok et
            foreach (var block in _previewBlocks)
            {
                if (block != null) Destroy(block);
            }
            _previewBlocks.Clear();
            
            // Etkilenen hücrelerin preview'ini gizle
            foreach (var cell in _affectedCells)
            {
                if (cell != null) cell.HidePreview();
            }
            _affectedCells.Clear();
        }
    }
}
