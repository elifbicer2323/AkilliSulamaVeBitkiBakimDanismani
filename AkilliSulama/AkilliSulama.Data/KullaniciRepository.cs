using AkilliSulama.Entities;
using Microsoft.EntityFrameworkCore;

namespace AkilliSulama.Data;

// Kullanıcı tablosu için veritabanı işlemleri
public class KullaniciRepository
{
    private readonly AkilliSulamaDbContext _db;
    public KullaniciRepository(AkilliSulamaDbContext db) => _db = db;

    public Task<Kullanici?> EpostaIleGetirAsync(string eposta) =>
        _db.Kullanicilar.FirstOrDefaultAsync(k => k.Eposta == eposta);

    public Task<bool> EpostaVarMiAsync(string eposta) =>
        _db.Kullanicilar.AnyAsync(k => k.Eposta == eposta);

    public async Task EkleAsync(Kullanici kullanici)
    {
        _db.Kullanicilar.Add(kullanici);
        await _db.SaveChangesAsync();
    }

    // Takip edilen (daha önce okunmuş) kullanıcıdaki değişiklikleri kaydeder
    public Task KaydetAsync() => _db.SaveChangesAsync();

    public Task<List<Kullanici>> TumuAsync() =>
        _db.Kullanicilar.AsNoTracking().OrderBy(k => k.KullaniciId).ToListAsync();
}