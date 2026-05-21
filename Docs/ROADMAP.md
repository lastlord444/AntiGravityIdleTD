# ROADMAP.md

## Stage 0 — Recovery & Project OS
Amaç: Repo temiz, BlockForge arşivli, agent workflow hazır.

Exit Criteria:
- develop temiz
- BlockForge archive branch’te
- AGENTS/CLAUDE/skills/docs mevcut
- PR workflow net

## Stage 1 — Combat Core
Amaç: Enemy + waypoint + tower shooting.

Exit Criteria:
- enemy path boyunca yürür
- tower range içindeki enemy’yi bulur
- projectile/damage çalışır
- compile error yok

## Stage 2 — Game Loop
Amaç: wave, base HP, gold, win/lose.

Exit Criteria:
- 3–5 wave oynanır
- enemy base’e ulaşınca HP azalır
- enemy ölünce gold gelir
- victory/game over çalışır

## Stage 3 — Playable UI
Amaç: basic mobile UI.

Exit Criteria:
- gold/wave/base HP görünür
- tower placement panel çalışır
- restart/next çalışır

## Stage 4 — Android Build & Playtest
Amaç: APK/AAB ile gerçek kullanıcı test.

Exit Criteria:
- Android build alınır
- 5–10 tester oynar
- PLAYTEST_PLAN güncellenir

## Stage 5 — Anti-Gravity Spike
Amaç: gravity mechanic gerçekten eğlenceli mi test etmek.

Exit Criteria:
- 30 sn gameplay video
- mechanic decision: integrate / revise / discard

## Stage 6 — Idle v0.1
Amaç: offline gold/progression.

Exit Criteria:
- lastPlayTimestamp çalışır
- offline gold hesaplanır
- exploit riskleri yazılır
