using AkilliSulama.Entities;
using Microsoft.EntityFrameworkCore;

namespace AkilliSulama.Data;

public class PlanRepository
{
    private readonly AkilliSulamaDbContext _db;
    public PlanRepository(AkilliSulamaDbContext db) => _db = db;

    // Bir bitkinin şu anki aktif planı
    public Task<SulamaPlani?> AktifPlanAsync(int bitkiId) =>
        _db.SulamaPlanlari.FirstOrDefaultAsync(p => p.BitkiId == bitkiId && p.AktifMi);

    // SAHİPLİK: Sadece bu kullanıcının parsellerindeki bitkilerin aktif planları (takvim için)
    public Task<List<SulamaPlani>> KullanicininAktifPlanlariAsync(int kullaniciId) =>
        _db.SulamaPlanlari
           .Include(p => p.Bitki).ThenInclude(b => b.Parsel)
           .Where(p => p.AktifMi && p.Bitki.Parsel.KullaniciId == kullaniciId)
           .AsNoTracking()
           .ToListAsync();

    // Not: Burada kaydetmiyoruz; servis birden fazla değişikliği TEK işlemde kaydeder
    public void Ekle(SulamaPlani plan) => _db.SulamaPlanlari.Add(plan);

    public Task KaydetAsync() => _db.SaveChangesAsync();
}