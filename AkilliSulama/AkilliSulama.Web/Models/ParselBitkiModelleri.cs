using System.ComponentModel.DataAnnotations;
using AkilliSulama.Entities;

namespace AkilliSulama.Web.Models;

// Formdan gelen parsel bilgisi (doğrulama kurallarıyla)
public class ParselFormModel
{
    public int ParselId { get; set; }

    [Required(ErrorMessage = "Parsel adı zorunludur.")]
    [StringLength(100)]
    public string Ad { get; set; } = string.Empty;

    [Range(0.01, 1000000, ErrorMessage = "Alan 0,01 ile 1.000.000 m² arasında olmalıdır.")]
    public decimal Alan { get; set; }

    [StringLength(50)]
    public string? ToprakTuru { get; set; }

    [Required(ErrorMessage = "İl zorunludur.")]
    [StringLength(50)]
    public string Il { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Ilce { get; set; }

    [StringLength(200)]
    public string? Konum { get; set; }

    [Range(-90, 90, ErrorMessage = "Enlem -90 ile 90 arasında olmalıdır.")]
    public decimal? Enlem { get; set; }

    [Range(-180, 180, ErrorMessage = "Boylam -180 ile 180 arasında olmalıdır.")]
    public decimal? Boylam { get; set; }

    public Parsel EntityyeCevir() => new()
    {
        ParselId = ParselId,
        Ad = Ad,
        Alan = Alan,
        ToprakTuru = ToprakTuru ?? string.Empty,
        Il = Il,
        Ilce = Ilce,
        Konum = Konum,
        Enlem = Enlem,
        Boylam = Boylam
    };

    public static ParselFormModel EntitydenOlustur(Parsel p) => new()
    {
        ParselId = p.ParselId,
        Ad = p.Ad,
        Alan = p.Alan,
        ToprakTuru = p.ToprakTuru,
        Il = p.Il,
        Ilce = p.Ilce,
        Konum = p.Konum,
        Enlem = p.Enlem,
        Boylam = p.Boylam
    };
}

// Formdan gelen bitki bilgisi
public class BitkiFormModel
{
    public int BitkiId { get; set; }
    public int ParselId { get; set; }

    [Required(ErrorMessage = "Bitki adı zorunludur.")]
    [StringLength(100)]
    public string Ad { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Bir bitki türü seçin.")]
    public int BitkiTuruId { get; set; }

    [Range(1, 10000, ErrorMessage = "Adet 1 ile 10.000 arasında olmalıdır.")]
    public int Adet { get; set; } = 1;

    [DataType(DataType.Date)]
    public DateTime EkimTarihi { get; set; } = DateTime.Today;

    [StringLength(500)]
    public string? Notlar { get; set; }

    public Bitki EntityyeCevir() => new()
    {
        BitkiId = BitkiId,
        ParselId = ParselId,
        Ad = Ad,
        BitkiTuruId = BitkiTuruId,
        Adet = Adet,
        EkimTarihi = EkimTarihi,
        Notlar = Notlar
    };

    public static BitkiFormModel EntitydenOlustur(Bitki b) => new()
    {
        BitkiId = b.BitkiId,
        ParselId = b.ParselId,
        Ad = b.Ad,
        BitkiTuruId = b.BitkiTuruId,
        Adet = b.Adet,
        EkimTarihi = b.EkimTarihi,
        Notlar = b.Notlar
    };
}