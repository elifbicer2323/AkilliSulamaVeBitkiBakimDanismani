using System.Security.Claims;
using AkilliSulama.Business;
using AkilliSulama.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AkilliSulama.Web.Controllers;

// Kayıt, giriş, çıkış. Kurallar iş katmanında (KimlikServisi).
public class HesapController : Controller
{
    private readonly KimlikServisi _kimlik;
    public HesapController(KimlikServisi kimlik) => _kimlik = kimlik;

    [HttpGet]
    public IActionResult Kayit() => View(new KayitModel());

    [HttpPost]
    [ValidateAntiForgeryToken] // CSRF koruması
    public async Task<IActionResult> Kayit(KayitModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var (basarili, mesaj) = await _kimlik.KayitOlAsync(model.Ad, model.Soyad, model.Eposta, model.Sifre);
        if (!basarili)
        {
            ModelState.AddModelError(string.Empty, mesaj);
            return View(model);
        }
        TempData["Bilgi"] = mesaj;
        return RedirectToAction(nameof(Giris));
    }

    [HttpGet]
    public IActionResult Giris(string? returnUrl = null) => View(new GirisModel { ReturnUrl = returnUrl });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Giris(GirisModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var sonuc = await _kimlik.GirisYapAsync(model.Eposta, model.Sifre);
        if (!sonuc.Basarili)
        {
            ModelState.AddModelError(string.Empty, sonuc.Mesaj);
            return View(model);
        }

        // Oturum kartına (cookie) kullanıcının kimliğini ve ROLÜNÜ yaz
        var k = sonuc.Kullanici!;
        var bilgiler = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, k.KullaniciId.ToString()),
            new(ClaimTypes.Name, $"{k.Ad} {k.Soyad}"),
            new(ClaimTypes.Email, k.Eposta),
            new(ClaimTypes.Role, k.Rol)
        };
        var kimlik = new ClaimsIdentity(bilgiler, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(kimlik));

        // Açık yönlendirme saldırısını önle: sadece kendi sitemizdeki adreslere dön
        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cikis()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Giris));
    }

    public IActionResult ErisimEngellendi() => View();
}