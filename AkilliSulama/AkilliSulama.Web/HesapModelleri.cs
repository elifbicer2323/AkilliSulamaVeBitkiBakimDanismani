using System.ComponentModel.DataAnnotations;

namespace AkilliSulama.Web.Models;

public class GirisModel
{
    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta girin.")]
    public string Eposta { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola zorunludur.")]
    [DataType(DataType.Password)]
    public string Sifre { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}

public class KayitModel
{
    [Required(ErrorMessage = "Ad zorunludur.")]
    public string Ad { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad zorunludur.")]
    public string Soyad { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta girin.")]
    public string Eposta { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola zorunludur.")]
    [DataType(DataType.Password)]
    public string Sifre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola tekrarı zorunludur.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Sifre), ErrorMessage = "Parolalar eşleşmiyor.")]
    public string SifreTekrar { get; set; } = string.Empty;
}