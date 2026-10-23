using AkilliSulama.Data;
using AkilliSulama.Entities;

namespace AkilliSulama.Business;

// Bitki iş kuralları ve sahiplik kontrolü
public class BitkiServisi
{
    private const int MetrekareBasinaMaksBitki = 10; // Kapasite kuralı

    private readonly BitkiRepository _bitkiRepo;
    private readonly ParselRepository _parselRepo;

    public BitkiServisi(BitkiRepository bitkiRepo, ParselRepository parselRepo)
    {
        _bitkiRepo = bitkiRepo;
        _parselRepo = parselRepo;
    }

    public Task<List<BitkiTuru>> TurleriGetirAsync() => _bitkiRepo.TurleriGetirAsync();

    public Task<Bitki?> GetirAsync(int bitkiId, int kullaniciId) => _bitkiRepo.KullanicininBitkisiAsync(bitkiId, kullaniciId);

    public async Task<IslemSonucu> EkleAsync(Bitki bitki, int kullaniciId)
    {
        // SAHİPLİK: Başkasının parseline bitki eklenemez
        var parsel = await _parselRepo.KullanicininParseliAsync(bitki.ParselId, kullaniciId);
        if (parsel is null) return IslemSonucu.Hata("Parsel bulunamadı veya size ait değil.");

        var hata = await DogrulaAsync(bitki, parsel, null);
        if (hata is not null) return IslemSonucu.Hata(hata);

        bitki.Ad = bitki.Ad.Trim();
        await _bitkiRepo.EkleAsync(bitki);
        return IslemSonucu.Tamam($"'{bitki.Ad}' eklendi.");
    }

    public async Task<IslemSonucu> GuncelleAsync(Bitki form, int kullaniciId)
    {
        var mevcut = await _bitkiRepo.KullanicininBitkisiAsync(form.BitkiId, kullaniciId);
        if (mevcut is null) return IslemSonucu.Hata("Bitki bulunamadı veya size ait değil.");

        var parsel = await _parselRepo.KullanicininParseliAsync(mevcut.ParselId, kullaniciId);
        form.ParselId = mevcut.ParselId; // Bitki başka parsele taşınamaz
        var hata = await DogrulaAsync(form, parsel!, form.BitkiId);
        if (hata is not null) return IslemSonucu.Hata(hata);

        mevcut.Ad = form.Ad.Trim();
        mevcut.BitkiTuruId = form.BitkiTuruId;
        mevcut.Adet = form.Adet;
        mevcut.EkimTarihi = form.EkimTarihi;
        mevcut.Notlar = form.Notlar;
        await _bitkiRepo.KaydetAsync();
        return IslemSonucu.Tamam("Bitki güncellendi.");
    }

    public async Task<IslemSonucu> SilAsync(int bitkiId, int kullaniciId)
    {
        var mevcut = await _bitkiRepo.KullanicininBitkisiAsync(bitkiId, kullaniciId);
        if (mevcut is null) return IslemSonucu.Hata("Bitki bulunamadı veya size ait değil.");

        await _bitkiRepo.SilAsync(mevcut);
        return IslemSonucu.Tamam($"'{mevcut.Ad}' silindi.");
    }

    // İŞ KURALLARI
    private async Task<string?> DogrulaAsync(Bitki b, Parsel parsel, int? haricBitkiId)
    {
        if (string.IsNullOrWhiteSpace(b.Ad)) return "Bitki adı boş olamaz.";
        if (b.Adet < 1 || b.Adet > 10_000) return "Adet 1 ile 10.000 arasında olmalıdır.";
        if (b.EkimTarihi.Date > DateTime.Today) return "Ekim tarihi gelecekte olamaz.";
        if (await _bitkiRepo.TurGetirAsync(b.BitkiTuruId) is null) return "Geçerli bir bitki türü seçin.";
        if (await _bitkiRepo.AyniIsimVarMiAsync(parsel.ParselId, b.Ad.Trim(), haricBitkiId))
            return "Bu parselde aynı isimde bir bitki zaten var.";

        // KAPASİTE KURALI: m² başına en fazla 10 bitki
        int mevcutAdet = parsel.Bitkiler.Where(x => x.BitkiId != haricBitkiId).Sum(x => x.Adet);
        int kapasite = (int)(parsel.Alan * MetrekareBasinaMaksBitki);
        if (mevcutAdet + b.Adet > kapasite)
            return $"Parsel kapasitesi aşılıyor: {parsel.Alan} m² için en fazla {kapasite} bitki (şu an {mevcutAdet}).";

        return null;
    }
}