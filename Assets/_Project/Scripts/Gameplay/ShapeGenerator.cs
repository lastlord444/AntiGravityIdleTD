using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BlockForge.Gameplay
{
    /// <summary>
    /// Shape üretici. Her tur 3 shape döner.
    /// "İmkansız üçlü" (hiçbirinin sığmadığı durum) üretmez.
    /// </summary>
    public class ShapeGenerator
    {
        private List<ShapeSO> _shapeLibrary;
        private GridManager _gridManager;
        private const int MAX_GENERATION_ATTEMPTS = 20;
        
        /// <summary>
        /// ShapeGenerator kurulumu
        /// </summary>
        public void Initialize(List<ShapeSO> shapeLibrary, GridManager gridManager)
        {
            _shapeLibrary = shapeLibrary;
            _gridManager = gridManager;
            
            if (_shapeLibrary == null || _shapeLibrary.Count == 0)
            {
                Debug.LogError("[ShapeGenerator] Shape library boş!");
            }
        }
        
        /// <summary>
        /// 3 shape üretir. En az birinin sığacağı garanti edilir.
        /// </summary>
        public List<ShapeSO> GenerateShapes()
        {
            if (_shapeLibrary == null || _shapeLibrary.Count == 0)
            {
                Debug.LogError("[ShapeGenerator] Shape library yok, boş liste dönülüyor.");
                return new List<ShapeSO>();
            }
            
            List<ShapeSO> result = new List<ShapeSO>(3);
            
            for (int attempt = 0; attempt < MAX_GENERATION_ATTEMPTS; attempt++)
            {
                result.Clear();
                
                // 3 rastgele shape seç
                for (int i = 0; i < 3; i++)
                {
                    ShapeSO randomShape = _shapeLibrary[Random.Range(0, _shapeLibrary.Count)];
                    result.Add(randomShape);
                }
                
                // En az birinin sığıp sığmadığını kontrol et
                bool atLeastOneFits = false;
                foreach (var shape in result)
                {
                    if (_gridManager != null && _gridManager.CanPlaceAnywhere(shape))
                    {
                        atLeastOneFits = true;
                        break;
                    }
                }
                
                if (atLeastOneFits || _gridManager == null)
                {
                    // Geçerli kombinasyon bulundu
                    return result;
                }
            }
            
            // MAX_GENERATION_ATTEMPTS sonrası hala bulamadıysak, güncel grid durumuna göre
            // sığabilecek shape'leri filtrele ve onlardan seç
            Debug.LogWarning("[ShapeGenerator] İmkansız üçlü önlenemedi, filtreli seçim yapılıyor...");
            
            List<ShapeSO> validShapes = new List<ShapeSO>();
            foreach (var shape in _shapeLibrary)
            {
                if (_gridManager != null && _gridManager.CanPlaceAnywhere(shape))
                {
                    validShapes.Add(shape);
                }
            }
            
            if (validShapes.Count == 0)
            {
                // Grid tamamen dolu, game over durumu
                Debug.LogWarning("[ShapeGenerator] Grid'de hiç shape sığmıyor, oyun bitmeli!");
                return _shapeLibrary.Take(3).ToList(); // En azından 3 shape dön
            }
            
            // Filtreli listeden 3 tane seç
            result.Clear();
            for (int i = 0; i < 3; i++)
            {
                result.Add(validShapes[Random.Range(0, validShapes.Count)]);
            }
            
            return result;
        }
        
        /// <summary>
        /// Tek bir shape üretir (booster için).
        /// </summary>
        public ShapeSO GenerateSingleShape()
        {
            if (_shapeLibrary == null || _shapeLibrary.Count == 0)
            {
                return null;
            }
            return _shapeLibrary[Random.Range(0, _shapeLibrary.Count)];
        }
    }
}
