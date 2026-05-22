# ROADMAP.md

## Stage 0 — Recovery & Project OS
Amaç: Repo temiz, BlockForge arşivli, agent workflow hazır.

Exit Criteria:
- [x] develop temiz
- [x] BlockForge archive branch’te
- [x] AGENTS/CLAUDE/skills/docs mevcut
- [x] PR workflow net

## Stage 1 — Combat Core (Spike)
Amaç: Enemy + waypoint + tower shooting.

Exit Criteria:
- [x] enemy path boyunca yürür
- [x] tower range içindeki enemy’yi bulur
- [x] projectile/damage çalışır
- [x] compile error yok

## Stage 1.5 — Reference & Art Transition (PR #5, #6, #7)
Amaç: Referans repo audit/stratejisi, Kenney CC0 spritelarının sisteme eklenerek görsel sahnenin yenilenmesi.

Exit Criteria:
- [x] AGENTS.md ve REFERENCE_REPOS.md güncellendi (PR #5)
- [ ] Kenney CC0 spriteları import edildi & THIRD_PARTY_ASSETS.md oluşturuldu (PR #6)
- [ ] Görsel sahne Kenney spriteları ile yenilendi (PR #7)

## Stage 2 — Game Loop & Combat Loop (PR #8, #9)
Amaç: wave, base HP, gold, win/lose, tower placement.

Exit Criteria:
- [ ] 3–5 wave oynanır (Wave Spawner) (PR #8)
- [ ] enemy base’e ulaşınca HP azalır (PR #8)
- [ ] enemy ölünce gold gelir (PR #8)
- [ ] victory/game over çalışır (PR #8)
- [ ] tower placement & build node sistemi çalışır (PR #9)

## Stage 3 — Playable UI & Android Build (PR #10)
Amaç: Canvas Scaler mobil uyumlu arayüz ve Android test build.

Exit Criteria:
- [ ] gold/wave/base HP görünür
- [ ] Mobile Touch Input ve Canvas Scaler entegre
- [ ] Android debug build (APK/AAB) alınır
- [ ] 5–10 tester oynar, PLAYTEST_PLAN güncellenir

## Stage 4 — Anti-Gravity Spike (PR #11)
Amaç: gravity mechanic gerçekten eğlenceli mi test etmek.

Exit Criteria:
- [ ] 30 sn gameplay video
- [ ] mechanic decision: integrate / revise / discard

## Stage 5 — Idle v0.1 (PR #12)
Amaç: offline gold/progression.

Exit Criteria:
- [ ] lastPlayTimestamp çalışır
- [ ] offline gold hesaplanır
- [ ] exploit riskleri yazılır

