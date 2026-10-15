namespace AkilliSulama.Entities;

public class Bitki
{
    public int BitkiId { get; set; }
    public int ParselId { get; set; }             // Yabancı anahtar (FK)
    public string Ad { get; set; } = string.Empty;
    public string Tur { get; set; } = string.Empty;
    public DateTime EkimTarihi { get; set; }

    public Parsel Parsel { get; set; } = null!;
    public ICollection<SulamaPlani> SulamaPlanlari { get; set; } = new List<SulamaPlani>();
}