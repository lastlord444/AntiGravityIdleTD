# Reference Repos

Bu projede referans repolar doğrudan base/proje temeli olarak kullanılmaz. Ancak lisansı uygun olan açık kaynaklı projelerin kodları, mimari yaklaşımları ve mobil pratikleri incelenip kendi projemizin yapısına uyarlanabilir.

## Revize Edilmiş Referans Repo Politikası
1. **Lisans Uyumluluğu**: Yalnızca lisansı uyumlu (Unlicense, MIT, CC0, Apache 2.0 vb.) açık kaynak projeler incelenebilir ve kod uyarlaması yapılabilir.
2. **Copyleft Yasağı**: GPL veya copyleft lisanslı kodlar kesinlikle projeye dahil edilmeyecektir. Lisansı belirsiz/bulunmayan repolardan kod kopyalanmayacaktır.
3. **Kör İçe Aktarma Yasağı (No Blind Transplant)**: Referans repolardan `ProjectSettings`, `Packages` klasörleri veya doğrudan proje dosyaları körü körüne kopyalanmayacaktır.
4. **Bağımsız PR'lar**: Kod değişiklikleri, asset import işlemleri, sahne değişiklikleri ve dökümantasyon güncellemeleri her zaman ayrı PR'larda tutulacaktır.
5. **Kayıt Tutma**: Projeye uyarlanan her kod parçası veya import edilen asset `THIRD_PARTY_ASSETS.md` dosyasına lisansı ve kaynak URL'i ile işlenecektir.

---

## 1. Brackeys/Tower-Defense-Tutorial
- **Rol**: Core TD Loop ve Oynanış Kaynağı.
- **Lisans**: Unlicense / Public Domain.
- **İncelenecek ve Uyarlanacak Yapılar**:
  - **Wave Spawner**: Düşman dalgalarının oluşturulması ve zamanlaması.
  - **Node/Build Placement**: Haritaya kule inşa etme noktaları ve inşa/satış mantığı.
  - **Player Stats**: Altın ekonomisi ve ana üs canının (Base HP) global yönetimi.
  - **Turret Rotation**: Kule kafasının hedefe yumuşak şekilde dönmesi.
- **Yasaklar**: Eski 3D sahneler veya projeye ait proje ayarları içe aktarılmaz.

## 2. DrFlower/TowerDefense-GameFramework-Demo
- **Rol**: İleri Düzey Mimari & Tasarım Taslağı.
- **Lisans**: MIT.
- **İncelenecek Tasarım Fikirleri**:
  - **Data-Driven Config**: Düşman ve kule dengelerinin ScriptableObject ile harici veri odaklı yapılması.
  - **Object Pooling**: Mermi ve düşman instantiate performans optimizasyonu.
- **Yasaklar**: Ağır "GameFramework" altyapısı projeye eklenmez, sadece fikirlerinden esinlenilir.

## 3. davda54/tower-defense-unity
- **Rol**: Android Mobil & Kenney Asset Kullanım Pratikleri.
- **Lisans**: Lisans doğrulanmadan kod kopyalanamaz, sadece mobil kontrol ve Sprite Atlas kullanımı incelenir.

---

## 4. Görsel Kaynaklar (Art Assets)
- **Kenney CC0 Assets**: Prototip ve dikey kesit (vertical slice) görselleri için Kenney 2D Tower Defense ve UI paketleri onaylanmıştır. Her import işlemi ayrı bir PR'da yapılmalı ve `THIRD_PARTY_ASSETS.md` dosyasına kaydedilmelidir.

