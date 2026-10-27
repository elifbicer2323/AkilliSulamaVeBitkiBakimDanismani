namespace AkilliSulama.Entities;

// Bitki türü özellikleri: sulama ve gübreleme ihtiyacı
public class BitkiTuru
{
    public int BitkiTuruId { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Kategori { get; set; } = string.Empty;
    public int OnerilenSulamaAraligiGun { get; set; }          // Kaç günde bir sulanmalı
    public decimal BitkiBasinaSuLitre { get; set; }            // Her sulamada bitki başına su (L)
    public string GunesIhtiyaci { get; set; } = string.Empty;

    // GÜBRELEME
    public int GubrelemeAraligiGun { get; set; }               // Kaç günde bir gübre
    public decimal BitkiBasinaGubreGram { get; set; }          // Her gübrelemede bitki başına (g)

    public ICollection<Bitki> Bitkiler { get; set; } = new List<Bitki>();
}