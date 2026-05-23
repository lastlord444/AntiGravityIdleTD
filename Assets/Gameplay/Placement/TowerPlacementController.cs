using UnityEngine;
using AntiGravityTD.Core;
using AntiGravityTD.Gameplay.Economy;

namespace AntiGravityTD.Gameplay.Placement
{
    /// <summary>
    /// Kule yerleştirme sürecini yöneten kontrolcü.
    /// Validasyon, altın harcama, instantiate ve rollback mantığını barındırır.
    /// UI veya input işlemez — dışarıdan TryPlaceTower çağrılır.
    /// </summary>
    public class TowerPlacementController : MonoBehaviour
    {
        [Header("Kule Ayarları")]
        [SerializeField, Tooltip("Yerleştirilecek kule prefab'ı.")]
        private GameObject towerPrefab;

        [SerializeField, Tooltip("Kule yerleştirme maliyeti (altın).")]
        private int towerCost = 50;

        [SerializeField, Tooltip("Opsiyonel: Kulelerin parent'ı olacak Transform.")]
        private Transform towerParent;

        [Header("Bağımlılıklar (Opsiyonel)")]
        [SerializeField, Tooltip("GoldWallet referansı. Boşsa ServiceLocator veya FindFirstObjectByType ile aranır.")]
        private GoldWallet goldWallet;

        [Header("Event Kanalları (Opsiyonel)")]
        [SerializeField, Tooltip("Yerleştirme tamamlandığında tetiklenen event kanalı.")]
        private TowerPlacementCompletedEventChannel placementCompletedChannel;

        /// <summary>
        /// Kule yerleştirme maliyetini döner (UI ve diğer sistemler için).
        /// </summary>
        public int TowerCost => towerCost;

        private void Awake()
        {
            ResolveGoldWallet();
        }

        /// <summary>
        /// Belirtilen noktaya kule yerleştirmeyi dener.
        /// Tüm validasyonları sırayla kontrol eder.
        /// Altın yalnızca tüm kontroller ve instantiate başarılı olursa harcanır.
        /// </summary>
        /// <param name="point">Kule yerleştirilecek nokta.</param>
        /// <returns>Yerleştirme sonucu.</returns>
        public TowerPlacementResult TryPlaceTower(TowerPlacementPoint point)
        {
            // 1. Tower prefab kontrolü
            if (towerPrefab == null)
            {
                Debug.LogWarning("[TowerPlacementController] Tower prefab atanmamış.");
                var failResult = TowerPlacementResult.Fail(TowerPlacementFailureReason.MissingTowerPrefab);
                RaisePlacementEvent(failResult, point);
                return failResult;
            }

            // 2. Placement point kontrolü
            if (point == null)
            {
                Debug.LogWarning("[TowerPlacementController] Placement point null.");
                var failResult = TowerPlacementResult.Fail(TowerPlacementFailureReason.MissingPlacementPoint);
                RaisePlacementEvent(failResult, null);
                return failResult;
            }

            // 3. Occupied kontrolü
            if (point.IsOccupied)
            {
                Debug.LogWarning($"[TowerPlacementController] Nokta zaten dolu: {point.gameObject.name}");
                var failResult = TowerPlacementResult.Fail(TowerPlacementFailureReason.PlacementPointOccupied);
                RaisePlacementEvent(failResult, point);
                return failResult;
            }

            // 4. GoldWallet kontrolü — lazy resolve dene
            if (goldWallet == null)
            {
                ResolveGoldWallet();
            }

            if (goldWallet == null)
            {
                Debug.LogWarning("[TowerPlacementController] GoldWallet bulunamadı.");
                var failResult = TowerPlacementResult.Fail(TowerPlacementFailureReason.MissingGoldWallet);
                RaisePlacementEvent(failResult, point);
                return failResult;
            }

            // 5. Altın yeterliliği ön kontrolü
            if (goldWallet.CurrentGold < towerCost)
            {
                Debug.LogWarning($"[TowerPlacementController] Altın yetersiz. Gereken: {towerCost}, Mevcut: {goldWallet.CurrentGold}");
                var failResult = TowerPlacementResult.Fail(TowerPlacementFailureReason.InsufficientGold);
                RaisePlacementEvent(failResult, point);
                return failResult;
            }

            // 6. Instantiate
            GameObject tower = InstantiateTower(point);
            if (tower == null)
            {
                Debug.LogError("[TowerPlacementController] Kule instantiate başarısız.");
                var failResult = TowerPlacementResult.Fail(TowerPlacementFailureReason.InstantiateFailed);
                RaisePlacementEvent(failResult, point);
                return failResult;
            }

            // 7. Altın harcama — instantiate sonrası
            bool spent = goldWallet.TrySpend(towerCost);
            if (!spent)
            {
                // Rollback: Kuleyi yok et, altın harcanamadı
                Debug.LogError("[TowerPlacementController] TrySpend beklenmedik şekilde başarısız! Kule rollback ediliyor.");
                Destroy(tower);
                var failResult = TowerPlacementResult.Fail(TowerPlacementFailureReason.InsufficientGold);
                RaisePlacementEvent(failResult, point);
                return failResult;
            }

            // 8. Noktayı dolu olarak işaretle
            point.MarkOccupied(tower);

            // 9. Başarılı sonuç
            int remainingGold = goldWallet.CurrentGold;
            Debug.Log($"[TowerPlacementController] Kule yerleştirildi: {point.gameObject.name}. Maliyet: {towerCost}, Kalan altın: {remainingGold}");

            var successResult = TowerPlacementResult.Succeed(tower, towerCost, remainingGold);
            RaisePlacementEvent(successResult, point);
            return successResult;
        }

        /// <summary>
        /// GoldWallet referansını çözümler.
        /// Önce ServiceLocator, yoksa FindFirstObjectByType kullanır.
        /// </summary>
        private void ResolveGoldWallet()
        {
            if (goldWallet != null) return;

            // ServiceLocator üzerinden dene
            if (ServiceLocator.Instance != null && ServiceLocator.Instance.IsRegistered<GoldWallet>())
            {
                goldWallet = ServiceLocator.Instance.Get<GoldWallet>();
            }

            // Hâlâ null ise scene'de ara
            if (goldWallet == null)
            {
                goldWallet = FindFirstObjectByType<GoldWallet>();
            }
        }

        /// <summary>
        /// Kuleyi placement point pozisyon ve rotasyonunda oluşturur.
        /// </summary>
        private GameObject InstantiateTower(TowerPlacementPoint point)
        {
            try
            {
                Transform spawnTransform = point.transform;
                GameObject tower = Instantiate(
                    towerPrefab,
                    spawnTransform.position,
                    spawnTransform.rotation,
                    towerParent
                );
                return tower;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[TowerPlacementController] Instantiate exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Yerleştirme event'ini yayınlar (kanal atanmışsa).
        /// </summary>
        private void RaisePlacementEvent(TowerPlacementResult result, TowerPlacementPoint point)
        {
            if (placementCompletedChannel == null) return;

            var payload = new TowerPlacementResultPayload
            {
                Success = result.Success,
                FailureReason = result.FailureReason,
                Cost = result.Cost,
                RemainingGold = result.RemainingGold,
                PlacementPoint = point,
                PlacedTower = result.PlacedTower
            };

            placementCompletedChannel.Raise(payload);
        }
    }
}
