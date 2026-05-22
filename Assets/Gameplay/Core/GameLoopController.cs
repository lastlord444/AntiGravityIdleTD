using System;
using UnityEngine;
using AntiGravityTD.Gameplay.Enemies;
using AntiGravityTD.Gameplay.Waves;

namespace AntiGravityTD.Gameplay.Core
{
    /// <summary>
    /// Oyun akış yöneticisi.
    /// WaveController ile koordineli çalışarak oyun durumunu (GameState) yönetir.
    /// Win/Lose koşullarını dinler ve state geçişlerini bildirir.
    /// </summary>
    public class GameLoopController : MonoBehaviour
    {
        [Header("Dalga Kontrolcüsü")]
        [SerializeField] private WaveController waveController;
        [SerializeField] private bool startAutomatically = false;

        private GameState currentState = GameState.NotStarted;

        /// <summary>Oyunun mevcut durumu.</summary>
        public GameState CurrentState => currentState;

        /// <summary>Oyun durumu değiştiğinde tetiklenir.</summary>
        public event Action<GameState> OnGameStateChanged;

        private void Start()
        {
            if (startAutomatically)
            {
                StartGame();
            }
        }

        private void OnEnable()
        {
            // Wave controller event'lerini dinle
            if (waveController != null)
            {
                waveController.OnAllWavesCompleted += HandleAllWavesCompleted;
            }

            // Düşman base'e ulaştığında lose koşulunu dinle
            EnemyMover.OnAnyEnemyReachedEnd += HandleEnemyReachedEnd;
        }

        private void OnDisable()
        {
            // Event aboneliklerini temizle (leak önleme)
            if (waveController != null)
            {
                waveController.OnAllWavesCompleted -= HandleAllWavesCompleted;
            }

            EnemyMover.OnAnyEnemyReachedEnd -= HandleEnemyReachedEnd;
        }

        /// <summary>
        /// Oyunu başlatır. State → Playing, dalgaları başlatır.
        /// </summary>
        public void StartGame()
        {
            if (currentState != GameState.NotStarted)
            {
                Debug.LogWarning($"[GameLoopController] Oyun zaten başlatılmış. Mevcut durum: {currentState}");
                return;
            }

            if (waveController == null)
            {
                Debug.LogError("[GameLoopController] WaveController referansı atanmamış!");
                return;
            }

            SetState(GameState.Playing);
            waveController.StartWaves();
            Debug.Log("[GameLoopController] Oyun başladı!");
        }

        /// <summary>
        /// Oyunu kazanılmış olarak işaretler. Sadece Playing durumunda çalışır.
        /// </summary>
        public void TriggerWin()
        {
            if (currentState != GameState.Playing)
            {
                Debug.LogWarning($"[GameLoopController] Win tetiklenemez. Mevcut durum: {currentState}");
                return;
            }

            SetState(GameState.Won);
            Debug.Log("[GameLoopController] Oyun kazanıldı!");
        }

        /// <summary>
        /// Oyunu kaybedilmiş olarak işaretler. Sadece Playing durumunda çalışır.
        /// </summary>
        public void TriggerLose()
        {
            if (currentState != GameState.Playing)
            {
                Debug.LogWarning($"[GameLoopController] Lose tetiklenemez. Mevcut durum: {currentState}");
                return;
            }

            SetState(GameState.Lost);
            if (waveController != null)
            {
                waveController.StopWaves();
            }
            Debug.Log("[GameLoopController] Oyun kaybedildi!");
        }

        private void SetState(GameState newState)
        {
            if (currentState == newState) return;

            GameState previousState = currentState;
            currentState = newState;
            Debug.Log($"[GameLoopController] State değişti: {previousState} → {newState}");
            OnGameStateChanged?.Invoke(newState);
        }

        private void HandleAllWavesCompleted()
        {
            Debug.Log("[GameLoopController] Tüm dalgalar tamamlandı — kazanma koşulu sağlandı.");
            TriggerWin();
        }

        private void HandleEnemyReachedEnd(EnemyMover enemy)
        {
            Debug.Log($"[GameLoopController] Düşman base'e ulaştı: {enemy.gameObject.name} — kaybetme koşulu sağlandı.");
            TriggerLose();
        }
    }
}
