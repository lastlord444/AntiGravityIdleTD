using UnityEngine;
using System.Collections.Generic;

namespace BlockForge.Gameplay
{
    /// <summary>
    /// Shape (Şekil) ScriptableObject - 10x10 grid'e yerleştirilebilir blok grupları
    /// </summary>
    [CreateAssetMenu(fileName = "NewShape", menuName = "BlockForge/Shape")]
    public class ShapeSO : ScriptableObject
    {
        [Header("Temel Bilgiler")]
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        
        [Header("Şekil Tanımı")]
        [Tooltip("Pivot'a göre relatif koordinatlar (örn: (0,0), (1,0), (0,1))")]
        [SerializeField] private List<Vector2Int> _blocks = new List<Vector2Int>();
        
        [Header("Görsel")]
        [Tooltip("Önizleme sprite'ı (opsiyonel)")]
        [SerializeField] private Sprite _previewSprite;
        
        [Tooltip("Şekil rengi/teması")]
        [SerializeField] private Color _color = Color.white;
        
        // Properties
        public string Id => _id;
        public string DisplayName => _displayName;
        public List<Vector2Int> Blocks => _blocks;
        public Sprite PreviewSprite => _previewSprite;
        public Color Color => _color;
        
        /// <summary>
        /// Şeklin toplam blok sayısı
        /// </summary>
        public int BlockCount => _blocks.Count;
        
        /// <summary>
        /// Şeklin bounding box'ını hesaplar (debug için)
        /// </summary>
        public Bounds GetBounds()
        {
            if (_blocks.Count == 0) return new Bounds(Vector3.zero, Vector3.zero);
            
            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;
            
            foreach (var block in _blocks)
            {
                if (block.x < minX) minX = block.x;
                if (block.x > maxX) maxX = block.x;
                if (block.y < minY) minY = block.y;
                if (block.y > maxY) maxY = block.y;
            }
            
            Vector3 center = new Vector3((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, 0);
            Vector3 size = new Vector3(maxX - minX + 1, maxY - minY + 1, 0);
            
            return new Bounds(center, size);
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Editor'da görselleştirme için (opsiyonel)
        /// </summary>
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_displayName))
            {
                _displayName = name;
            }
            
            if (string.IsNullOrEmpty(_id))
            {
                _id = name.ToLower().Replace(" ", "_");
            }
        }
#endif
    }
}
