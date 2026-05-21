using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlockForge.Core
{
    /// <summary>
    /// Basit Service Locator pattern - MVP için yeterli.
    /// Singleton service'leri kaydeder ve erişim sağlar.
    /// </summary>
    public static class Services
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        
        /// <summary>
        /// Bir servisi kaydeder. Aynı tip zaten varsa üzerine yazar.
        /// </summary>
        public static void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Debug.LogWarning($"[Services] '{type.Name}' zaten kayıtlı, üzerine yazılıyor.");
            }
            _services[type] = service;
        }
        
        /// <summary>
        /// Kayıtlı bir servisi döner. Bulunamazsa null döner.
        /// </summary>
        public static T Get<T>() where T : class
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return service as T;
            }
            Debug.LogError($"[Services] '{type.Name}' bulunamadı! Önce Register çağrılmalı.");
            return null;
        }
        
        /// <summary>
        /// Bir servisin kayıtlı olup olmadığını kontrol eder.
        /// </summary>
        public static bool Has<T>() where T : class
        {
            return _services.ContainsKey(typeof(T));
        }
        
        /// <summary>
        /// Belirtilen servisi kaldırır.
        /// </summary>
        public static void Unregister<T>() where T : class
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                _services.Remove(type);
            }
        }
        
        /// <summary>
        /// Tüm kayıtlı servisleri temizler (test veya scene geçişi için).
        /// </summary>
        public static void ClearAll()
        {
            _services.Clear();
        }
    }
}
