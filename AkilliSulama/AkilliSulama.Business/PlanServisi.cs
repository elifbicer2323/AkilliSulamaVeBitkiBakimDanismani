using AkilliSulama.Data;
using AkilliSulama.Entities;

namespace AkilliSulama.Business;

// Bitki planı sayfası için gereken her şey
public record PlanOnizleme(Bitki Bitki, SulamaPlani? MevcutPlan, PlanOnerisi Oneri, List<TakvimOlayi> Takvim);

public class PlanServisi
{
    private readonly BitkiRepository _bitkiRepo;
    private readonly PlanRepository _planRepo;

    public PlanServisi(BitkiRepository bitkiRepo, PlanRepository planRepo)
    {
        _bitkiRepo = bitkiRepo;
        _planRepo = planRepo;
    }

    public async Task<PlanOnizleme?> BitkiPlaniAsync(int bitkiId, int kullaniciId)
    {
        // SAHİPLİK: Bitki bu kullanıcıya ait değilse null
        var bitki = await _bitkiRepo.KullanicininBitkisiAsync(bitkiId, kullaniciId);
        if (bitki is null) return null;

        var mevcut = await _planRepo.AktifPlanAsync(bitkiId);
        var oneri = PlanHesaplayici.Hesapla(bitki.BitkiTuru, bitki.Adet, bitki.Parsel.ToprakTuru, DateTime.Today);
        var takvim = mevcut is null
            ? new List<TakvimOlayi>()
            : PlanHesaplayici.TakvimUret(mevcut, bitki.Ad, bitki.Parsel.Ad, DateTime.Today, DateTime.Today.AddDays(13));

        return new PlanOnizleme(bitki, mevcut, oneri, takvim);
    }

    public async Task<IslemSonucu> PlanOlusturAsync(int bitkiId, int kullaniciId, DateTime baslangic, TimeOnly saat)
    {
        var bitki = await _bitkiRepo.KullanicininBitkisiAsync(bitkiId, kullaniciId);
        if (bitki is null) return IslemSonucu.Hata("Bitki bulunamadı veya size ait değil.");

        // İŞ KURALLARI
        if (baslangic.Date < DateTime.Today)
            return IslemSonucu.Hata("Plan başlangıcı geçmişte olamaz.");
        if (baslangic.Date < bitki.EkimTarihi.Date)
            return IslemSonucu.Hata("Plan, ekim tarihinden önce başlayamaz.");
        if (baslangic.Date > DateTime.Today.AddDays(60))
            return IslemSonucu.Hata("Plan en fazla 60 gün sonrası için kurulabilir.");

        bool sabah = saat >= new TimeOnly(5, 0) && saat <= new TimeOnly(10, 0);
        bool aksam = saat >= new TimeOnly(17, 0) && saat <= new TimeOnly(21, 0);
        if (!sabah && !aksam)
            return IslemSonucu.Hata("Sulama saati 05:00-10:00 veya 17:00-21:00 arasında olmalı (gün ortasında su buharlaşır, yapraklar yanabilir).");

        var oneri = PlanHesaplayici.Hesapla(bitki.BitkiTuru, bitki.Adet, bitki.Parsel.ToprakTuru, baslangic);

        // KURAL: Bir bitkinin tek aktif planı olur.
        // Eskiyi pasifleştir + yeniyi ekle -> TEK SaveChanges = tek işlem (transaction): ya ikisi birden olur ya hiçbiri
        var eski = await _planRepo.AktifPlanAsync(bitkiId);
        if (eski is not null) eski.AktifMi = false;

        _planRepo.Ekle(new SulamaPlani
        {
            BitkiId = bitkiId,
            KacGundeBir = oneri.SulamaAraligiGun,
            SuMiktari = oneri.SuMiktariLitre,
            SulamaSaati = saat,
            GubreAraligiGun = oneri.GubreAraligiGun,
            GubreMiktariGram = oneri.GubreMiktariGram,
            BaslangicTarihi = baslangic.Date,
            AktifMi = true,
            Aciklama = string.Join("\n", oneri.UygulananKurallar)
        });
        await _planRepo.KaydetAsync();

        return IslemSonucu.Tamam($"{bitki.Ad} için plan oluşturuldu: {oneri.SulamaAraligiGun} günde bir {oneri.SuMiktariLitre} L su.");
    }

    public async Task<IslemSonucu> PlaniDurdurAsync(int bitkiId, int kullaniciId)
    {
        var bitki = await _bitkiRepo.KullanicininBitkisiAsync(bitkiId, kullaniciId);
        if (bitki is null) return IslemSonucu.Hata("Bitki bulunamadı veya size ait değil.");

        var plan = await _planRepo.AktifPlanAsync(bitkiId);
        if (plan is null) return IslemSonucu.Hata("Bu bitkinin aktif planı yok.");

        plan.AktifMi = false;
        await _planRepo.KaydetAsync();
        return IslemSonucu.Tamam("Plan durduruldu.");
    }

    // Kullanıcının bütün bitkileri için önümüzdeki günlerin takvimi
    public async Task<List<TakvimOlayi>> KullaniciTakvimiAsync(int kullaniciId, int gunSayisi = 7)
    {
        var planlar = await _planRepo.KullanicininAktifPlanlariAsync(kullaniciId);
        var bugun = DateTime.Today;
        return planlar
            .SelectMany(p => PlanHesaplayici.TakvimUret(p, p.Bitki.Ad, p.Bitki.Parsel.Ad, bugun, bugun.AddDays(gunSayisi - 1)))
            .OrderBy(o => o.Tarih).ThenBy(o => o.Saat).ThenBy(o => o.BitkiAd)
            .ToList();
    }
}