# Teknik Kararlar ve Justifikasyonlar

## Mimari Kararlar

### Service Locator Pattern
**Karar:** Service Locator pattern kullanımı  
**Nedeni:** 
- Dependency Injection framework'ünden daha basit
- Unity ile doğal entegrasyon
- Test edilebilir ve modüler

**Alternatifler:** Zenject/Extenject DI framework  
**Reddedilme nedeni:** Küçük-orta ölçekli proje için overkill

---

### Event Channel (ScriptableObject) Pattern
**Karar:** SO tabanlı event sistemi  
**Nedeni:**
- Loose coupling
- Inspector'dan configure edilebilir
- Unity serialization ile uyumlu

**Alternatifler:** C# Events, Message Bus  
**Reddedilme nedeni:** SO pattern Unity workflow'una daha uygun

---

### Save System - JSON
**Karar:** JSON serialization ile local save  
**Nedeni:**
- Human-readable
- Debug kolaylığı
- PlayerPrefs'ten daha esnek

**Alternatifler:** Binary, SQLite, Cloud Save  
**Reddedilme nedeni:** 
- Binary: Debug zorluğu
- SQLite: Overkill
- Cloud: Offline-first tasarım gereği lokal öncelik

---

### Time Service - Device Time
**Karar:** Device time kullanımı (server sync yok)  
**Nedeni:**
- Offline-first tasarım
- Sunucu maliyeti yok
- Basit implementasyon

**Risk:** Saat değiştirme exploiti  
**Mitigasyon:** İleride server validation eklenebilir

---

### Branch Strategy
**Karar:** main (protected) + develop (active)  
**Nedeni:**
- Gitflow benzeri basit flow
- Main her zaman stable
- PR review süreci

---

## Unity Özel Kararlar

### URP 2D
**Karar:** Universal Render Pipeline 2D kullanımı  
**Nedeni:**
- 2D oyun için optimize
- Lighting ve shader desteği
- Forward compatible

---

### Unity 6.3 LTS
**Karar:** LTS versiyonu seçimi  
**Nedeni:**
- Long-term support
- Stability garantisi
- Production-ready

---

_Yeni kararlar eklendikçe bu doküman güncellenecektir._
