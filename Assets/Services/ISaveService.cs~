using System;
using System.Threading.Tasks;
using AntiGravityTD.Core.Save;

namespace AntiGravityTD.Services
{
    /// <summary>
    /// Save/Load işlemi için arayüz.
    /// Test edilebilirlik için interface pattern kullanılır.
    /// </summary>
    public interface ISaveService
    {
        /// <summary>
        /// Asenkron save işlemi.
        /// </summary>
        Task<bool> SaveAsync(SaveSchema data);

        /// <summary>
        /// Asenkron load işlemi.
        /// </summary>
        Task<(bool success, SaveSchema data)> LoadAsync();

        /// <summary>
        /// Kayıtlı save verisi var mı kontrol eder.
        /// </summary>
        bool HasSaveData();

        /// <summary>
        /// Kayıtlı save verisini siler.
        /// </summary>
        Task<bool> DeleteSaveAsync();

        /// <summary>
        /// Save dosyası path'i (debug için).
        /// </summary>
        string GetSavePath();
    }
}
