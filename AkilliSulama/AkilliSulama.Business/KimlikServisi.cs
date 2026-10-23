using AkilliSulama.Data;
using AkilliSulama.Entities;

namespace AkilliSulama.Business;

public record GirisSonucu(bool Basarili, string Mesaj, Kullanici? Kullanici = null);

// Kayıt ve giriş kuralları (iş katmanı)
public class KimlikServisi
{
    private const int MaksHataliDeneme = 5;
    private static readonly TimeSpan KilitSuresi = TimeSpan.FromMinutes(15);

    private readonly KullaniciRepository _repo;
    public KimlikServisi(KullaniciRepository repo) => _repo = repo;

    public async Task<(bool Basarili, string Mesaj)> KayitOlAsync(string ad, string soyad, string eposta, string sifre)
    {
        eposta = eposta.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(soyad))
            return (false, "Ad ve soyad zorunludur.");
        if (sifre.Length < 8 || !sifre.Any(char.IsDigit) || !sifre.Any(char.IsLetter))
            return (false, "Parola en az 8 karakter olmalı, harf ve rakam içermelidir.");
        if (await _repo.EpostaVarMiAsync(eposta))
            return (false, "Bu e-posta ile kayıtlı bir hesap zaten var.");

        await _repo.EkleAsync(new Kullanici
        {
            Ad = ad.Trim(),
            Soyad = soyad.Trim(),
            Eposta = eposta,
            SifreHash = SifreHasher.Hashle(sifre), // düz metin ASLA saklanmaz
            Rol = Roller.Ciftci,                    // yeni kayıtlar her zaman Çiftçi olur
            KayitTarihi = DateTime.Now
        });
        return (true, "Kayıt başarılı. Giriş yapabilirsiniz.");
    }

    public async Task<GirisSonucu> GirisYapAsync(string eposta, string sifre)
    {
        eposta = eposta.Trim().ToLowerInvariant();
        const string genelHata = "E-posta veya parola hatalı.";

        var kullanici = await _repo.EpostaIleGetirAsync(eposta);

        // Güvenlik: e-posta yoksa da AYNI mesaj → saldırgan hangi e-postanın kayıtlı olduğunu öğrenemez
        if (kullanici is null) return new GirisSonucu(false, genelHata);

        if (kullanici.KilitBitisZamani > DateTime.Now)
            return new GirisSonucu(false, "Çok fazla hatalı deneme. Hesap 15 dakika kilitlendi.");

        if (!SifreHasher.Dogrula(sifre, kullanici.SifreHash))
        {
            kullanici.BasarisizGirisSayisi++;
            if (kullanici.BasarisizGirisSayisi >= MaksHataliDeneme)
            {
                kullanici.KilitBitisZamani = DateTime.Now.Add(KilitSuresi);
                kullanici.BasarisizGirisSayisi = 0;
            }
            await _repo.KaydetAsync();
            return new GirisSonucu(false, genelHata);
        }

        // Başarılı giriş: sayaçları sıfırla
        kullanici.BasarisizGirisSayisi = 0;
        kullanici.KilitBitisZamani = null;
        await _repo.KaydetAsync();
        return new GirisSonucu(true, "Giriş başarılı.", kullanici);
    }

    public Task<List<Kullanici>> KullanicilariListeleAsync() => _repo.TumuAsync();
}