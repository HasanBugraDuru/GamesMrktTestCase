# 🍓 Match-3 Tile Sliding Game

Bu proje, Gamesmrkt tarafından verilen bir test case kapsamında geliştirilmiştir. Scrubby Dubby Saga benzeri, satır ve sütun kaydırmalı bir Match-3 oyunudur. Unity ile geliştirilmiş olup temel oyun mekanikleri ve 3 seviye içermektedir.

## 🎮 Oyun Özellikleri

- 🎯 Satır ve sütun kaydırmalı Match-3 mekaniği  
- 🎨 Satranç tahtası deseninde karo oluşturma sistemi  
- 🍓 Rastgele yerleştirilen 5 farklı meyve  
- 🧠 Level bilgileri ScriptableObject olarak yönetilir  
- 🧩 Belirli meyveleri toplama üzerine kurulu görev sistemi  
- ⚡ Eşleşme sonrası otomatik olarak boşlukları dolduran "kendini yenileyen" grid sistemi  
- 💥 Eşleşme sonrası partikül efektleri (VFX)  
- 🔊 Ses Efektleri (SFX) sistemi (eşleşme, kaydırma vs.)  

## 🧩 Seviye Bilgileri

### Level 1
- Grid: 5x5  
- Görev: 5 kırmızı, 5 sarı meyve topla

### Level 2
- Grid: 4x6  
- Görev: 3 kırmızı, 5 sarı, 2 yeşil meyve topla

### Level 3
- Grid: 8x6  
- Görev: 12 kırmızı, 8 mavi meyve topla

## 🛠 Kullanılan Teknolojiler

- Unity 2022+
- C#
- Unity UI System
- ScriptableObject (Level verisi, ayarlar)
- Partikül sistemi (Unity Particle System)
- Ses sistemi (AudioSource & AudioClip)
- Unity Asset Store'dan alınan [Hungry Bat - Match 3 UI Free](https://assetstore.unity.com/packages/2d/gui/hungry-bat-match-3-ui-free-229197)


