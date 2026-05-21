using UnityEngine;
using System;
using System.Collections.Generic;

namespace BlockForge.Analytics
{
    /// <summary>
    /// Analytics servisi. MVP: Debug.Log + opsiyonel local JSON.
    /// Sonra Firebase Analytics entegrasyonu yapılabilir.
    /// </summary>
    public class AnalyticsService
    {
        private List<AnalyticsEvent> _eventLog = new List<AnalyticsEvent>();
        private bool _isEnabled = true;
        
        /// <summary>
        /// Event loglar (parametre olmadan).
        /// </summary>
        public void LogEvent(string eventName)
        {
            if (!_isEnabled) return;
            
            var ev = new AnalyticsEvent
            {
                eventName = eventName,
                timestamp = DateTime.UtcNow.ToString("o"),
                parameters = new Dictionary<string, string>()
            };
            
            _eventLog.Add(ev);
            Debug.Log($"[Analytics] {eventName}");
        }
        
        /// <summary>
        /// Event loglar (tek parametre).
        /// </summary>
        public void LogEvent(string eventName, string paramKey, string paramValue)
        {
            if (!_isEnabled) return;
            
            var ev = new AnalyticsEvent
            {
                eventName = eventName,
                timestamp = DateTime.UtcNow.ToString("o"),
                parameters = new Dictionary<string, string> { { paramKey, paramValue } }
            };
            
            _eventLog.Add(ev);
            Debug.Log($"[Analytics] {eventName} ({paramKey}={paramValue})");
        }
        
        /// <summary>
        /// Event loglar (çoklu parametre).
        /// </summary>
        public void LogEvent(string eventName, Dictionary<string, string> parameters)
        {
            if (!_isEnabled) return;
            
            var ev = new AnalyticsEvent
            {
                eventName = eventName,
                timestamp = DateTime.UtcNow.ToString("o"),
                parameters = new Dictionary<string, string>(parameters)
            };
            
            _eventLog.Add(ev);
            
            string paramStr = "";
            foreach (var kvp in parameters)
            {
                paramStr += $" {kvp.Key}={kvp.Value}";
            }
            Debug.Log($"[Analytics] {eventName}{paramStr}");
        }
        
        /// <summary>
        /// Analytics'i aktif/pasif yapar.
        /// </summary>
        public void SetEnabled(bool enabled)
        {
            _isEnabled = enabled;
        }
        
        /// <summary>
        /// Tüm kayıtlı eventleri döner (debug için).
        /// </summary>
        public List<AnalyticsEvent> GetEventLog() => _eventLog;
        
        /// <summary>
        /// Event logunu JSON string olarak döner.
        /// </summary>
        public string GetEventLogJson()
        {
            return JsonUtility.ToJson(new AnalyticsEventLog { events = _eventLog });
        }
        
        /// <summary>
        /// Event logunu temizler.
        /// </summary>
        public void ClearLog()
        {
            _eventLog.Clear();
        }
    }
    
    /// <summary>
    /// Tek analytics event verisi.
    /// </summary>
    [System.Serializable]
    public class AnalyticsEvent
    {
        public string eventName;
        public string timestamp;
        public Dictionary<string, string> parameters;
    }
    
    /// <summary>
    /// Analytics event listesi (JSON serialize için).
    /// </summary>
    [System.Serializable]
    public class AnalyticsEventLog
    {
        public List<AnalyticsEvent> events;
    }
}
