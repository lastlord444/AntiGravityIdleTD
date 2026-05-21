using System;
using System.Collections.Generic;
using UnityEngine;

namespace AntiGravityTD.Core
{
    /// <summary>
    /// Global service kayıt ve erişim noktası.
    /// Singleton pattern ile tüm oyun boyunca tek instance.
    /// </summary>
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator _instance;
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[ServiceLocator]");
                    _instance = go.AddComponent<ServiceLocator>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Service kaydı yapar.
        /// </summary>
        public void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Debug.LogWarning($"Service {type.Name} zaten kayıtlı. Üzerine yazılıyor.");
            }
            _services[type] = service;
            Debug.Log($"[ServiceLocator] {type.Name} kaydedildi.");
        }

        /// <summary>
        /// Service çözümler (resolve).
        /// </summary>
        public T Get<T>() where T : class
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return service as T;
            }

            Debug.LogError($"Service {type.Name} bulunamadı! Önce Register edilmiş olmalı.");
            return null;
        }

        /// <summary>
        /// Service kayıtlı mı kontrol eder.
        /// </summary>
        public bool IsRegistered<T>() where T : class
        {
            return _services.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Tüm servisleri temizler (test veya scene reload için).
        /// </summary>
        public void Clear()
        {
            _services.Clear();
            Debug.Log("[ServiceLocator] Tüm servisler temizlendi.");
        }
    }
}
