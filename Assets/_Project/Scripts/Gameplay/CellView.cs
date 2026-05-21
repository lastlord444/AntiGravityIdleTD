using UnityEngine;
using UnityEngine.UI;

namespace BlockForge.Gameplay
{
    /// <summary>
    /// Grid hücre görünümü. Boş/dolu durumu ve rengini gösterir.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class CellView : MonoBehaviour
    {
        [Header("Görsel Ayarları")]
        [SerializeField] private Color _emptyColor = new Color(0.2f, 0.2f, 0.2f, 0.3f);
        [SerializeField] private Color _filledColor = Color.white;
        
        private Image _image;
        private Vector2Int _gridPosition;
        private bool _isOccupied;
        
        /// <summary>
        /// Hücre kurulumu
        /// </summary>
        public void Setup(int x, int y)
        {
            _gridPosition = new Vector2Int(x, y);
            _image = GetComponent<Image>();
            if (_image == null)
            {
                _image = gameObject.AddComponent<Image>();
            }
            SetOccupied(false, Color.clear);
        }
        
        /// <summary>
        /// Doluluk durumunu ve rengini ayarlar.
        /// </summary>
        public void SetOccupied(bool occupied, Color color)
        {
            _isOccupied = occupied;
            
            if (_image != null)
            {
                if (occupied)
                {
                    _image.color = color;
                }
                else
                {
                    _image.color = _emptyColor;
                }
            }
        }
        
        /// <summary>
        /// Preview için geçici renk gösterir (valid/invalid).
        /// </summary>
        public void ShowPreview(bool isValid)
        {
            if (_image != null)
            {
                _image.color = isValid ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
            }
        }
        
        /// <summary>
        /// Preview'i gizler, orijinal renge döner.
        /// </summary>
        public void HidePreview()
        {
            SetOccupied(_isOccupied, _filledColor);
        }
        
        public Vector2Int GridPosition => _gridPosition;
        public bool IsOccupied => _isOccupied;
    }
}
