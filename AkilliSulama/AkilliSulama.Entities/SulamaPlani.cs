namespace AkilliSulama.Entities;

// Bir bitki için üretilen sulama + gübreleme planı
public class SulamaPlani
{
    public int SulamaPlaniId { get; set; }
    public int BitkiId { get; set; }                  // FK

    // SULAMA
    public int KacGundeBir { get; set; }
    public decimal SuMiktari { get; set; }            // Her sulamada toplam su (L)
    public TimeOnly SulamaSaati { get; set; }

    // GÜBRELEME
    public int? GubreAraligiGun { get; set; }
    public decimal? GubreMiktariGram { get; set; }    // Her gübrelemede toplam gübre (g)

    public DateTime BaslangicTarihi { get; set; }
    public bool AktifMi { get; set; } = true;
    public string? Aciklama { get; set; }             // Plan hangi kurallarla üretildi

    public Bitki Bitki { get; set; } = null!;
    public ICollection<Uyari> Uyarilar { get; set; } = new List<Uyari>();
}