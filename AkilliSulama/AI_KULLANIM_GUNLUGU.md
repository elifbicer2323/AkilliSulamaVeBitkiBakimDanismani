# Yapay Zekâ Kullanım Günlüğü

Bu projede yapay zekâyı (Claude) bir öğretmen/rehber gibi kullandım: adımları anlattı, örnek kod verdi, hataları birlikte çözdük. Her kodu projeye eklerken ne işe yaradığını sordum ve anladım.

| Hafta | Ne için kullandım | Ne öğrendim / ben ne yaptım |
|---|---|---|
| 3 | Kurulum, GitHub deposu, katmanlı proje yapısı | 4 katmanı (Web, Business, Data, Entities) kendim oluşturdum ve referansları bağladım. ER diyagramını ve mimari şemayı draw.io'da çizdim. |
| 4 | Entity Framework Core, DbContext, migration | Tablolar, ilişkiler (1-N), kısıtlar (CHECK, FK) ve indeksleri öğrendim. Kısıtları SSMS'te kendim test ettim. |
| 5 | Parola hash'leme, giriş/kayıt, rol kontrolü | Parolanın neden hash'lendiğini, tuzun (salt) ne işe yaradığını öğrendim. Çiftçi/Yönetici rollerini test ettim. |
| 6 | Parsel ve bitki modülü, iş kuralları | Sahiplik kontrolünü (başkasının parselini görememe) test ettim, 404 sonucunu gördüm. |
| 7 | Sulama/gübreleme planı ve takvim | Kural tabanlı hesabı (toprak, mevsim) ve su miktarı formülünü öğrendim. Yanlış saat kuralını test ettim. |
| 8 | Entegrasyon, ana sayfa, belgeler | Modülleri ana sayfada birleştirdim, sürümü etiketledim. |

## Yapay zekâdan gelen ama düzelttiğim hatalar
- Visual Studio'da ana sayfa kodunu yanlış Index.cshtml dosyasına (Parsel klasörü) yapıştırdım, fark edip düzelttim.
- Bir kayıt satırını `return` ifadesinin altına ekledim; çalışmayacağını öğrenip yerini düzelttim.
- Migration'ı kaydetmeden çalıştırdığım için hata aldım; dosyaları kaydedip yeniden migration ekledim.

## Kurallar
- Gerçek kişi verisi kullanılmadı, tüm örnek veriler sentetiktir (KVKK).
- Anlamadığım hiçbir kodu projeye eklemedim.