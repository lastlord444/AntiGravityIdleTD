using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace AntiGravityTD.Gameplay.Placement
{
    /// <summary>
    /// Kule yerleştirme girdi köprüsü.
    /// Seçili bir TowerPlacementPoint ile TowerPlacementController arasında aracılık eder.
    /// Doğrudan mobil input işlemez — gelecekte bir sahne bileşeni veya UI tarafından çağrılır.
    /// Opsiyonel debug klavye girişi (varsayılan kapalı) smoke test için vardır.
    /// Input System ve legacy Input Manager'ı koşullu derleme ile destekler.
    /// </summary>
    public class TowerPlacementInputHandler : MonoBehaviour
    {
        [Header("Bağımlılıklar")]
        [SerializeField, Tooltip("Yerleştirme kontrolcüsü. Boşsa FindFirstObjectByType ile aranır.")]
        private TowerPlacementController placementController;

        [SerializeField, Tooltip("Seçili yerleştirme noktası. Inspector veya SetSelectedPoint ile atanır.")]
        private TowerPlacementPoint selectedPoint;

        [Header("Debug Ayarları")]
        [SerializeField, Tooltip("True ise Start'ta otomatik olarak selectedPoint'e yerleştirme dener (smoke test).")]
        private bool placeOnStart = false;

        [SerializeField, Tooltip("True ise belirtilen tuş ile debug yerleştirme etkinleşir.")]
        private bool allowKeyboardDebugInput = false;

        [SerializeField, Tooltip("Debug yerleştirme tetikleme tuşu.")]
        private KeyCode debugPlaceKey = KeyCode.P;

        [Header("Feedback (Opsiyonel)")]
        [SerializeField, Tooltip("Yerleştirme sonucunu otomatik olarak iletmek için feedback state.")]
        private TowerPlacementFeedbackState feedbackState;

        private bool controllerResolved;

        private void Start()
        {
            ResolveController();

            if (placeOnStart && selectedPoint != null)
            {
                var result = TryPlaceSelected();
                Debug.Log($"[TowerPlacementInputHandler] placeOnStart sonucu: {result.FailureReason}");
            }
        }

        private void Update()
        {
            if (!allowKeyboardDebugInput) return;

            if (WasDebugPlaceKeyPressed())
            {
                var result = TryPlaceSelected();
                Debug.Log($"[TowerPlacementInputHandler] Debug tuş ({debugPlaceKey}) sonucu: " +
                          (result.Success ? "Başarılı" : $"Başarısız ({result.FailureReason})"));
            }
        }

        /// <summary>
        /// Seçili yerleştirme noktasını ayarlar.
        /// </summary>
        /// <param name="point">Yeni seçili nokta (null olabilir).</param>
        public void SetSelectedPoint(TowerPlacementPoint point)
        {
            selectedPoint = point;
        }

        /// <summary>
        /// Mevcut seçili noktaya kule yerleştirmeyi dener.
        /// Seçili nokta yoksa MissingPlacementPoint ile başarısız döner.
        /// </summary>
        /// <returns>Yerleştirme sonucu.</returns>
        public TowerPlacementResult TryPlaceSelected()
        {
            return TryPlaceAt(selectedPoint);
        }

        /// <summary>
        /// Belirtilen noktaya kule yerleştirmeyi dener.
        /// Controller yoksa MissingPlacementController ile başarısız döner.
        /// </summary>
        /// <param name="point">Kule yerleştirilecek nokta.</param>
        /// <returns>Yerleştirme sonucu.</returns>
        public TowerPlacementResult TryPlaceAt(TowerPlacementPoint point)
        {
            // Lazy resolve
            if (placementController == null && !controllerResolved)
            {
                ResolveController();
            }

            if (placementController == null)
            {
                Debug.LogWarning("[TowerPlacementInputHandler] TowerPlacementController bulunamadı.");
                var failResult = TowerPlacementResult.Fail(TowerPlacementFailureReason.MissingPlacementController);
                PushFeedback(failResult);
                return failResult;
            }

            var result = placementController.TryPlaceTower(point);
            PushFeedback(result);
            return result;
        }

        /// <summary>
        /// Controller referansını çözümler.
        /// </summary>
        private void ResolveController()
        {
            if (placementController != null)
            {
                controllerResolved = true;
                return;
            }

            placementController = FindFirstObjectByType<TowerPlacementController>();
            controllerResolved = true;
        }

        /// <summary>
        /// Sonucu feedback state'e iletir (atanmışsa).
        /// </summary>
        private void PushFeedback(TowerPlacementResult result)
        {
            if (feedbackState == null) return;
            feedbackState.SetResult(result);
        }

        /// <summary>
        /// Debug yerleştirme tuşunun bu karede basılıp basılmadığını kontrol eder.
        /// Input System ve legacy Input Manager'ı koşullu derleme ile destekler.
        /// </summary>
        /// <returns>Tuş bu karede basıldıysa true.</returns>
        private bool WasDebugPlaceKeyPressed()
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            if (keyboard == null) return false;

            if (System.Enum.TryParse<Key>(debugPlaceKey.ToString(), true, out var inputSystemKey)
                && inputSystemKey != Key.None)
            {
                var control = keyboard[inputSystemKey];
                return control != null && control.wasPressedThisFrame;
            }

            return false;
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(debugPlaceKey);
#else
            return false;
#endif
        }
    }
}
