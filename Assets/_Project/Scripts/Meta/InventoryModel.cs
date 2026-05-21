using System.Collections.Generic;
using UnityEngine;
using BlockForge.Save;

namespace BlockForge.Meta
{
    /// <summary>
    /// Inventory (envanter) modeli. Item, coin, gem, booster sayılarını tutar.
    /// Global singleton olarak çalışır (Services'e kayıtlı).
    /// </summary>
    public class InventoryModel
    {
        // Currencies
        private int _coins;
        private int _gems;
        
        // Items (parça/ürün)
        private Dictionary<string, int> _items = new Dictionary<string, int>();
        
        // Boosters
        private Dictionary<string, int> _boosters = new Dictionary<string, int>();
        
        // Events
        public System.Action<int> OnCoinsChanged;
        public System.Action<int> OnGemsChanged;
        public System.Action<string, int> OnItemChanged; // itemId, newCount
        public System.Action<string, int> OnBoosterChanged; // boosterId, newCount
        
        // ======== CURRENCY ========
        
        public int Coins => _coins;
        public int Gems => _gems;
        
        public void AddCoins(int amount)
        {
            if (amount < 0) return;
            _coins += amount;
            OnCoinsChanged?.Invoke(_coins);
        }
        
        public bool SpendCoins(int amount)
        {
            if (amount < 0 || _coins < amount) return false;
            _coins -= amount;
            OnCoinsChanged?.Invoke(_coins);
            return true;
        }
        
        public void AddGems(int amount)
        {
            if (amount < 0) return;
            _gems += amount;
            OnGemsChanged?.Invoke(_gems);
        }
        
        public bool SpendGems(int amount)
        {
            if (amount < 0 || _gems < amount) return false;
            _gems -= amount;
            OnGemsChanged?.Invoke(_gems);
            return true;
        }
        
        // ======== ITEMS ========
        
        public int GetItemCount(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return 0;
            return _items.ContainsKey(itemId) ? _items[itemId] : 0;
        }
        
        public void AddItem(string itemId, int amount)
        {
            if (string.IsNullOrEmpty(itemId) || amount <= 0) return;
            
            if (!_items.ContainsKey(itemId))
                _items[itemId] = 0;
            
            _items[itemId] += amount;
            OnItemChanged?.Invoke(itemId, _items[itemId]);
        }
        
        public bool RemoveItem(string itemId, int amount)
        {
            if (string.IsNullOrEmpty(itemId) || amount <= 0) return false;
            
            int current = GetItemCount(itemId);
            if (current < amount) return false;
            
            _items[itemId] -= amount;
            OnItemChanged?.Invoke(itemId, _items[itemId]);
            return true;
        }
        
        public bool HasItems(List<ItemAmount> requirements)
        {
            foreach (var req in requirements)
            {
                if (req.item == null) continue;
                if (GetItemCount(req.item.Id) < req.amount)
                    return false;
            }
            return true;
        }
        
        public void ConsumeItems(List<ItemAmount> requirements)
        {
            foreach (var req in requirements)
            {
                if (req.item == null) continue;
                RemoveItem(req.item.Id, req.amount);
            }
        }
        
        // ======== BOOSTERS ========
        
        public int GetBoosterCount(string boosterId)
        {
            if (string.IsNullOrEmpty(boosterId)) return 0;
            return _boosters.ContainsKey(boosterId) ? _boosters[boosterId] : 0;
        }
        
        public void AddBooster(string boosterId, int amount)
        {
            if (string.IsNullOrEmpty(boosterId) || amount <= 0) return;
            
            if (!_boosters.ContainsKey(boosterId))
                _boosters[boosterId] = 0;
            
            _boosters[boosterId] += amount;
            OnBoosterChanged?.Invoke(boosterId, _boosters[boosterId]);
        }
        
        public bool UseBooster(string boosterId)
        {
            int current = GetBoosterCount(boosterId);
            if (current <= 0) return false;
            
            _boosters[boosterId]--;
            OnBoosterChanged?.Invoke(boosterId, _boosters[boosterId]);
            return true;
        }
        
        // ======== SAVE/LOAD ========
        
        public SaveModel ToSaveData()
        {
            SaveModel save = new SaveModel
            {
                coins = _coins,
                gems = _gems,
                inventory = new Dictionary<string, int>(_items),
                boosters = new Dictionary<string, int>(_boosters)
            };
            return save;
        }
        
        public void LoadFromSave(SaveModel save)
        {
            if (save == null) return;
            
            _coins = save.coins;
            _gems = save.gems;
            _items = save.inventory != null ? new Dictionary<string, int>(save.inventory) : new Dictionary<string, int>();
            _boosters = save.boosters != null ? new Dictionary<string, int>(save.boosters) : new Dictionary<string, int>();
            
            // Events tetikle
            OnCoinsChanged?.Invoke(_coins);
            OnGemsChanged?.Invoke(_gems);
        }
        
        /// <summary>
        /// Debug/test için tüm inventory'yi loglar.
        /// </summary>
        public void DebugPrint()
        {
            Debug.Log($"[InventoryModel] Coins: {_coins}, Gems: {_gems}");
            Debug.Log($"[InventoryModel] Items: {_items.Count}, Boosters: {_boosters.Count}");
        }
    }
}
