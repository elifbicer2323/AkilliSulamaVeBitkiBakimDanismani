using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AkilliSulama.Data.Migrations
{
    /// <inheritdoc />
    public partial class IlkKurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    KullaniciId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Eposta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sifre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciId);
                });

            migrationBuilder.CreateTable(
                name: "Parseller",
                columns: table => new
                {
                    ParselId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    Ad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Konum = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Alan = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ToprakTuru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parseller", x => x.ParselId);
                    table.CheckConstraint("CK_Parsel_Alan", "[Alan] > 0");
                    table.ForeignKey(
                        name: "FK_Parseller_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bitkiler",
                columns: table => new
                {
                    BitkiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParselId = table.Column<int>(type: "int", nullable: false),
                    Ad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tur = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EkimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bitkiler", x => x.BitkiId);
                    table.ForeignKey(
                        name: "FK_Bitkiler_Parseller_ParselId",
                        column: x => x.ParselId,
                        principalTable: "Parseller",
                        principalColumn: "ParselId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SulamaPlanlari",
                columns: table => new
                {
                    SulamaPlaniId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BitkiId = table.Column<int>(type: "int", nullable: false),
                    KacGundeBir = table.Column<int>(type: "int", nullable: false),
                    SuMiktari = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    SulamaSaati = table.Column<TimeOnly>(type: "time", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SulamaPlanlari", x => x.SulamaPlaniId);
                    table.CheckConstraint("CK_Plan_Gun", "[KacGundeBir] > 0");
                    table.ForeignKey(
                        name: "FK_SulamaPlanlari_Bitkiler_BitkiId",
                        column: x => x.BitkiId,
                        principalTable: "Bitkiler",
                        principalColumn: "BitkiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Uyarilar",
                columns: table => new
                {
                    UyariId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    SulamaPlaniId = table.Column<int>(type: "int", nullable: false),
                    Mesaj = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OkunduMu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uyarilar", x => x.UyariId);
                    table.ForeignKey(
                        name: "FK_Uyarilar_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciId");
                    table.ForeignKey(
                        name: "FK_Uyarilar_SulamaPlanlari_SulamaPlaniId",
                        column: x => x.SulamaPlaniId,
                        principalTable: "SulamaPlanlari",
                        principalColumn: "SulamaPlaniId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Kullanicilar",
                columns: new[] { "KullaniciId", "Ad", "Eposta", "KayitTarihi", "Sifre", "Soyad" },
                values: new object[,]
                {
                    { 1, "Test", "test@ornek.com", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "GECICI-SIFRE", "Kullanici" },
                    { 2, "Deneme", "deneme@ornek.com", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "GECICI-SIFRE", "Kisi" }
                });

            migrationBuilder.InsertData(
                table: "Parseller",
                columns: new[] { "ParselId", "Ad", "Alan", "Konum", "KullaniciId", "ToprakTuru" },
                values: new object[,]
                {
                    { 1, "Arka Bahçe", 50m, "Örnek Mahalle", 1, "Tınlı" },
                    { 2, "Sera", 20m, "Örnek Mahalle", 1, "Kumlu" },
                    { 3, "Balkon", 5m, "Örnek Sokak", 2, "Saksı toprağı" }
                });

            migrationBuilder.InsertData(
                table: "Bitkiler",
                columns: new[] { "BitkiId", "Ad", "EkimTarihi", "ParselId", "Tur" },
                values: new object[,]
                {
                    { 1, "Domates", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Sebze" },
                    { 2, "Biber", new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Sebze" },
                    { 3, "Marul", new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Yeşillik" },
                    { 4, "Fesleğen", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Aromatik" }
                });

            migrationBuilder.InsertData(
                table: "SulamaPlanlari",
                columns: new[] { "SulamaPlaniId", "AktifMi", "BaslangicTarihi", "BitkiId", "KacGundeBir", "SuMiktari", "SulamaSaati" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 3.5m, new TimeOnly(7, 0, 0) },
                    { 2, true, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, 2.5m, new TimeOnly(7, 30, 0) },
                    { 3, true, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, 1.5m, new TimeOnly(18, 0, 0) },
                    { 4, true, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, 0.5m, new TimeOnly(9, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "Uyarilar",
                columns: new[] { "UyariId", "KullaniciId", "Mesaj", "OkunduMu", "OlusturmaTarihi", "SulamaPlaniId" },
                values: new object[,]
                {
                    { 1, 1, "Domatesi sulama zamanı geldi.", false, new DateTime(2026, 10, 15, 7, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 1, "Marulu akşam sulamayı unutma.", true, new DateTime(2026, 10, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 3, 2, "Fesleğen için sulama günü.", false, new DateTime(2026, 10, 16, 9, 0, 0, 0, DateTimeKind.Unspecified), 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bitkiler_ParselId",
                table: "Bitkiler",
                column: "ParselId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_Eposta",
                table: "Kullanicilar",
                column: "Eposta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parseller_KullaniciId",
                table: "Parseller",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_SulamaPlanlari_BitkiId",
                table: "SulamaPlanlari",
                column: "BitkiId");

            migrationBuilder.CreateIndex(
                name: "IX_Uyarilar_KullaniciId_OkunduMu",
                table: "Uyarilar",
                columns: new[] { "KullaniciId", "OkunduMu" });

            migrationBuilder.CreateIndex(
                name: "IX_Uyarilar_SulamaPlaniId",
                table: "Uyarilar",
                column: "SulamaPlaniId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Uyarilar");

            migrationBuilder.DropTable(
                name: "SulamaPlanlari");

            migrationBuilder.DropTable(
                name: "Bitkiler");

            migrationBuilder.DropTable(
                name: "Parseller");

            migrationBuilder.DropTable(
                name: "Kullanicilar");
        }
    }
}
