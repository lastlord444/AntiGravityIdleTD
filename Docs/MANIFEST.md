# AntiGravityIdleTD - Proje Manifestosu (MANIFEST.md)

## A) Mevcut Durum ve Temel Bilgiler (Current Truth)
* **Repository:** `lastlord444/AntiGravityIdleTD`
* **Ana Ürün:** Android-first Micro TD / Idle Lane Defense
* **Oyun Motoru:** Unity 6.3 LTS / URP 2D
* **Mimari Yapı:** ServiceLocator + ScriptableObject EventChannel + JSON yerel kayıt sistemi (local save) + DeviceTimeService (Uzun vadeli mimari hedef olarak; şu an ilk vertical slice kapsamında ertelenmiştir.)
* **Aktif Geliştirme Branch'i:** `develop`
* **Geliştirme Akışı:** `develop` -> `feature/*` -> PR -> `develop` (develop veya main branch'lerine doğrudan push yapılmaz, her iş için bir feature branch ve PR zorunludur.)

---

## B) Güncel Birleştirilen PR Durumu (Current Merged PR State)
* **PR #18:** Kule yerleştirme (placement) ve altın harcama (gold spend) temeli
* **PR #19:** Yerleştirme girdisi (placement input) ve geri bildirim (feedback) kod temeli
* **PR #20:** Sadece sahne üzerinde yerleştirme kablolaması (scene-only placement wiring)
* **PR #21:** Input System debug yerleştirme girdi düzeltmesi (Input System debug placement fix)
* **PR #22:** Kod iyileştirmesi ve dayanıklılık çalışması (code: harden first vertical slice playability) - *Açık, doküman uyumsuzluğu nedeniyle bekletilmektedir.*

---

## C) İlk Dikey Kesit Kapsamı (First Vertical Slice Scope)
Projenin ilk aşamasında hedeflenen oynanabilir dikey kesit özellikleri şunlardır:
* 1 lane / yol (waypoint path) ve 1 harita
* Düşman hareketi ve yapay zeka (enemy movement & AI)
* Kule hedefleme mekaniği (tower targeting - en yakın canlı düşmana odaklanma)
* Mermi ve hasar sistemi (projectile & damage)
* 3–5 dalgalık temel oyun döngüsü (waves)
* Altın ekonomisi (gold economy)
* Ana üs can değeri (base HP)
* Kazanma / kaybetme durumları (win / lose)
* Temel arayüz (basic UI)
* Kule yerleştirme mekaniği (tower placement)
* Android hata ayıklama sürümü (Android debug build)

---

## D) Ertelenen / Kapsam Dışı Özellikler (Deferred / Out of Scope)
İlk dikey kesit kapsamında yer almayan ve daha sonra değerlendirilecek özellikler:
* Anti-gravity mekanikleri (tam entegrasyon)
* Idle ilerleme ve offline altın kazanma mekaniği (idle progression & offline save/load)
* Parasal araçlar (monetization / IAP / AdMob)
* Gerçek harici SDK entegrasyonları (Real Firebase / Real AdMob)
* Bulut kayıt sistemi (cloud save)
* Çok oyunculu mod (multiplayer)
* Gelişmiş meta ekonomi ve yükseltmeler
* İleri düzey grafik, ses ve mobil UX cilalaması

---

## E) PR Disiplini ve Geliştirme Kuralları (PR Discipline)
* **Değişiklik Ayrımı:** Sadece kod (CODE-ONLY) ve sadece sahne (SCENE-ONLY) değişiklikleri ayrı PR'lar olarak gönderilmelidir. Oynanış kodları ile Unity sahne/prefab değişiklikleri aynı PR'da birleştirilmez.
* **Doküman PR'ları:** Sadece dokümantasyon (DOCS-ONLY) içeren PR'larda kod veya görsel varlık değişikliği bulunmamalıdır.
* **Proje Ayarları:** `ProjectSettings` ve `Packages` klasöründeki dosyalar yalnızca görevin kapsamı bunu açıkça gerektirdiğinde değiştirilir.
* **Gereksiz Dosyalar:** `Library/`, `Temp/`, `UserSettings/` gibi otomatik üretilen dosyalar kesinlikle repoya commit edilmez.
* **Lisans ve Üçüncü Parti:** Lisansı belirsiz hiçbir asset veya harici kod projeye dahil edilemez. GPL veya benzeri kısıtlayıcı copyleft lisansına sahip kodlar kullanılamaz. Referans repolardaki kodlar doğrudan kopyalanamaz, sadece mimari fikir edinmek için incelenebilir.

---

## F) Mevcut Engelleyicinin Çözümü (Current Blocker Resolution)
* **PR #22 Engeli:** PR #22, `Docs/MANIFEST.md` dosyasındaki eski milestone planlamasının (Save/Load, Time Service, Offline Progression vb.) güncel dikey kesit truth (kapsamı) ile çelişmesi (dokümantasyon drifti) nedeniyle beklemeye alınmıştı.
* **Çözüm:** Bu PR (#23 - docs-sync-manifest) aracılığıyla `MANIFEST.md` belgesi `AGENTS.md` ve `CLAUDE.md` ile tam uyumlu hale getirilerek aradaki çelişki (drift) çözülmüştür.
* **Sonraki Adım:** Bu dokümantasyon PR'ı merge edildikten sonra, PR #22'nin kod, derleme ve çalışma zamanı (runtime) testleri yeniden doğrulanarak merge edilmesinin önünde bir engel kalmayacaktır.

_Son güncelleme: 2026-05-23_
