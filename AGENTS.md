# AGENTS.md — AntiGravityIdleTD

## Project Truth
Repo: lastlord444/AntiGravityIdleTD  
Ana ürün: Android-first Micro TD / Idle Lane Defense  
Engine: Unity 6.3 LTS / URP 2D  
Branch flow: develop → feature/* → PR → develop  
Main branch daha sonra stable release için oluşturulacaktır.

## Non-Negotiable Rules
- develop veya main branch’e direkt push yok.
- Her iş issue + feature branch + PR ile yapılır.
- PR olmadan iş tamamlanmış sayılmaz.
- Gameplay kodu yazmadan önce repo truth okunur.
- Her ajan önce mevcut dosyaları okur, sonra dosya değişiklik planı verir.
- Scope dışı dosya değiştirilmez.
- Unity scene/prefab değişiklikleri açıkça raporlanır.
- Lisansı belirsiz asset veya repo kodu eklenmez.
- GPL veya copyleft lisanslı kod projeye alınmaz.
- BlockForge kodu ana projeye geri getirilmez.
- Referans repo stratejisi: Lisansı uygun açık kaynak kodlar (örn. Unlicense/MIT/CC0) incelenip uyarlanabilir; ancak proje bütünlüğü için körü körüne kopyalama veya ProjectSettings/Packages aktarımı yapılmaz. Kaynak ve lisans her zaman THIRD_PARTY_ASSETS.md dosyasına işlenir.

## Current Product Direction
İlk hedef tam oyun değil, çalışan vertical slice:
- 1 lane/path
- 1 map
- 2 tower type
- 2 enemy type
- 3–5 wave
- gold economy
- base HP
- win/lose
- basic UI
- Android build

## Deferred
- Idle progression
- Anti-gravity full integration
- Real Firebase
- Real AdMob
- IAP
- Cloud save
- Multiplayer
- Complex meta economy
- Procedural levels

## Required Agent Roles
1. Repo Auditor
2. Game Architect
3. Unity Scene Agent
4. Gameplay Coder
5. Level Designer
6. QA Playtest Agent
7. Docs Sync Agent
8. Reference Repo Reviewer
9. Release Gate Agent

## Update Policy
Bu dosya yaşayan kontrattır. Ajanlar bu dosyanın eski kaldığını fark ederse kod yazmadan önce kullanıcıyı ve GPT mentor’u uyarmalıdır.

Ajan şu ifadeyi kullanmalıdır:
“AGENTS.md mevcut repo truth ile uyumsuz görünüyor. Kod yazmadan önce doküman güncelleme PR’ı öneriyorum.”

## Authority
Final teknik/scope kararı GPT mentor review sonrası verilir.
Antigravity uygular.
Opus mimari/ürün danışmanı olabilir.
Comet pazar/repo/tool doğrulaması için kullanılabilir.
Dyad yalnızca web, landing, dashboard, store/support tool işleri için kullanılır.
