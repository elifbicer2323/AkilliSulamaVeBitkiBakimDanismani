
using AkilliSulama.Data;
using AkilliSulama.Entities;

namespace AkilliSulama.Business;

// İşlem sonucu: başarılı mı, kullanıcıya gösterilecek mesaj ne?
public record IslemSonucu(bool Basarili, string Mesaj)
{
    public static IslemSonucu Tamam(string mesaj) => new(true, mesaj);
    public static IslemSonucu Hata(string mesaj) => new(false, mesaj);
}

// Parsel iş kuralları ve sahiplik kontrolü
public class ParselServisi
{
    private const decimal MaksAlanM2 = 1_000_000; // 100 hektar

    private readonly ParselRepository _repo;
    public ParselServisi(ParselRepository repo) => _repo = repo;

    public Task<List<Parsel>> ListeleAsync(int kullaniciId) => _repo.KullanicininParselleriAsync(kullaniciId);

    public Task<Parsel?> GetirAsync(int parselId, int kullaniciId) => _repo.KullanicininParseliAsync(parselId, kullaniciId);

    public async Task<IslemSonucu> EkleAsync(Parsel parsel, int kullaniciId)
    {
        parsel.KullaniciId = kullaniciId; // Sahibi formdan değil, oturumdan alınır
        var hata = await DogrulaAsync(parsel, kullaniciId, null);
        if (hata is not null) return IslemSonucu.Hata(hata);

        Temizle(parsel);
        await _repo.EkleAsync(parsel);
        return IslemSonucu.Tamam($"'{parsel.Ad}' parseli eklendi.");
    }

    public async Task<IslemSonucu> GuncelleAsync(Parsel form, int kullaniciId)
    {
        var mevcut = await _repo.KullanicininParseliAsync(form.ParselId, kullaniciId);
        if (mevcut is null) return IslemSonucu.Hata("Parsel bulunamadı veya size ait değil.");

        var hata = await DogrulaAsync(form, kullaniciId, form.ParselId);
        if (hata is not null) return IslemSonucu.Hata(hata);

        Temizle(form);
        mevcut.Ad = form.Ad;
        mevcut.Alan = form.Alan;
        mevcut.ToprakTuru = form.ToprakTuru;
        mevcut.Il = form.Il;
        mevcut.Ilce = form.Ilce;
        mevcut.Konum = form.Konum;
        mevcut.Enlem = form.Enlem;
        mevcut.Boylam = form.Boylam;
        await _repo.KaydetAsync();
        return IslemSonucu.Tamam("Parsel güncellendi.");
    }

    public async Task<IslemSonucu> SilAsync(int parselId, int kullaniciId)
    {
        var mevcut = await _repo.KullanicininParseliAsync(parselId, kullaniciId);
        if (mevcut is null) return IslemSonucu.Hata("Parsel bulunamadı veya size ait değil.");

        await _repo.SilAsync(mevcut);
        return IslemSonucu.Tamam($"'{mevcut.Ad}' parseli ve bitkileri silindi.");
    }

    // İŞ KURALLARI
    private async Task<string?> DogrulaAsync(Parsel p, int kullaniciId, int? haricParselId)
    {
        if (string.IsNullOrWhiteSpace(p.Ad)) return "Parsel adı boş olamaz.";
        if (p.Alan <= 0) return "Parsel alanı 0'dan büyük olmalıdır.";
        if (p.Alan > MaksAlanM2) return "Parsel alanı en fazla 1.000.000 m² olabilir.";
        if (string.IsNullOrWhiteSpace(p.Il)) return "İl bilgisi zorunludur.";
        if ((p.Enlem is null) != (p.Boylam is null)) return "Enlem ve boylam birlikte girilmelidir.";
        if (p.Enlem is < -90 or > 90) return "Enlem -90 ile 90 arasında olmalıdır.";
        if (p.Boylam is < -180 or > 180) return "Boylam -180 ile 180 arasında olmalıdır.";
        if (await _repo.AyniIsimVarMiAsync(kullaniciId, p.Ad.Trim(), haricParselId))
            return "Bu isimde bir parseliniz zaten var.";
        return null;
    }

    private static void Temizle(Parsel p)
    {
        p.Ad = p.Ad.Trim();
        p.Il = p.Il.Trim();
        p.Ilce = p.Ilce?.Trim();
        p.Konum = p.Konum?.Trim();
    }
}