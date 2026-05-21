using UnityEngine;

namespace BlockForge.Gameplay
{
    /// <summary>
    /// Satır/sütun temizleme sistemi. Animasyon ve SFX tetikler.
    /// GridManager'daki clear mantığını tamamlar.
    /// </summary>
    public class LineClearSystem : MonoBehaviour
    {
        [Header("Referanslar")]
        [SerializeField] private GridManager _gridManager;
        
        [Header("Animasyon Ayarları")]
        [SerializeField] private float _clearAnimDuration = 0.3f;
        // MVP: _clearDelayBetween şimdilik kullanılmıyor, ilerde animasyon ekleneceğinde aktif edilecek
        // [SerializeField] private float _clearDelayBetween = 0.05f;
        
        // Events
        public System.Action<int> OnEnergyProduced; // linesClearedCount
        public System.Action<LineClearResult> OnLineClearAnimComplete;
        
        private void Start()
        {
            if (_gridManager != null)
            {
                _gridManager.OnLinesCleared += HandleLinesCleared;
            }
        }
        
        private void OnDestroy()
        {
            if (_gridManager != null)
            {
                _gridManager.OnLinesCleared -= HandleLinesCleared;
            }
        }
        
        /// <summary>
        /// Satır/sütun temizlendiğinde çağrılır.
        /// </summary>
        private void HandleLinesCleared(LineClearResult result)
        {
            // Enerji üret (her temizlenen satır/sütun = +1 Energy)
            int energyProduced = result.linesClearedCount;
            OnEnergyProduced?.Invoke(energyProduced);
            
            // SFX & VFX
            var audio = BlockForge.Core.Services.Get<BlockForge.AudioVfx.AudioService>();
            audio?.PlaySfx(BlockForge.AudioVfx.SfxType.LineClear);
            
            // Animasyon başlat
            StartCoroutine(PlayClearAnimation(result));
            
            Debug.Log($"[LineClearSystem] {result.linesClearedCount} hat temizlendi → +{energyProduced} Enerji");
        }
        
        /// <summary>
        /// Temizleme animasyonu (basit fade-out)
        /// </summary>
        private System.Collections.IEnumerator PlayClearAnimation(LineClearResult result)
        {
            // MVP: Basit bekleme, ilerde particle/anim eklenebilir
            yield return new WaitForSeconds(_clearAnimDuration);
            
            OnLineClearAnimComplete?.Invoke(result);
        }
    }
}
