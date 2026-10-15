namespace AkilliSulama.Entities;

public class Parsel
{
    public int ParselId { get; set; }
    public int KullaniciId { get; set; }          // Yabancı anahtar (FK)
    public string Ad { get; set; } = string.Empty;
    public string Konum { get; set; } = string.Empty;
    public decimal Alan { get; set; }             // m²
    public string ToprakTuru { get; set; } = string.Empty;

    public Kullanici Kullanici { get; set; } = null!;
    public ICollection<Bitki> Bitkiler { get; set; } = new List<Bitki>();
}