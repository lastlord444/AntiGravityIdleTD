# BlockForge MVP — AntiGravity

> Klasik 10x10 blok yerleştirme oyununu, her satır/sütun temizlemenin "atölyede üretim" tetiklediği **inşa/metası olan** bir F2P'ye çevir.

## Proje Hedefi

BlockForge, block puzzle core loop'una atölye/üretim metası ekleyen bir mobil F2P oyundur. Oyuncu 10x10 grid'e şekil yerleştirir, satır/sütun temizler, temizlenen hat enerji üretir, enerji makineleri tetikler, makineler parça/ürün üretir, kontratlar teslim edilir.

## Teknik Detaylar

| Özellik | Değer |
|---|---|
| **Motor** | Unity 6.3 LTS (6000.3.8f1) |
| **Render Pipeline** | Universal Render Pipeline (URP) 2D |
| **Dil** | C# |
| **Platform** | Mobile (Android) |
| **UI** | UGUI (Canvas) + TextMeshPro |
| **Save** | JSON + persistentDataPath |

## Projeyi Açma

1. **Unity Hub'ı açın** ve "Open" butonuna tıklayın
2. `D:\AntiGravityIdleTD\AntiGravityIdleTD` klasörünü seçin
3. Unity 6.3 LTS ile projeyi açın
4. İlk açılışta Library yeniden oluşturulacaktır
5. **Build Settings** → `Assets/_Project/Scenes/Boot`, `Home`, `Run` sahnelerini ekleyin

## Proje Yapısı (BlockForge MVP)

```
Assets/
  _Project/
    Scripts/
      Core/           # Services, GameBootstrapper, SceneLoader
      Gameplay/        # GridManager, ShapeSO, ShapeGenerator, RunManager, LineClearSystem, PlacementPreview, ShapeDragController, CellView, BoosterSO
      Meta/            # InventoryModel, MachineSO, MachineModel, ProductionSystem, ContractSO, ContractModel, ContractSystem, ItemSO
      UI/              # HomeUIController
      Monetization/    # AdsService, IapService
      Analytics/       # AnalyticsService
      Save/            # SaveModel, SaveService
      AudioVfx/        # AudioService, SfxLibrarySO
    Prefabs/
      UI/
      Gameplay/
      Meta/
    ScriptableObjects/
      Items/
      Machines/
      Contracts/
      Shapes/
      Boosters/
    Scenes/
      Boot.unity       # Init, service locator, save load → Home
      Home.unity       # Atölye ekranı, kontratlar, upgrade
      Run.unity        # 10x10 grid + shapes + run UI
    Art/
    Audio/
    Resources/
```

## Oyun Döngüleri

### Core Loop (Run)
1. Oyuncu 3 şekilden birini seçer
2. Grid'e yerleştirir
3. Satır/sütun dolarsa temizlenir
4. Temizlenen hat = Üretim Enerjisi
5. Makineler tetiklenir → ürün üretir
6. Kontrat ilerler → teslim → ödül
7. Shape kalmazsa run biter
8. Rewarded Continue: +1 hamle

### Meta Loop (Atölye)
Kontrat seç → üret → teslim → coin + upgrade materyali → makineleri güçlendir → daha hızlı kontrat

## MVP Durumu

- [x] Grid + placement + line clear scriptleri
- [x] Shape generator + "always placeable" garantisi
- [x] Run manager + score + game over + continue
- [x] Inventory model
- [x] Production system (line clear → inventory outputs)
- [x] Contract system (deliver)
- [x] Save/Load (JSON)
- [x] Rewarded continue (placeholder)
- [x] Analytics (Debug.Log)
- [x] Audio service (placeholder)
- [ ] Sahne GameObjectleri kurulumu (prefab + sahne düzeni)
- [ ] Shape ScriptableObject dataları (20+ shape)
- [ ] Machine/Item/Contract ScriptableObject dataları
- [ ] UI prefab'ları oluşturma
- [ ] SFX + basic VFX
- [ ] Tutorial (3 adım)

## Lisans

Bu proje özel bir lisans altındadır. Tüm hakları saklıdır.

---
_Son güncelleme: 2026-02-15_
