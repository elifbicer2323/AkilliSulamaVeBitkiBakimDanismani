using System.Globalization;
using AkilliSulama.Entities;

namespace AkilliSulama.Business;

// Hesaplanan plan önerisi + hangi kuralların uygulandığı
public record PlanOnerisi(
    int SulamaAraligiGun,
    decimal SuMiktariLitre,
    int GubreAraligiGun,
    decimal GubreMiktariGram,
    List<string> UygulananKurallar);

// Takvimdeki bir satır: hangi gün, ne yapılacak, ne kadar
public record TakvimOlayi(DateTime Tarih, TimeOnly Saat, string Tur, string BitkiAd, string ParselAd, string Miktar);

// KURAL TABANLI PLAN: bitki türü + toprak + mevsim -> sulama/gübre planı
public static class PlanHesaplayici
{
    private static readonly CultureInfo Tr = new("tr-TR");

    public static PlanOnerisi Hesapla(BitkiTuru tur, int adet, string? toprakTuru, DateTime tarih)
    {
        var kurallar = new List<string>();
        int aralik = tur.OnerilenSulamaAraligiGun;
        decimal suCarpani = 1.0m;
        int gubreAralik = tur.GubrelemeAraligiGun;

        kurallar.Add($"Temel değer: {tur.Ad} {tur.OnerilenSulamaAraligiGun} günde bir, bitki başına {tur.BitkiBasinaSuLitre} L su.");

        // KURAL 1: Toprak türü
        string toprak = (toprakTuru ?? string.Empty).ToLower(Tr);
        if (toprak.Contains("kum"))
        {
            aralik -= 1; suCarpani *= 1.2m;
            kurallar.Add("Kumlu toprak suyu hızlı kaybeder: sulama 1 gün daha sık, su %20 fazla.");
        }
        else if (toprak.Contains("kil"))
        {
            aralik += 1; suCarpani *= 0.9m;
            kurallar.Add("Killi toprak suyu uzun tutar: sulama 1 gün daha seyrek, su %10 az.");
        }
        else if (toprak.Contains("saksı"))
        {
            suCarpani *= 1.1m;
            kurallar.Add("Saksı toprağı çabuk kurur: su %10 fazla.");
        }

        // KURAL 2: Mevsim
        int ay = tarih.Month;
        if (ay is 6 or 7 or 8)
        {
            aralik -= 1; suCarpani *= 1.3m;
            kurallar.Add("Yaz: buharlaşma yüksek, sulama 1 gün daha sık, su %30 fazla.");
        }
        else if (ay is 12 or 1 or 2)
        {
            aralik += 1; suCarpani *= 0.7m; gubreAralik *= 2;
            kurallar.Add("Kış: bitki yavaş büyür, sulama 1 gün daha seyrek, su %30 az, gübreleme aralığı 2 katı.");
        }
        else
        {
            kurallar.Add("İlkbahar/sonbahar: mevsim düzeltmesi yok.");
        }

        // KURAL 3: Güvenli sınırlar (her gün ile haftada bir arası)
        aralik = Math.Clamp(aralik, 1, 7);

        // SU / GİRDİ HESABI
        decimal su = Math.Round(adet * tur.BitkiBasinaSuLitre * suCarpani, 1);
        decimal gubre = Math.Round(adet * tur.BitkiBasinaGubreGram, 1);

        kurallar.Add($"Su hesabı: {adet} bitki × {tur.BitkiBasinaSuLitre} L × {suCarpani:0.##} = {su} L (her sulamada).");
        kurallar.Add($"Gübre hesabı: {adet} bitki × {tur.BitkiBasinaGubreGram} g = {gubre} g ({gubreAralik} günde bir).");

        return new PlanOnerisi(aralik, su, gubreAralik, gubre, kurallar);
    }

    // TAKVİM ÜRETİMİ: Plan başlangıcından itibaren aralığa düşen günleri listeler
    public static List<TakvimOlayi> TakvimUret(SulamaPlani plan, string bitkiAd, string parselAd, DateTime bas, DateTime bit)
    {
        var liste = new List<TakvimOlayi>();
        for (var gun = bas.Date; gun <= bit.Date; gun = gun.AddDays(1))
        {
            if (gun < plan.BaslangicTarihi.Date) continue;
            int fark = (gun - plan.BaslangicTarihi.Date).Days;

            if (fark % plan.KacGundeBir == 0)
                liste.Add(new TakvimOlayi(gun, plan.SulamaSaati, "Sulama", bitkiAd, parselAd, $"{plan.SuMiktari:0.#} L"));

            if (plan.GubreAraligiGun is int g && g > 0 && fark % g == 0)
                liste.Add(new TakvimOlayi(gun, plan.SulamaSaati, "Gübreleme", bitkiAd, parselAd, $"{plan.GubreMiktariGram:0.#} g"));
        }
        return liste;
    }
}