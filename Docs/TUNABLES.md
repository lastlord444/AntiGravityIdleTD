# Tunables - Oyun Dengesi Parametreleri

## Genel Ayarlar

### Idle Progression
```yaml
offline_gain_max_hours: 4
offline_gain_efficiency: 0.5  # %50 verim
```

### Economy
```yaml
starting_gold: 100
gold_per_enemy: 10
gold_growth_rate: 1.15
```

### Tower Stats
```yaml
base_tower:
  cost: 50
  damage: 10
  range: 3.0
  fire_rate: 1.0  # saniye

upgraded_tower_multiplier: 1.5
```

### Enemy Stats
```yaml
base_enemy:
  health: 100
  speed: 2.0
  gold_drop: 10

wave_difficulty_scaling: 1.1  # Her wave %10 artış
```

## Anti-Gravity Mekanikleri
```yaml
gravity_flip_cooldown: 30.0  # saniye
gravity_flip_duration: 10.0  # saniye
gravity_affected_tower_bonus: 1.2  # %20 hasar artışı
```

---
_Bu değerler playtest sonuçlarına göre güncellenecektir._
