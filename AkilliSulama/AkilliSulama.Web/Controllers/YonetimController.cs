using System.Text;
using AkilliSulama.Business;
using AkilliSulama.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkilliSulama.Web.Controllers;

// ROL TABANLI ERİŞİM: Bu sayfaya SADECE "Yonetici" rolündekiler girebilir.
// Kontrol sunucuda yapılır; menüden link gizlemek tek başına güvenlik değildir.
[Authorize(Roles = Roller.Yonetici)]
public class YonetimController : Controller
{
    private readonly KimlikServisi _kimlik;
    public YonetimController(KimlikServisi kimlik) => _kimlik = kimlik;

    // Adres: /Yonetim
    public async Task<IActionResult> Index()
    {
        var kullanicilar = await _kimlik.KullanicilariListeleAsync();
        var sb = new StringBuilder("YÖNETİM PANELİ - Kayıtlı kullanıcılar\n\n");
        foreach (var k in kullanicilar)
            sb.AppendLine($"{k.KullaniciId}) {k.Ad} {k.Soyad} | {k.Eposta} | Rol: {k.Rol}");
        // Not: Parola hash'i bile ekrana basılmaz
        return Content(sb.ToString());
    }
}