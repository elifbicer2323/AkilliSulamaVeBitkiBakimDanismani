using AkilliSulama.Entities;
using Microsoft.EntityFrameworkCore;

namespace AkilliSulama.Data;

public class AkilliSulamaDbContext : DbContext
{
    public AkilliSulamaDbContext(DbContextOptions<AkilliSulamaDbContext> options) : base(options) { }

    // Her DbSet veritabanında bir tablo olur
    public DbSet<Kullanici> Kullanicilar => Set<Kullanici>();
    public DbSet<Parsel> Parseller => Set<Parsel>();
    public DbSet<BitkiTuru> BitkiTurleri => Set<BitkiTuru>();
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
            e.Property(x => x.SifreHash).IsRequired().HasMaxLength(200);
            e.Property(x => x.Rol).IsRequired().HasMaxLength(20);
            // İNDEKS + KISIT: Aynı e-posta ile iki kullanıcı olamaz, e-postayla arama hızlanır
            e.HasIndex(x => x.Eposta).IsUnique();
        });

        // ---------- PARSEL ----------
        modelBuilder.Entity<Parsel>(e =>
        {
            e.ToTable("Parseller", t =>
            {
                // KISITLAR: Alan pozitif olmalı, koordinatlar geçerli aralıkta olmalı
                t.HasCheckConstraint("CK_Parsel_Alan", "[Alan] > 0");
                t.HasCheckConstraint("CK_Parsel_Enlem", "[Enlem] IS NULL OR ([Enlem] >= -90 AND [Enlem] <= 90)");
                t.HasCheckConstraint("CK_Parsel_Boylam", "[Boylam] IS NULL OR ([Boylam] >= -180 AND [Boylam] <= 180)");
            });
            e.Property(x => x.Ad).IsRequired().HasMaxLength(100);
            e.Property(x => x.ToprakTuru).HasMaxLength(50);
            e.Property(x => x.Alan).HasPrecision(10, 2);
            e.Property(x => x.Il).IsRequired().HasMaxLength(50);
            e.Property(x => x.Ilce).HasMaxLength(50);
            e.Property(x => x.Konum).HasMaxLength(200);
            e.Property(x => x.Enlem).HasPrecision(9, 6);
            e.Property(x => x.Boylam).HasPrecision(9, 6);
            // İLİŞKİ (1-N): Bir kullanıcının çok parseli olur
            e.HasOne(x => x.Kullanici).WithMany(k => k.Parseller)
             .HasForeignKey(x => x.KullaniciId).OnDelete(DeleteBehavior.Cascade);
            // İNDEKS + KISIT: Bir kullanıcının aynı isimde iki parseli olamaz
            e.HasIndex(x => new { x.KullaniciId, x.Ad }).IsUnique();
        });

        // ---------- BITKI TURU ----------
        modelBuilder.Entity<BitkiTuru>(e =>
        {
            e.ToTable("BitkiTurleri", t =>
            {
                t.HasCheckConstraint("CK_BitkiTuru_Aralik", "[OnerilenSulamaAraligiGun] > 0");
                t.HasCheckConstraint("CK_BitkiTuru_Su", "[BitkiBasinaSuLitre] > 0");
            });
            e.Property(x => x.Ad).IsRequired().HasMaxLength(50);
            e.Property(x => x.Kategori).IsRequired().HasMaxLength(30);
            e.Property(x => x.GunesIhtiyaci).IsRequired().HasMaxLength(30);
            e.Property(x => x.BitkiBasinaSuLitre).HasPrecision(6, 2);
            e.HasIndex(x => x.Ad).IsUnique();
        });

        // ---------- BITKI ----------
        modelBuilder.Entity<Bitki>(e =>
        {
            e.ToTable("Bitkiler", t => t.HasCheckConstraint("CK_Bitki_Adet", "[Adet] > 0"));
            e.Property(x => x.Ad).IsRequired().HasMaxLength(100);
            e.Property(x => x.Notlar).HasMaxLength(500);
            // İLİŞKİ (1-N): Bir parselde çok bitki olur
            e.HasOne(x => x.Parsel).WithMany(p => p.Bitkiler)
             .HasForeignKey(x => x.ParselId).OnDelete(DeleteBehavior.Cascade);
            // İLİŞKİ (1-N): Bir türden çok bitki olur. Kullanılan tür silinemez (Restrict)
            e.HasOne(x => x.BitkiTuru).WithMany(t => t.Bitkiler)
             .HasForeignKey(x => x.BitkiTuruId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- SULAMA PLANI ----------
        modelBuilder.Entity<SulamaPlani>(e =>
        {
            e.ToTable("SulamaPlanlari", t => t.HasCheckConstraint("CK_Plan_Gun", "[KacGundeBir] > 0"));
            e.Property(x => x.SuMiktari).HasPrecision(6, 2);
            e.HasOne(x => x.Bitki).WithMany(b => b.SulamaPlanlari)
             .HasForeignKey(x => x.BitkiId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- UYARI ----------
        modelBuilder.Entity<Uyari>(e =>
        {
            e.ToTable("Uyarilar");
            e.Property(x => x.Mesaj).IsRequired().HasMaxLength(250);
            e.HasOne(x => x.SulamaPlani).WithMany(p => p.Uyarilar)
             .HasForeignKey(x => x.SulamaPlaniId).OnDelete(DeleteBehavior.Cascade);
            // NoAction: SQL Server aynı tabloya iki farklı silme yolu olmasına izin vermez
            e.HasOne(x => x.Kullanici).WithMany(k => k.Uyarilar)
             .HasForeignKey(x => x.KullaniciId).OnDelete(DeleteBehavior.NoAction);
            e.HasIndex(x => new { x.KullaniciId, x.OkunduMu });
        });

        OrnekVeriEkle(modelBuilder);
    }

    // ---------- ÖRNEK (SENTETİK) VERİ — gerçek kişi bilgisi yok (KVKK) ----------
    private static void OrnekVeriEkle(ModelBuilder mb)
    {
        // test@ornek.com -> Test123! (Yönetici) | deneme@ornek.com -> Deneme123! (Çiftçi)
        mb.Entity<Kullanici>().HasData(
            new Kullanici
            {
                KullaniciId = 1,
                Ad = "Test",
                Soyad = "Kullanici",
                Eposta = "test@ornek.com",
                SifreHash = "100000.QWtpbGxpU3VsYW1hVHN0MQ==.Cf7JGTzY0P9ak3GZy3nznoRSJ/nJ3KB0yIQtUlyHQww=",
                Rol = Roller.Yonetici,
                KayitTarihi = new DateTime(2026, 10, 1)
            },
            new Kullanici
            {
                KullaniciId = 2,
                Ad = "Deneme",
                Soyad = "Kisi",
                Eposta = "deneme@ornek.com",
                SifreHash = "100000.QWtpbGxpU3VsYW1hRG5tMg==.VCH9gmDqyaNXzXW44a8POmR/rMAQtFFG8byLcDRmSTI=",
                Rol = Roller.Ciftci,
                KayitTarihi = new DateTime(2026, 10, 2)
            }
        );

        mb.Entity<Parsel>().HasData(
            new Parsel { ParselId = 1, KullaniciId = 1, Ad = "Arka Bahçe", Alan = 50, ToprakTuru = "Tınlı", Il = "Bingöl", Ilce = "Merkez", Konum = "Örnek Mahalle", Enlem = 38.885500m, Boylam = 40.498300m },
            new Parsel { ParselId = 2, KullaniciId = 1, Ad = "Sera", Alan = 20, ToprakTuru = "Kumlu", Il = "Bingöl", Ilce = "Merkez", Konum = "Örnek Mahalle" },
            new Parsel { ParselId = 3, KullaniciId = 2, Ad = "Balkon", Alan = 5, ToprakTuru = "Saksı toprağı", Il = "Bingöl", Ilce = "Genç", Konum = "Örnek Sokak" }
        );

        // Bitki türü özellikleri (genel bahçecilik değerleri, yaklaşık)
        mb.Entity<BitkiTuru>().HasData(
            new BitkiTuru { BitkiTuruId = 1, Ad = "Domates", Kategori = "Sebze", OnerilenSulamaAraligiGun = 2, BitkiBasinaSuLitre = 3.0m, GunesIhtiyaci = "Tam güneş" },
            new BitkiTuru { BitkiTuruId = 2, Ad = "Biber", Kategori = "Sebze", OnerilenSulamaAraligiGun = 2, BitkiBasinaSuLitre = 2.5m, GunesIhtiyaci = "Tam güneş" },
            new BitkiTuru { BitkiTuruId = 3, Ad = "Marul", Kategori = "Yeşillik", OnerilenSulamaAraligiGun = 1, BitkiBasinaSuLitre = 1.5m, GunesIhtiyaci = "Yarı gölge" },
            new BitkiTuru { BitkiTuruId = 4, Ad = "Fesleğen", Kategori = "Aromatik", OnerilenSulamaAraligiGun = 3, BitkiBasinaSuLitre = 0.5m, GunesIhtiyaci = "Tam güneş" },
            new BitkiTuru { BitkiTuruId = 5, Ad = "Salatalık", Kategori = "Sebze", OnerilenSulamaAraligiGun = 1, BitkiBasinaSuLitre = 3.0m, GunesIhtiyaci = "Tam güneş" },
            new BitkiTuru { BitkiTuruId = 6, Ad = "Çilek", Kategori = "Meyve", OnerilenSulamaAraligiGun = 2, BitkiBasinaSuLitre = 1.0m, GunesIhtiyaci = "Tam güneş" },
            new BitkiTuru { BitkiTuruId = 7, Ad = "Nane", Kategori = "Aromatik", OnerilenSulamaAraligiGun = 2, BitkiBasinaSuLitre = 0.5m, GunesIhtiyaci = "Yarı gölge" },
            new BitkiTuru { BitkiTuruId = 8, Ad = "Patlıcan", Kategori = "Sebze", OnerilenSulamaAraligiGun = 2, BitkiBasinaSuLitre = 3.0m, GunesIhtiyaci = "Tam güneş" }
        );

        mb.Entity<Bitki>().HasData(
            new Bitki { BitkiId = 1, ParselId = 1, BitkiTuruId = 1, Ad = "Domates", Adet = 10, EkimTarihi = new DateTime(2026, 5, 1) },
            new Bitki { BitkiId = 2, ParselId = 1, BitkiTuruId = 2, Ad = "Biber", Adet = 8, EkimTarihi = new DateTime(2026, 5, 3) },
            new Bitki { BitkiId = 3, ParselId = 2, BitkiTuruId = 3, Ad = "Marul", Adet = 20, EkimTarihi = new DateTime(2026, 9, 10) },
            new Bitki { BitkiId = 4, ParselId = 3, BitkiTuruId = 4, Ad = "Fesleğen", Adet = 3, EkimTarihi = new DateTime(2026, 6, 15) }
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