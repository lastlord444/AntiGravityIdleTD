# AntiGravityIdleTD

> Anti-gravity mekanikleriyle zenginleştirilmiş idle tower defense oyunu.

## Proje Hedefi

AntiGravityIdleTD, strateji ve idle oyun mekaniklerini birleştiren bir mobil tower defense oyunudur. Yerçekimine meydan okuyan benzersiz kule yerleştirme mekanikleri ve offline ilerleme sistemi ile oyuncuya sürekli bir gelişim hissi sunar.

## Teknik Detaylar

| Özellik | Değer |
|---|---|
| **Motor** | Unity 6.3 LTS |
| **Render Pipeline** | Universal Render Pipeline (URP) 2D |
| **Dil** | C# |
| **Platform** | Mobile (Android / iOS) |
| **Min Unity Versiyon** | 6000.0.42f1 |

## Projeyi Açma

1. **Unity Hub'ı açın** ve "Open" butonuna tıklayın
2. `D:\AntiGravityIdleTD\AntiGravityIdleTD` klasörünü seçin
3. Unity 6.3 LTS ile projeyi açın
4. İlk açılışta Library yeniden oluşturulacaktır (birkaç dakika sürebilir)
5. `Assets/Scenes/Main.unity` sahnesini açın

## Branch Akışı

```
main (korumalı - direkt push yok)
  └── develop (aktif geliştirme)
        └── feature/xxx (özellik branch'leri)
```

- **main:** Her zaman stabil, deploy edilebilir durum
- **develop:** Aktif geliştirme branch'i. Tüm feature branch'leri buradan açılır
- **feature/xxx:** Yeni özellikler için kısa ömürlü branch'ler
- **main'e merge:** Sadece develop'tan PR ile

## Proje Yapısı

```
AntiGravityIdleTD/
├── Assets/
│   ├── Core/                    # Temel altyapı
│   │   ├── ServiceLocator/      # Global service registry
│   │   ├── EventBus/            # SO tabanlı event channel sistemi
│   │   └── Save/                # Save schema POCO
│   ├── Services/                # Service implementasyonları
│   │   ├── ISaveService.cs      # Save interface
│   │   ├── JsonSaveService.cs   # JSON save implementasyonu
│   │   ├── ITimeService.cs      # Time interface
│   │   └── DeviceTimeService.cs # Device time implementasyonu
│   ├── Gameplay/                # Oyun mekanikleri (TBD)
│   ├── UI/                      # Kullanıcı arayüzü (TBD)
│   ├── Config/                  # Konfigürasyon (TBD)
│   ├── Content/                 # İçerik / Asset'ler (TBD)
│   └── Scenes/
│       └── Main.unity           # Ana sahne
├── Docs/
│   ├── MANIFEST.md              # Proje manifestosu
│   ├── TUNABLES.md              # Oyun dengesi parametreleri
│   ├── DECISIONS.md             # Teknik kararlar
│   └── PLAYTEST_01.md           # Playtest raporu template
├── Packages/
├── ProjectSettings/
└── README.md
```

## Issue #0 - Bootstrap Checklist

- [x] Git init + remote origin eklendi
- [x] `develop` branch oluşturuldu
- [x] Unity `.gitignore` eklendi (Library, Temp, Logs, UserSettings, obj dahil)
- [x] Docs klasörü oluşturuldu (MANIFEST, TUNABLES, DECISIONS, PLAYTEST_01)
- [x] Assets alt klasörleri oluşturuldu (Core, Services, Gameplay, UI, Config, Content)
- [x] `ServiceLocator.cs` — Global service registry
- [x] `EventChannel.cs` — SO tabanlı generic ve void event channel
- [x] `SaveSchema.cs` — POCO save data, schema_version=1
- [x] `ISaveService.cs` + `JsonSaveService.cs` — Async JSON save/load
- [x] `ITimeService.cs` + `DeviceTimeService.cs` — UTC time service
- [x] SampleScene → Main.unity olarak kopyalandı ve Build Settings güncellendi
- [x] README.md oluşturuldu
- [x] Commit: `chore: bootstrap unity project + core skeleton (Issue #0)`
- [x] Push: develop branch'e pushlandı

## Lisans

Bu proje özel bir lisans altındadır. Tüm hakları saklıdır.

---
_Oluşturulma tarihi: 2026-02-13_
