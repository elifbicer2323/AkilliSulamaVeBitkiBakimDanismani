using AkilliSulama.Entities;
using Microsoft.EntityFrameworkCore;

namespace AkilliSulama.Data;

// Parsel tablosu için temel CRUD işlemleri
public class ParselRepository
{
    private readonly AkilliSulamaDbContext _db;
    public ParselRepository(AkilliSulamaDbContext db) => _db = db;

    // CREATE: Yeni parsel ekler  (SQL: INSERT INTO Parseller ...)
    public async Task<Parsel> EkleAsync(Parsel parsel)
    {
        _db.Parseller.Add(parsel);
        await _db.SaveChangesAsync();
        return parsel;
    }

    // READ: Bir kullanıcının parsellerini bitkileriyle listeler  (SQL: SELECT ... WHERE KullaniciId = @id)
    public Task<List<Parsel>> KullanicininParselleriAsync(int kullaniciId) =>
        _db.Parseller
           .Where(p => p.KullaniciId == kullaniciId)
           .Include(p => p.Bitkiler)
           .AsNoTracking()
           .ToListAsync();

    // READ: Tek parseli getirir
    public Task<Parsel?> GetirAsync(int parselId) =>
        _db.Parseller.Include(p => p.Bitkiler).FirstOrDefaultAsync(p => p.ParselId == parselId);

    // UPDATE: Parsel bilgilerini günceller  (SQL: UPDATE Parseller SET ... WHERE ParselId = @id)
    public async Task<bool> GuncelleAsync(Parsel guncel)
    {
        var mevcut = await _db.Parseller.FindAsync(guncel.ParselId);
        if (mevcut is null) return false;

        mevcut.Ad = guncel.Ad;
        mevcut.Konum = guncel.Konum;
        mevcut.Alan = guncel.Alan;
        mevcut.ToprakTuru = guncel.ToprakTuru;
        await _db.SaveChangesAsync();
        return true;
    }

    // DELETE: Parseli siler  (SQL: DELETE FROM Parseller WHERE ParselId = @id)
    public async Task<bool> SilAsync(int parselId)
    {
        var mevcut = await _db.Parseller.FindAsync(parselId);
        if (mevcut is null) return false;

        _db.Parseller.Remove(mevcut);
        await _db.SaveChangesAsync();
        return true;
    }
}