namespace AkilliSulama.Entities;

// Bitki türü özellikleri: her türün kendine özgü sulama ve güneş ihtiyacı var
public class BitkiTuru
{
    public int BitkiTuruId { get; set; }
    public string Ad { get; set; } = string.Empty;             // Domates, Marul...
    public string Kategori { get; set; } = string.Empty;       // Sebze, Yeşillik, Aromatik, Meyve
    public int OnerilenSulamaAraligiGun { get; set; }          // Kaç günde bir sulanmalı
    public decimal BitkiBasinaSuLitre { get; set; }            // Her sulamada bitki başına su (litre)
    public string GunesIhtiyaci { get; set; } = string.Empty;  // Tam güneş / Yarı gölge / Gölge

    // Bir türden birçok bitki olabilir (1-N)
    public ICollection<Bitki> Bitkiler { get; set; } = new List<Bitki>();
}