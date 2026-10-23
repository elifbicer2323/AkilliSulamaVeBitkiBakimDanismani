using System.Security.Claims;
using AkilliSulama.Business;
using AkilliSulama.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AkilliSulama.Web.Controllers;

[Authorize]
public class BitkiController : Controller
{
    private readonly BitkiServisi _servis;
    private readonly ParselServisi _parselServis;

    public BitkiController(BitkiServisi servis, ParselServisi parselServis)
    {
        _servis = servis;
        _parselServis = parselServis;
    }

    private int KullaniciId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // Tür listesi: açılır menüde türün özellikleri de görünsün
    private async Task TurleriHazirlaAsync()
    {
        var turler = await _servis.TurleriGetirAsync();
        ViewBag.Turler = new SelectList(
            turler.Select(t => new
            {
                t.BitkiTuruId,
                Metin = $"{t.Ad} ({t.Kategori}) - {t.OnerilenSulamaAraligiGun} günde bir, {t.BitkiBasinaSuLitre} L, {t.GunesIhtiyaci}"
            }),
            "BitkiTuruId", "Metin");
    }

    // /Bitki/Ekle?parselId=1
    [HttpGet]
    public async Task<IActionResult> Ekle(int parselId)
    {
        var parsel = await _parselServis.GetirAsync(parselId, KullaniciId);
        if (parsel is null) return NotFound();

        await TurleriHazirlaAsync();
        return View("Form", new BitkiFormModel { ParselId = parselId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(BitkiFormModel model)
    {
        if (!ModelState.IsValid)
        {
            await TurleriHazirlaAsync();
            return View("Form", model);
        }

        var sonuc = await _servis.EkleAsync(model.EntityyeCevir(), KullaniciId);
        if (!sonuc.Basarili)
        {
            ModelState.AddModelError(string.Empty, sonuc.Mesaj);
            await TurleriHazirlaAsync();
            return View("Form", model);
        }
        TempData["Bilgi"] = sonuc.Mesaj;
        return RedirectToAction("Detay", "Parsel", new { id = model.ParselId });
    }

    [HttpGet]
    public async Task<IActionResult> Duzenle(int id)
    {
        var bitki = await _servis.GetirAsync(id, KullaniciId);
        if (bitki is null) return NotFound();

        await TurleriHazirlaAsync();
        return View("Form", BitkiFormModel.EntitydenOlustur(bitki));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(BitkiFormModel model)
    {
        if (!ModelState.IsValid)
        {
            await TurleriHazirlaAsync();
            return View("Form", model);
        }

        var bitki = model.EntityyeCevir();
        var sonuc = await _servis.GuncelleAsync(bitki, KullaniciId);
        if (!sonuc.Basarili)
        {
            ModelState.AddModelError(string.Empty, sonuc.Mesaj);
            await TurleriHazirlaAsync();
            return View("Form", model);
        }
        TempData["Bilgi"] = sonuc.Mesaj;
        return RedirectToAction("Detay", "Parsel", new { id = bitki.ParselId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var bitki = await _servis.GetirAsync(id, KullaniciId);
        if (bitki is null) return NotFound();

        int parselId = bitki.ParselId;
        var sonuc = await _servis.SilAsync(id, KullaniciId);
        TempData[sonuc.Basarili ? "Bilgi" : "Hata"] = sonuc.Mesaj;
        return RedirectToAction("Detay", "Parsel", new { id = parselId });
    }
}