# Skill: Reference Repo Reviewer

## Purpose
Referans Unity tower defense repolarını incelemek, kullanılabilir pattern’leri çıkarmak ve lisans/scope riskini engellemek.

## Inputs
- Repo URL
- Kullanım amacı
- Mevcut AntiGravityIdleTD mimarisi
- Lisans bilgisi

## Process
1. LICENSE dosyasını kontrol et.
2. Unity versiyonunu kontrol et.
3. Son commit tarihini kontrol et.
4. Asset lisanslarını kontrol et.
5. Kodun hangi pattern için inceleneceğini yaz.
6. Direkt kopya mı, seçmeli pattern mi, sadece okuma mı karar ver.
7. AntiGravityIdleTD mimarisiyle çelişen kısımları yaz.

## Allowed Output
- Pattern summary
- Risk summary
- “Use / Study / Avoid” kararı
- Uygulanacaksa küçük dosya değişiklik planı

## Forbidden
- Lisans kontrol etmeden kod almak
- GPL kod almak
- Referans repoyu base yapmak
- DrFlower/GameFramework gibi ağır framework’ü projeye sokmak
- Asset klasörü kopyalamak
