using UnityEngine;

namespace BlockForge.Gameplay
{
    /// <summary>
    /// Booster (Güçlendirici) ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "NewBooster", menuName = "BlockForge/Booster")]
    public class BoosterSO : ScriptableObject
    {
        [Header("Temel Bilgiler")]
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [TextArea(2, 4)]
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        
        [Header("Özellikler")]
        [SerializeField] private BoosterType _type;
        [SerializeField] private int _startingCount; // Oyun başında verilen miktar
        
        [Header("Rewarded Teklifi (Opsiyonel)")]
        [SerializeField] private bool _hasRewardedOffer;
        [SerializeField] private int _rewardedAmount; // Reklam izlendiğinde verilen miktar
        
        // Properties
        public string Id => _id;
        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public BoosterType Type => _type;
        public int StartingCount => _startingCount;
        public bool HasRewardedOffer => _hasRewardedOffer;
        public int RewardedAmount => _rewardedAmount;
    }
    
    /// <summary>
    /// Booster tipleri
    /// </summary>
    public enum BoosterType
    {
        Undo,         // Son hareketi geri al
        Swap,         // Shape'leri değiştir
        Hammer,       // Tek hücreyi temizle
        ShapeRefresh  // Yeni 3 shape al
    }
}
