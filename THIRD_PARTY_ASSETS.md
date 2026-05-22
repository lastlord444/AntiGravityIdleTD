# Third Party Assets

Bu dosya, projeye dahil edilen tüm harici görselleri, sesleri, 3D/2D modelleri ve lisanslı kod uyarlamalarını kayıt altında tutmak amacıyla kullanılır.

## Şablon Kayıt Formatı
Harici bir varlık eklendiğinde bu dosyanın altına aşağıdaki formatta kayıt eklenmelidir:

```markdown
### [Varlık Adı / Kaynak]
- **Kaynak URL**: [Harici repodaki veya web sitesindeki URL]
- **Lisans Türü**: [Unlicense, CC0, MIT vb.]
- **İçe Aktarılan Dosya Yolları**: 
  - `Assets/Content/...` (Sprite veya ses dosyaları)
  - `Assets/Gameplay/...` (Eğer kod uyarlaması ise ilgili C# dosyası)
- **Eklenme Tarihi**: YYYY-MM-DD
- **Açıklama/Kullanım Amacı**: [Neden ve nerede kullanıldığı bilgisi]
```

---

## Aktif Kayıtlar

### Kenney Tower Defense (Top-Down)
- **Kaynak URL**: https://kenney.nl/assets/tower-defense-top-down
- **Lisans Türü**: Creative Commons CC0
- **İçe Aktarılan Dosya Yolları**: 
  - `Assets/Content/Art/Kenney/TowerDefenseTopDown/towerDefense_tilesheet.png`
  - `Assets/Content/Art/Kenney/TowerDefenseTopDown/towerDefense_tilesheet.png.meta`
- **Eklenme Tarihi**: 2026-05-22
- **Açıklama/Kullanım Amacı**: Prototip kule (tower), düşman (enemy), mermi (projectile) ve harita/yol (map/path) spriteları. İlk dikey kesit (vertical slice) görselleştirmesinde kullanılacaktır.

