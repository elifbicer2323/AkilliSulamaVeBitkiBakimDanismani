using System.Security.Claims;
using AkilliSulama.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkilliSulama.Web.Controllers;

[Authorize]
public class PlanController : Controller
{
    private readonly PlanServisi _servis;
    public PlanController(PlanServisi servis) => _servis = servis;

    private int KullaniciId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // /Plan/Bitki/5 -> bir bitkinin planı, önerisi ve takvimi
    public async Task<IActionResult> Bitki(int id)
    {
        var onizleme = await _servis.BitkiPlaniAsync(id, KullaniciId);
        if (onizleme is null) return NotFound(); // Başkasının bitkisi de "bulunamadı"
        return View(onizleme);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Olustur(int bitkiId, DateTime baslangic, string saat)
    {
        if (!TimeOnly.TryParse(saat, out var sulamaSaati))
        {
            TempData["Hata"] = "Geçerli bir sulama saati girin.";
            return RedirectToAction(nameof(Bitki), new { id = bitkiId });
        }

        var sonuc = await _servis.PlanOlusturAsync(bitkiId, KullaniciId, baslangic, sulamaSaati);
        TempData[sonuc.Basarili ? "Bilgi" : "Hata"] = sonuc.Mesaj;
        return RedirectToAction(nameof(Bitki), new { id = bitkiId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Durdur(int bitkiId)
    {
        var sonuc = await _servis.PlaniDurdurAsync(bitkiId, KullaniciId);
        TempData[sonuc.Basarili ? "Bilgi" : "Hata"] = sonuc.Mesaj;
        return RedirectToAction(nameof(Bitki), new { id = bitkiId });
    }

    // /Plan/Takvim -> bütün bitkiler için önümüzdeki 7 gün
    public async Task<IActionResult> Takvim()
    {
        var takvim = await _servis.KullaniciTakvimiAsync(KullaniciId, 7);
        return View(takvim);
    }
}