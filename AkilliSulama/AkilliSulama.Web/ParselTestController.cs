using AkilliSulama.Business;
using AkilliSulama.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AkilliSulama.Web.Controllers;

// Test amaçlı: CRUD işlemlerini sırayla çalıştırıp sonucu gösterir.
// Web katmanı veritabanına değil, iş katmanına (ParselServisi) konuşur.
public class ParselTestController : Controller
{
    private readonly ParselServisi _servis;
    public ParselTestController(ParselServisi servis) => _servis = servis;

    // Adres: /ParselTest/Liste
    public async Task<IActionResult> Liste(int kullaniciId = 1)
    {
        var parseller = await _servis.ListeleAsync(kullaniciId);
        var sonuc = parseller.Select(p => new
        {
            p.ParselId,
            p.Ad,
            p.Alan,
            p.ToprakTuru,
            Bitkiler = p.Bitkiler.Select(b => b.Ad)
        });
        return Json(sonuc);
    }

    // Adres: /ParselTest/Crud
    public async Task<IActionResult> Crud()
    {
        var adimlar = new List<string>();

        // CREATE
        var yeni = await _servis.EkleAsync(new Parsel
        {
            KullaniciId = 1,
            Ad = "Test Parseli",
            Konum = "Deneme",
            Alan = 10,
            ToprakTuru = "Killi"
        });
        adimlar.Add($"1) EKLE: '{yeni.Ad}' eklendi, yeni Id = {yeni.ParselId}");

        // READ
        var okunan = await _servis.GetirAsync(yeni.ParselId);
        adimlar.Add($"2) OKU: Id {okunan!.ParselId} -> Ad: {okunan.Ad}, Alan: {okunan.Alan} m²");

        // UPDATE
        okunan.Ad = "Güncellenmiş Parsel";
        okunan.Alan = 15;
        await _servis.GuncelleAsync(okunan);
        var guncel = await _servis.GetirAsync(yeni.ParselId);
        adimlar.Add($"3) GÜNCELLE: Yeni ad: {guncel!.Ad}, yeni alan: {guncel.Alan} m²");

        // DELETE
        await _servis.SilAsync(yeni.ParselId);
        var silinen = await _servis.GetirAsync(yeni.ParselId);
        adimlar.Add($"4) SİL: Kayıt silindi mi? {(silinen is null ? "Evet" : "Hayır")}");

        // İŞ KURALI TESTİ
        try
        {
            await _servis.EkleAsync(new Parsel { KullaniciId = 1, Ad = "Hatalı", Alan = 0 });
            adimlar.Add("5) KURAL: Alan 0 olan parsel eklendi (beklenmiyordu!)");
        }
        catch (ArgumentException ex)
        {
            adimlar.Add($"5) KURAL: Alan 0 olan parsel reddedildi -> {ex.Message}");
        }

        return Content(string.Join("\n", adimlar));
    }
}