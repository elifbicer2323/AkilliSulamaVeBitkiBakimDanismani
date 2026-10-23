using AkilliSulama.Entities;
using Microsoft.EntityFrameworkCore;

namespace AkilliSulama.Data;

public class ParselRepository
{
    private readonly AkilliSulamaDbContext _db;
    public ParselRepository(AkilliSulamaDbContext db) => _db = db;

    // READ: Kullanıcının parselleri, bitkileri ve türleriyle
    public Task<List<Parsel>> KullanicininParselleriAsync(int kullaniciId) =>
        _db.Parseller
           .Where(p => p.KullaniciId == kullaniciId)
           .Include(p => p.Bitkiler).ThenInclude(b => b.BitkiTuru)
           .AsNoTracking()
           .OrderBy(p => p.Ad)
           .ToListAsync();

    // SAHİPLİK: Parsel sadece SAHİBİNE getirilir; başkasınınsa null döner
    public Task<Parsel?> KullanicininParseliAsync(int parselId, int kullaniciId) =>
        _db.Parseller
           .Include(p => p.Bitkiler).ThenInclude(b => b.BitkiTuru)
           .FirstOrDefaultAsync(p => p.ParselId == parselId && p.KullaniciId == kullaniciId);

    public Task<bool> AyniIsimVarMiAsync(int kullaniciId, string ad, int? haricParselId = null) =>
        _db.Parseller.AnyAsync(p => p.KullaniciId == kullaniciId && p.Ad == ad && p.ParselId != (haricParselId ?? 0));

    public async Task EkleAsync(Parsel parsel)
    {
        _db.Parseller.Add(parsel);
        await _db.SaveChangesAsync();
    }

    public Task KaydetAsync() => _db.SaveChangesAsync();

    public async Task SilAsync(Parsel parsel)
    {
        _db.Parseller.Remove(parsel); // Bitkileri de silinir (Cascade)
        await _db.SaveChangesAsync();
    }
}
