using UnityEngine;
using System.Collections.Generic;

namespace BlockForge.Meta
{
    /// <summary>
    /// Atölye Makinesi ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "NewMachine", menuName = "BlockForge/Machine")]
    public class MachineSO : ScriptableObject
    {
        [Header("Temel Bilgiler")]
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private Sprite _icon;
        
        [Header("Seviye Bilgileri")]
        [SerializeField] private List<MachineLevel> _levels = new List<MachineLevel>();
        
        // Properties
        public string Id => _id;
        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
        public List<MachineLevel> Levels => _levels;
        
        /// <summary>
        /// Verilen seviye için MachineLevel verisini döner
        /// </summary>
        public MachineLevel GetLevel(int level)
        {
            if (level < 0 || level >= _levels.Count)
            {
                Debug.LogWarning($"[MachineSO] Geçersiz seviye: {level}, makine: {_id}");
                return _levels.Count > 0 ? _levels[_levels.Count - 1] : default;
            }
            return _levels[level];
        }
        
        public int MaxLevel => _levels.Count - 1;

        public void Init(string id, string displayName, List<MachineLevel> levels)
        {
            _id = id;
            _displayName = displayName;
            _levels = levels;
        }
    }
    
    /// <summary>
    /// Makine seviye verisi
    /// </summary>
    [System.Serializable]
    public struct MachineLevel
    {
        [Tooltip("Seviye numarası (0-tabanlı)")]
        public int level;
        
        [Tooltip("Üretim tetiklemesi için gereken enerji miktarı")]
        public int triggerCostEnergy;
        
        [Tooltip("Üretilen çıktılar")]
        public List<ItemAmount> outputs;
        
        [Tooltip("Kritik üretim şansı (0-1 arası)")]
        [Range(0f, 1f)]
        public float critChance;
        
        [Tooltip("Bir sonraki seviyeye yükseltme maliyeti (coin)")]
        public int upgradeCostCoins;
    }
}
