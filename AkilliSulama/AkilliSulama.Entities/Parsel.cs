namespace AkilliSulama.Entities;

public class Parsel
{
    public int ParselId { get; set; }
    public int KullaniciId { get; set; }                  // FK: sahibi
    public string Ad { get; set; } = string.Empty;
    public decimal Alan { get; set; }                     // m²
    public string ToprakTuru { get; set; } = string.Empty;

    // KONUM BİLGİSİ
    public string Il { get; set; } = string.Empty;
    public string? Ilce { get; set; }
    public string? Konum { get; set; }                    // Adres / açıklama
    public decimal? Enlem { get; set; }                   // -90 ile 90 arası
    public decimal? Boylam { get; set; }                  // -180 ile 180 arası

    public Kullanici Kullanici { get; set; } = null!;
    public ICollection<Bitki> Bitkiler { get; set; } = new List<Bitki>();
}