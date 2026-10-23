namespace AkilliSulama.Entities;

public class Bitki
{
    public int BitkiId { get; set; }
    public int ParselId { get; set; }                 // FK: hangi parselde
    public int BitkiTuruId { get; set; }              // FK: hangi tür (Domates, Marul...)
    public string Ad { get; set; } = string.Empty;    // Kullanıcının verdiği ad, ör. "Domates - 1. sıra"
    public int Adet { get; set; } = 1;                // Kaç fide/bitki
    public DateTime EkimTarihi { get; set; }
    public string? Notlar { get; set; }

    public Parsel Parsel { get; set; } = null!;
    public BitkiTuru BitkiTuru { get; set; } = null!;
    public ICollection<SulamaPlani> SulamaPlanlari { get; set; } = new List<SulamaPlani>();
}