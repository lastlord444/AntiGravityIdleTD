using UnityEngine;
using System.Collections.Generic;

namespace BlockForge.Meta
{
    /// <summary>
    /// Ürün/Parça ScriptableObject - Item (Ürün/Parça)
    /// </summary>
    [CreateAssetMenu(fileName = "NewItem", menuName = "BlockForge/Item")]
    public class ItemSO : ScriptableObject
    {
        [Header("Temel Bilgiler")]
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private Sprite _icon;
        
        [Header("Özellikler")]
        [SerializeField] private Rarity _rarity;
        [SerializeField] private int _baseValueCoins;
        [SerializeField] private bool _isIntermediate; // parça mı ürün mü
        
        // Properties
        public string Id => _id;
        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
        public Rarity Rarity => _rarity;
        public int BaseValueCoins => _baseValueCoins;
        public bool IsIntermediate => _isIntermediate;

        public void Init(string id, string displayName, Rarity rarity, int baseValue, bool isIntermediate)
        {
            _id = id;
            _displayName = displayName;
            _rarity = rarity;
            _baseValueCoins = baseValue;
            _isIntermediate = isIntermediate;
        }
    }
    
    /// <summary>
    /// Nadirlik seviyeleri
    /// </summary>
    public enum Rarity
    {
        Common,
        Rare,
        Epic
    }
    
    /// <summary>
    /// Item miktarı için struct
    /// </summary>
    [System.Serializable]
    public struct ItemAmount
    {
        public ItemSO item;
        public int amount;
        
        public ItemAmount(ItemSO item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
}
