using System;

namespace AntiGravityTD.Services
{
    /// <summary>
    /// Zaman erişimi için arayüz.
    /// Test edilebilirlik ve birden fazla zaman kaynağı için.
    /// </summary>
    public interface ITimeService
    {
        /// <summary>
        /// Şu anki UTC zamanı döner.
        /// </summary>
        DateTime GetUtcNow();

        /// <summary>
        /// Timestamp (ticks) döner.
        /// </summary>
        long GetUtcNowTicks();

        /// <summary>
        /// İki timestamp arasındaki farkı saniye cinsinden döner.
        /// </summary>
        double GetElapsedSeconds(long fromTicks);

        /// <summary>
        /// Zaman kaynağının tipi (debug için).
        /// </summary>
        string GetSourceType();
    }
}
