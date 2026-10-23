namespace AkilliSulama.Entities;

public class Kullanici
{
    public int KullaniciId { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Soyad { get; set; } = string.Empty;
    public string Eposta { get; set; } = string.Empty;

    // GÜVENLİK: Parolanın kendisi DEĞİL, tuzlanmış hash'i saklanır
    public string SifreHash { get; set; } = string.Empty;

    // Rol tabanlı erişim: "Ciftci" veya "Yonetici"
    public string Rol { get; set; } = Roller.Ciftci;

    public DateTime KayitTarihi { get; set; }

    // Kaba kuvvet (brute force) koruması: art arda hatalı girişte hesap geçici kilitlenir
    public int BasarisizGirisSayisi { get; set; }
    public DateTime? KilitBitisZamani { get; set; }

    // Bir kullanıcının birden çok parseli ve uyarısı olur (1-N)
    public ICollection<Parsel> Parseller { get; set; } = new List<Parsel>();
    public ICollection<Uyari> Uyarilar { get; set; } = new List<Uyari>();
}