using AkilliSulama.Entities;
using Microsoft.EntityFrameworkCore;

namespace AkilliSulama.Data;

public class AkilliSulamaDbContext : DbContext
{
    public AkilliSulamaDbContext(DbContextOptions<AkilliSulamaDbContext> options) : base(options) { }

    // Her DbSet veritabanında bir tablo olur
    public DbSet<Kullanici> Kullanicilar => Set<Kullanici>();
    public DbSet<Parsel> Parseller => Set<Parsel>();
    public DbSet<Bitki> Bitkiler => Set<Bitki>();
    public DbSet<SulamaPlani> SulamaPlanlari => Set<SulamaPlani>();
    public DbSet<Uyari> Uyarilar => Set<Uyari>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ---------- KULLANICI ----------
        modelBuilder.Entity<Kullanici>(e =>
        {
            e.ToTable("Kullanicilar");
            e.Property(x => x.Ad).IsRequired().HasMaxLength(50);
            e.Property(x => x.Soyad).IsRequired().HasMaxLength(50);
            e.Property(x => x.Eposta).IsRequired().HasMaxLength(100);
            e.Property(x => x.Sifre).IsRequired().HasMaxLength(200);
            // İNDEKS + KISIT: Aynı e-posta ile iki kullanıcı olamaz, e-postayla arama hızlanır
            e.HasIndex(x => x.Eposta).IsUnique();
        });

