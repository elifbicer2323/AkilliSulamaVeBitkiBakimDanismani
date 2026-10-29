# İlerleme Raporu – Ara Sürüm v0.1.0 (Vize)

**Proje:** Akıllı Sulama ve Bitki Bakım Danışmanı
**Teknolojiler:** C#, ASP.NET Core MVC (.NET 10), SQL Server, Entity Framework Core

## Tamamlananlar
| Hafta | Modül | Durum |
|---|---|---|
| 3 | Proje iskeleti, katmanlı mimari, ER diyagramı, mimari şema | ✅ |
| 4 | Veritabanı (6 tablo), ilişkiler, kısıtlar, indeksler, CRUD | ✅ |
| 5 | Kayıt/giriş, parola hash, oturum, rol tabanlı erişim | ✅ |
| 6 | Parsel ve Bitki Kaydı (konum, bitki türü özellikleri, iş kuralları, sahiplik) | ✅ |
| 7 | Sulama/Gübreleme Planlayıcı (kural tabanlı plan, su/gübre hesabı, takvim) | ✅ |
| 8 | Entegrasyon: ana sayfa paneli, sürüm etiketi | ✅ |

## Uçtan uca akış
Kayıt/giriş → Parsel ekle → Bitki ekle → Plan oluştur → Ana sayfada bugünkü işler ve haftalık takvim

## Bilinen eksikler / sonraki adımlar
- Uyarılar şu an örnek veri; sulama zamanı gelince otomatik uyarı üretimi eklenecek.
- Hava durumuna göre (yağmurlu gün) planın ayarlanması düşünülüyor.
- Birim testleri (özellikle PlanHesaplayici için) eklenecek.