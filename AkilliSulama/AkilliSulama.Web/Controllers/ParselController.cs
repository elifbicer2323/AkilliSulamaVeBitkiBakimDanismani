using System.Security.Claims;
using AkilliSulama.Business;
using AkilliSulama.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkilliSulama.Web.Controllers;

// Sadece giriş yapmış kullanıcılar; herkes yalnızca KENDİ parsellerini görür
[Authorize]
public class ParselController : Controller
{
    private readonly ParselServisi _servis;
    public ParselController(ParselServisi servis) => _servis = servis;

    // Kullanıcı kimliği formdan değil, oturumdan (cookie) alınır
    private int KullaniciId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // /Parsel
    public async Task<IActionResult> Index() => View(await _servis.ListeleAsync(KullaniciId));

    // /Parsel/Detay/5
    public async Task<IActionResult> Detay(int id)
    {
        var parsel = await _servis.GetirAsync(id, KullaniciId);
        if (parsel is null) return NotFound(); // Başkasının parseli de "bulunamadı" görünür
        return View(parsel);
    }

    [HttpGet]
    public IActionResult Ekle() => View("Form", new ParselFormModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(ParselFormModel model)
    {
        if (!ModelState.IsValid) return View("Form", model);

        var sonuc = await _servis.EkleAsync(model.EntityyeCevir(), KullaniciId);
        if (!sonuc.Basarili)
        {
            ModelState.AddModelError(string.Empty, sonuc.Mesaj);
            return View("Form", model);
        }
        TempData["Bilgi"] = sonuc.Mesaj;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Duzenle(int id)
    {
        var parsel = await _servis.GetirAsync(id, KullaniciId);
        if (parsel is null) return NotFound();
        return View("Form", ParselFormModel.EntitydenOlustur(parsel));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(ParselFormModel model)
    {
        if (!ModelState.IsValid) return View("Form", model);

        var sonuc = await _servis.GuncelleAsync(model.EntityyeCevir(), KullaniciId);
        if (!sonuc.Basarili)
        {
            ModelState.AddModelError(string.Empty, sonuc.Mesaj);
            return View("Form", model);
        }
        TempData["Bilgi"] = sonuc.Mesaj;
        return RedirectToAction(nameof(Detay), new { id = model.ParselId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _servis.SilAsync(id, KullaniciId);
        TempData[sonuc.Basarili ? "Bilgi" : "Hata"] = sonuc.Mesaj;
        return RedirectToAction(nameof(Index));
    }
}