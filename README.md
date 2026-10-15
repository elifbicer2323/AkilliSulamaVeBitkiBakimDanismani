# Akıllı Sulama ve Bitki Bakım Danışmanı

Kullanıcıların bahçelerindeki parselleri ve bitkileri kaydedip sulama planı oluşturabildiği, sulama zamanı geldiğinde uyarı alabildiği bir web uygulaması.

## Kullanılan Teknolojiler
- C# / ASP.NET Core MVC (.NET 10)
- SQL Server Express
- Entity Framework Core
- Git & GitHub

## Mimari: Katmanlı Yapı
| Proje | Katman | Görevi |
|---|---|---|
| AkilliSulama.Web | Sunum | Sayfalar ve kullanıcı arayüzü |
| AkilliSulama.Business | İş | Sulama kuralları ve hesaplamalar |
| AkilliSulama.Data | Veri | SQL Server erişimi |
| AkilliSulama.Entities | Ortak model | Kullanıcı, Parsel, Bitki, Sulama Planı, Uyarı sınıfları |

Bağımlılık yönü: Web → Business → Data → Entities

## Varlıklar
Kullanıcı, Parsel, Bitki, Sulama Planı, Uyarı

## Çalıştırma
1. Depoyu klonlayın.
2. `AkilliSulama/AkilliSulama.slnx` dosyasını Visual Studio ile açın.
3. `AkilliSulama.Web` projesini başlangıç projesi yapıp F5 ile çalıştırın.