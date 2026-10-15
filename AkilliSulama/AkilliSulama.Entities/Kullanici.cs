namespace AkilliSulama.Entities;

public class Kullanici
{
    public int KullaniciId { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Soyad { get; set; } = string.Empty;
    public string Eposta { get; set; } = string.Empty;
    public string Sifre { get; set; } = string.Empty;
    public DateTime KayitTarihi { get; set; }

    // Bir kullanıcının birden çok parseli ve uyarısı olur (1-N)
    public ICollection<Parsel> Parseller { get; set; } = new List<Parsel>();
    public ICollection<Uyari> Uyarilar { get; set; } = new List<Uyari>();
}