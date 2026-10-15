namespace AkilliSulama.Entities;

public class Uyari
{
    public int UyariId { get; set; }
    public int KullaniciId { get; set; }          // Yabancı anahtar (FK)
    public int SulamaPlaniId { get; set; }        // Yabancı anahtar (FK)
    public string Mesaj { get; set; } = string.Empty;
    public DateTime OlusturmaTarihi { get; set; }
    public bool OkunduMu { get; set; }

    public Kullanici Kullanici { get; set; } = null!;
    public SulamaPlani SulamaPlani { get; set; } = null!;
}