using System.Collections.Generic;
using UnityEngine;

namespace AntiGravityTD.Core.Events
{
    /// <summary>
    /// ScriptableObject tabanlı event channel.
    /// Loose coupling için oyun içi eventleri yayınlar ve dinler.
    /// </summary>
    /// <typeparam name="T">Event payload tipi</typeparam>
    public abstract class EventChannel<T> : ScriptableObject
    {
        private readonly List<System.Action<T>> _listeners = new List<System.Action<T>>();

        /// <summary>
        /// Event dinleyici ekler.
        /// </summary>
        public void AddListener(System.Action<T> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        /// <summary>
        /// Event dinleyici kaldırır.
        /// </summary>
        public void RemoveListener(System.Action<T> listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }

        /// <summary>
        /// Event yayınlar (raise/invoke).
        /// </summary>
        public void Raise(T data)
        {
            // Reverse iteration ile listener removal sırasında hata önlenir
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i]?.Invoke(data);
            }
        }

        /// <summary>
        /// Tüm listener'ları temizler.
        /// </summary>
        public void ClearListeners()
        {
            _listeners.Clear();
        }
    }

    /// <summary>
    /// Parametresiz event channel (void event).
    /// </summary>
    public abstract class VoidEventChannel : ScriptableObject
    {
        private readonly List<System.Action> _listeners = new List<System.Action>();

        public void AddListener(System.Action listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        public void RemoveListener(System.Action listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }

        public void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i]?.Invoke();
            }
        }

        public void ClearListeners()
        {
            _listeners.Clear();
        }
    }
}
