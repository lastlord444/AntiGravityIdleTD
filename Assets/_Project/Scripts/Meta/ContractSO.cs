using UnityEngine;
using System.Collections.Generic;

namespace BlockForge.Meta
{
    /// <summary>
    /// Kontrat ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "NewContract", menuName = "BlockForge/Contract")]
    public class ContractSO : ScriptableObject
    {
        [Header("Temel Bilgiler")]
        [SerializeField] private string _id;
        [SerializeField] private string _title;
        [TextArea(3, 6)]
        [SerializeField] private string _description;
        
        [Header("Gereksinimler")]
        [SerializeField] private List<ItemAmount> _requirements = new List<ItemAmount>();
        
        [Header("Ödüller")]
        [SerializeField] private Reward _reward;
        
        [Header("Kurallar (Opsiyonel)")]
        [SerializeField] private ContractRules _rules;
        
        // Properties
        public string Id => _id;
        public string Title => _title;
        public string Description => _description;
        public List<ItemAmount> Requirements => _requirements;
        public Reward Reward => _reward;

        public ContractRules Rules => _rules;

        public void Init(string id, string title, string description, List<ItemAmount> requirements, Reward reward)
        {
            _id = id;
            _title = title;
            _description = description;
            _requirements = requirements;
            _reward = reward;
        }
    }
    
    /// <summary>
    /// Kontrat ödülü
    /// </summary>
    [System.Serializable]
    public struct Reward
    {
        [Tooltip("Coin ödülü")]
        public int coins;
        
        [Tooltip("Gem ödülü (opsiyonel)")]
        public int gems;
        
        [Tooltip("Ek item ödülleri (opsiyonel)")]
        public List<ItemAmount> bonusItems;
    }
    
    /// <summary>
    /// Kontrat kuralları (opsiyonel ekstra zorluk)
    /// </summary>
    [System.Serializable]
    public struct ContractRules
    {
        [Tooltip("Zaman sınırı (saniye, 0 = yok)")]
        public int timeLimitSeconds;
        
        [Tooltip("Minimum temizlenmesi gereken satır/sütun sayısı (0 = yok)")]
        public int minLinesClearedInRun;
        
        [Tooltip("Maksimum hamle sayısı (0 = sınırsız)")]
        public int maxMovesInRun;
    }
}
