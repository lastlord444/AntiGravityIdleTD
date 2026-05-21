# Skill: Release Gate

## Purpose
Her PR’ın merge edilebilir olup olmadığını kontrol eder.

## Required Checks
- Branch doğru mu?
- PR develop’a mı açılmış?
- Changed files görev kapsamıyla uyumlu mu?
- Unity scene/prefab değişiklikleri beklenen mi?
- Library, Temp, Logs, UserSettings commitlenmiş mi?
- .cs~ backup dosyası var mı?
- Asset lisansları belgeli mi?
- Build/test kanıtı var mı?
- README/Docs drift var mı?
- Scope creep var mı?

## Merge Decision
- PASS: PR merge edilebilir.
- BLOCK: Merge yasak, düzeltme gerekli.
- NEEDS_REVIEW: GPT mentor review gerekli.

## Output Format
1. Summary
2. Changed files assessment
3. Build/test evidence
4. License/asset assessment
5. Scope assessment
6. Decision
