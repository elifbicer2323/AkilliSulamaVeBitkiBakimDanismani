namespace AkilliSulama.Entities;

public class SulamaPlani
{
    public int SulamaPlaniId { get; set; }
    public int BitkiId { get; set; }              // Yabancı anahtar (FK)
    public int KacGundeBir { get; set; }
    public decimal SuMiktari { get; set; }        // litre
    public TimeOnly SulamaSaati { get; set; }
    public DateTime BaslangicTarihi { get; set; }
    public bool AktifMi { get; set; } = true;

    public Bitki Bitki { get; set; } = null!;
    public ICollection<Uyari> Uyarilar { get; set; } = new List<Uyari>();
}