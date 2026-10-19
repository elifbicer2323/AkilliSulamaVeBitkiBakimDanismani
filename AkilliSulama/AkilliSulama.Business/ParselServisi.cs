using AkilliSulama.Data;
using AkilliSulama.Entities;

namespace AkilliSulama.Business;

// İş kuralları burada; veritabanı işini repository yapar
public class ParselServisi
{
    private readonly ParselRepository _repo;
    public ParselServisi(ParselRepository repo) => _repo = repo;

    public Task<List<Parsel>> ListeleAsync(int kullaniciId) => _repo.KullanicininParselleriAsync(kullaniciId);

    public Task<Parsel?> GetirAsync(int parselId) => _repo.GetirAsync(parselId);

    public Task<Parsel> EkleAsync(Parsel parsel)
    {
        Dogrula(parsel);
        return _repo.EkleAsync(parsel);
    }

    public Task<bool> GuncelleAsync(Parsel parsel)
    {
        Dogrula(parsel);
        return _repo.GuncelleAsync(parsel);
    }

    public Task<bool> SilAsync(int parselId) => _repo.SilAsync(parselId);

    // İŞ KURALLARI
    private static void Dogrula(Parsel p)
    {
        if (string.IsNullOrWhiteSpace(p.Ad))
            throw new ArgumentException("Parsel adı boş olamaz.");
        if (p.Alan <= 0)
            throw new ArgumentException("Parsel alanı 0'dan büyük olmalıdır.");
    }
}