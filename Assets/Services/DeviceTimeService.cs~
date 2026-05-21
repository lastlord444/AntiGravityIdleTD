using System;
using UnityEngine;

namespace AntiGravityTD.Services
{
    /// <summary>
    /// Cihaz zamanını kullanan implementasyon.
    /// Offline progression için kullanılır.
    /// </summary>
    public class DeviceTimeService : ITimeService
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        public long GetUtcNowTicks()
        {
            return DateTime.UtcNow.Ticks;
        }

        public double GetElapsedSeconds(long fromTicks)
        {
            var now = DateTime.UtcNow.Ticks;
            var elapsed = TimeSpan.FromTicks(now - fromTicks);
            return elapsed.TotalSeconds;
        }

        public string GetSourceType()
        {
            return "DeviceTime";
        }

        /// <summary>
        /// Constructor'da log at
        /// </summary>
        public DeviceTimeService()
        {
            Debug.Log("[DeviceTimeService] Device time servisi başlatıldı.");
            Debug.Log($"[DeviceTimeService] Mevcut UTC zaman: {GetUtcNow()}");
        }
    }
}
