using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BlockForge.Gameplay
{
    /// <summary>
    /// Shape sürükleme kontrolcüsü. Shape slotlarından grid'e sürükle-bırak.
    /// </summary>
    public class ShapeDragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Referanslar")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _gridRectTransform;
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private PlacementPreview _placementPreview;
        [SerializeField] private RunManager _runManager;
        
        [Header("Sürükleme Ayarları")]
        [SerializeField] private float _dragScale = 1.2f;
        
        private RectTransform _draggedShape;
        private ShapeSO _currentShape;
        private CanvasGroup _canvasGroup;
        private Vector2 _originalPosition;
        private Transform _originalParent;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            // Mevcut GameObject'deki ShapeSlot'u kontrol et
            ShapeSlot slot = GetComponent<ShapeSlot>();
            if (slot == null || slot.Shape == null)
            {
                Debug.LogWarning("[ShapeDragController] Shape slot yok veya shape boş!");
                return;
            }
            
            _currentShape = slot.Shape;
            _originalPosition = (transform as RectTransform).anchoredPosition;
            _originalParent = transform.parent;
            
            // Drag için görsel oluştur
            CreateDragPreview();
            
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.8f;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_draggedShape == null) return;
            
            // Sürükle
            _draggedShape.position = Input.mousePosition;
            
            // Grid'de valid/invalid preview göster
            UpdatePreview();
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_currentShape == null) return;
            
            // Grid'de valid bir yere bırakıldı mı?
            Vector2Int gridPos = GetGridPositionFromPointer(eventData);
            
            if (_gridManager != null && _gridManager.CanPlace(_currentShape, gridPos))
            {
                // Valid placement
                if (_runManager != null)
                {
                    _runManager.PlaceShape(_currentShape, gridPos);
                }
                else
                {
                    _gridManager.PlaceShape(_currentShape, gridPos, Color.white);
                }
                
                // Slot'u temizle
                ShapeSlot slot = GetComponent<ShapeSlot>();
                if (slot != null) slot.Clear();
            }
            
            // Cleanup
            DestroyDragPreview();
            _placementPreview?.Hide();
            
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
            
            _currentShape = null;
        }
        
        private void CreateDragPreview()
        {
            GameObject dragObj = new GameObject("DragPreview");
            dragObj.transform.SetParent(_canvas.transform, false);
            
            _draggedShape = dragObj.AddComponent<RectTransform>();
            _draggedShape.sizeDelta = new Vector2(100, 100);
            _draggedShape.position = Input.mousePosition;
            _draggedShape.localScale = Vector3.one * _dragScale;
            
            Image img = dragObj.AddComponent<Image>();
            img.sprite = _currentShape.PreviewSprite;
            img.color = _currentShape.Color;
            
            RectTransform rect = dragObj.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0.5f, 0.5f);
        }
        
        private void DestroyDragPreview()
        {
            if (_draggedShape != null)
            {
                Destroy(_draggedShape.gameObject);
                _draggedShape = null;
            }
        }
        
        private void UpdatePreview()
        {
            if (_placementPreview == null || _currentShape == null) return;
            
            Vector2Int gridPos = GetGridPositionFromPointer();
            
            if (_gridManager != null && _gridManager.CanPlace(_currentShape, gridPos))
            {
                _placementPreview.Show(_currentShape, gridPos, true);
            }
            else
            {
                _placementPreview.Show(_currentShape, gridPos, false);
            }
        }
        
        private Vector2Int GetGridPositionFromPointer(PointerEventData eventData = null)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _gridRectTransform,
                eventData != null ? eventData.position : Input.mousePosition,
                _canvas.worldCamera,
                out localPoint
            );
            
            return _gridManager.ScreenToGridPosition(localPoint);
        }
    }
    
    /// <summary>
    /// Shape slot referansı (UI'da gösterilen shape)
    /// </summary>
    public class ShapeSlot : MonoBehaviour
    {
        [SerializeField] private Image _shapeIcon;
        private ShapeSO _shape;
        
        public ShapeSO Shape => _shape;
        
        public void SetShape(ShapeSO shape)
        {
            _shape = shape;
            if (_shapeIcon != null && shape != null)
            {
                _shapeIcon.sprite = shape.PreviewSprite;
                _shapeIcon.color = shape.Color;
                _shapeIcon.gameObject.SetActive(true);
            }
        }
        
        public void Clear()
        {
            _shape = null;
            if (_shapeIcon != null)
            {
                _shapeIcon.gameObject.SetActive(false);
            }
        }
    }
}
