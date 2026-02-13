using System;
using UnityEngine;

namespace AntiGravityTD.Core.Save
{
    /// <summary>
    /// Oyun save data şeması.
    /// JSON serialization için POCO (Plain Old C# Object).
    /// </summary>
    [Serializable]
    public class SaveSchema
    {
        /// <summary>
        /// Save schema versiyonu. İleride save migration için kritik.
        /// </summary>
        public int schemaVersion = 1;

        /// <summary>
        /// Save oluşturulma zamanı (UTC ticks).
        /// </summary>
        public long saveTimestamp;

        /// <summary>
        /// Son oynama zamanı (UTC ticks). Offline kazanç hesabı için.
        /// </summary>
        public long lastPlayTimestamp;

        /// <summary>
        /// Oyuncu altın miktarı.
        /// </summary>
        public long gold;

        /// <summary>
        /// Oyuncu seviyesi.
        /// </summary>
        public int playerLevel;

        /// <summary>
        /// Tamamlanan dalga numarası.
        /// </summary>
        public int currentWave;

        /// <summary>
        /// Placeholder: Kule verilerinin JSON array'i (gelecekte expand edilecek).
        /// </summary>
        public string towersData = "[]";

        /// <summary>
        /// Placeholder: Yükseltme verilerinin JSON array'i.
        /// </summary>
        public string upgradesData = "[]";

        /// <summary>
        /// Default constructor ile yeni save oluşturur.
        /// </summary>
        public SaveSchema()
        {
            saveTimestamp = DateTime.UtcNow.Ticks;
            lastPlayTimestamp = DateTime.UtcNow.Ticks;
            gold = 100; // Başlangıç altını (TUNABLES.md'den)
            playerLevel = 1;
            currentWave = 0;
        }

        /// <summary>
        /// lastPlayTimestamp'ı şu anki zamana günceller.
        /// </summary>
        public void UpdateLastPlayTime()
        {
            lastPlayTimestamp = DateTime.UtcNow.Ticks;
        }

        /// <summary>
        /// Son oynama zamanından bu yana geçen süreyi saniye cinsinden döner.
        /// </summary>
        public double GetTimeSinceLastPlaySeconds()
        {
            var now = DateTime.UtcNow.Ticks;
            var elapsed = TimeSpan.FromTicks(now - lastPlayTimestamp);
            return elapsed.TotalSeconds;
        }
    }
}