        // ---------- PARSEL ----------
        modelBuilder.Entity<Parsel>(e =>
        {
            // KISIT: Alan 0'dan büyük olmalı
            e.ToTable("Parseller", t => t.HasCheckConstraint("CK_Parsel_Alan", "[Alan] > 0"));
            e.Property(x => x.Ad).IsRequired().HasMaxLength(100);
            e.Property(x => x.Konum).HasMaxLength(200);
            e.Property(x => x.ToprakTuru).HasMaxLength(50);
            e.Property(x => x.Alan).HasPrecision(10, 2);
            // İLİŞKİ (1-N): Bir kullanıcının çok parseli olur. Kullanıcı silinirse parselleri de silinir.
            e.HasOne(x => x.Kullanici).WithMany(k => k.Parseller)
             .HasForeignKey(x => x.KullaniciId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- BITKI ----------
        modelBuilder.Entity<Bitki>(e =>
        {
            e.ToTable("Bitkiler");
            e.Property(x => x.Ad).IsRequired().HasMaxLength(100);
            e.Property(x => x.Tur).HasMaxLength(50);
            // İLİŞKİ (1-N): Bir parselde çok bitki olur
            e.HasOne(x => x.Parsel).WithMany(p => p.Bitkiler)
             .HasForeignKey(x => x.ParselId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- SULAMA PLANI ----------
        modelBuilder.Entity<SulamaPlani>(e =>
        {
            // KISIT: En az 1 günde bir sulama
            e.ToTable("SulamaPlanlari", t => t.HasCheckConstraint("CK_Plan_Gun", "[KacGundeBir] > 0"));
            e.Property(x => x.SuMiktari).HasPrecision(6, 2);
            // İLİŞKİ (1-N): Bir bitkinin birden çok planı olabilir
            e.HasOne(x => x.Bitki).WithMany(b => b.SulamaPlanlari)
             .HasForeignKey(x => x.BitkiId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- UYARI ----------
        modelBuilder.Entity<Uyari>(e =>
        {
            e.ToTable("Uyarilar");
            e.Property(x => x.Mesaj).IsRequired().HasMaxLength(250);
            // İLİŞKİ (1-N): Uyarı bir plana bağlı
            e.HasOne(x => x.SulamaPlani).WithMany(p => p.Uyarilar)
             .HasForeignKey(x => x.SulamaPlaniId).OnDelete(DeleteBehavior.Cascade);
            // İLİŞKİ (1-N): Uyarı bir kullanıcıya gider.
            // NoAction: SQL Server aynı tabloya iki farklı silme yolu olmasına izin vermez
            e.HasOne(x => x.Kullanici).WithMany(k => k.Uyarilar)
             .HasForeignKey(x => x.KullaniciId).OnDelete(DeleteBehavior.NoAction);
            // İNDEKS: Bir kullanıcının okunmamış uyarılarını hızlı bulmak için
            e.HasIndex(x => new { x.KullaniciId, x.OkunduMu });
        });

        OrnekVeriEkle(modelBuilder);
    }

    // ---------- ÖRNEK (SENTETİK) VERİ — gerçek kişi bilgisi yok (KVKK) ----------
    private static void OrnekVeriEkle(ModelBuilder mb)
    {
        mb.Entity<Kullanici>().HasData(
            new Kullanici { KullaniciId = 1, Ad = "Test", Soyad = "Kullanici", Eposta = "test@ornek.com", Sifre = "GECICI-SIFRE", KayitTarihi = new DateTime(2026, 10, 1) },
            new Kullanici { KullaniciId = 2, Ad = "Deneme", Soyad = "Kisi", Eposta = "deneme@ornek.com", Sifre = "GECICI-SIFRE", KayitTarihi = new DateTime(2026, 10, 2) }
        );

        mb.Entity<Parsel>().HasData(
            new Parsel { ParselId = 1, KullaniciId = 1, Ad = "Arka Bahçe", Konum = "Örnek Mahalle", Alan = 50, ToprakTuru = "Tınlı" },
            new Parsel { ParselId = 2, KullaniciId = 1, Ad = "Sera", Konum = "Örnek Mahalle", Alan = 20, ToprakTuru = "Kumlu" },
            new Parsel { ParselId = 3, KullaniciId = 2, Ad = "Balkon", Konum = "Örnek Sokak", Alan = 5, ToprakTuru = "Saksı toprağı" }
        );

        mb.Entity<Bitki>().HasData(
            new Bitki { BitkiId = 1, ParselId = 1, Ad = "Domates", Tur = "Sebze", EkimTarihi = new DateTime(2026, 5, 1) },
            new Bitki { BitkiId = 2, ParselId = 1, Ad = "Biber", Tur = "Sebze", EkimTarihi = new DateTime(2026, 5, 3) },
            new Bitki { BitkiId = 3, ParselId = 2, Ad = "Marul", Tur = "Yeşillik", EkimTarihi = new DateTime(2026, 9, 10) },
            new Bitki { BitkiId = 4, ParselId = 3, Ad = "Fesleğen", Tur = "Aromatik", EkimTarihi = new DateTime(2026, 6, 15) }
        );

        mb.Entity<SulamaPlani>().HasData(
            new SulamaPlani { SulamaPlaniId = 1, BitkiId = 1, KacGundeBir = 2, SuMiktari = 3.5m, SulamaSaati = new TimeOnly(7, 0), BaslangicTarihi = new DateTime(2026, 10, 1), AktifMi = true },
            new SulamaPlani { SulamaPlaniId = 2, BitkiId = 2, KacGundeBir = 2, SuMiktari = 2.5m, SulamaSaati = new TimeOnly(7, 30), BaslangicTarihi = new DateTime(2026, 10, 1), AktifMi = true },
            new SulamaPlani { SulamaPlaniId = 3, BitkiId = 3, KacGundeBir = 1, SuMiktari = 1.5m, SulamaSaati = new TimeOnly(18, 0), BaslangicTarihi = new DateTime(2026, 10, 1), AktifMi = true },
            new SulamaPlani { SulamaPlaniId = 4, BitkiId = 4, KacGundeBir = 3, SuMiktari = 0.5m, SulamaSaati = new TimeOnly(9, 0), BaslangicTarihi = new DateTime(2026, 10, 1), AktifMi = true }
        );

        mb.Entity<Uyari>().HasData(
            new Uyari { UyariId = 1, KullaniciId = 1, SulamaPlaniId = 1, Mesaj = "Domatesi sulama zamanı geldi.", OlusturmaTarihi = new DateTime(2026, 10, 15, 7, 0, 0), OkunduMu = false },
            new Uyari { UyariId = 2, KullaniciId = 1, SulamaPlaniId = 3, Mesaj = "Marulu akşam sulamayı unutma.", OlusturmaTarihi = new DateTime(2026, 10, 15, 18, 0, 0), OkunduMu = true },
            new Uyari { UyariId = 3, KullaniciId = 2, SulamaPlaniId = 4, Mesaj = "Fesleğen için sulama günü.", OlusturmaTarihi = new DateTime(2026, 10, 16, 9, 0, 0), OkunduMu = false }
        );
    }
}