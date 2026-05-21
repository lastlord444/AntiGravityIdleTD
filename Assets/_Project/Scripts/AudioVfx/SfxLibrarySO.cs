using UnityEngine;

namespace BlockForge.AudioVfx
{
    /// <summary>
    /// SFX Library ScriptableObject. SFX tipine göre AudioClip eşlemesi.
    /// </summary>
    [CreateAssetMenu(fileName = "SfxLibrary", menuName = "BlockForge/SFX Library")]
    public class SfxLibrarySO : ScriptableObject
    {
        [Header("Gameplay")]
        [SerializeField] private AudioClip _placeSound;
        [SerializeField] private AudioClip _invalidPlaceSound;
        [SerializeField] private AudioClip _lineClearSound;
        
        [Header("Meta")]
        [SerializeField] private AudioClip _productionPopSound;
        [SerializeField] private AudioClip _contractDeliverSound;
        [SerializeField] private AudioClip _upgradeSound;
        
        [Header("UI")]
        [SerializeField] private AudioClip _buttonClickSound;
        
        [Header("Game Over")]
        [SerializeField] private AudioClip _gameOverSound;
        
        /// <summary>
        /// SFX tipine göre AudioClip döner.
        /// </summary>
        public AudioClip GetClip(SfxType sfxType)
        {
            switch (sfxType)
            {
                case SfxType.Place: return _placeSound;
                case SfxType.InvalidPlace: return _invalidPlaceSound;
                case SfxType.LineClear: return _lineClearSound;
                case SfxType.ProductionPop: return _productionPopSound;
                case SfxType.ContractDeliver: return _contractDeliverSound;
                case SfxType.ButtonClick: return _buttonClickSound;
                case SfxType.Upgrade: return _upgradeSound;
                case SfxType.GameOver: return _gameOverSound;
                default: return null;
            }
        }
    }
}
