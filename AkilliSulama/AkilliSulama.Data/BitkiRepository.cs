using AkilliSulama.Entities;
using Microsoft.EntityFrameworkCore;

namespace AkilliSulama.Data;

public class BitkiRepository
{
    private readonly AkilliSulamaDbContext _db;
    public BitkiRepository(AkilliSulamaDbContext db) => _db = db;

    public Task<List<BitkiTuru>> TurleriGetirAsync() =>
        _db.BitkiTurleri.AsNoTracking().OrderBy(t => t.Ad).ToListAsync();

    public Task<BitkiTuru?> TurGetirAsync(int bitkiTuruId) =>
        _db.BitkiTurleri.FirstOrDefaultAsync(t => t.BitkiTuruId == bitkiTuruId);

    // SAHİPLİK: Bitki, parseli üzerinden bu kullanıcıya ait mi?
    public Task<Bitki?> KullanicininBitkisiAsync(int bitkiId, int kullaniciId) =>
        _db.Bitkiler
           .Include(b => b.Parsel)
           .Include(b => b.BitkiTuru)
           .FirstOrDefaultAsync(b => b.BitkiId == bitkiId && b.Parsel.KullaniciId == kullaniciId);

    public Task<bool> AyniIsimVarMiAsync(int parselId, string ad, int? haricBitkiId = null) =>
        _db.Bitkiler.AnyAsync(b => b.ParselId == parselId && b.Ad == ad && b.BitkiId != (haricBitkiId ?? 0));

    public async Task EkleAsync(Bitki bitki)
    {
        _db.Bitkiler.Add(bitki);
        await _db.SaveChangesAsync();
    }

    public Task KaydetAsync() => _db.SaveChangesAsync();

    public async Task SilAsync(Bitki bitki)
    {
        _db.Bitkiler.Remove(bitki);
        await _db.SaveChangesAsync();
    }
}