using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AkilliSulama.Data.Migrations
{
    /// <inheritdoc />
    public partial class ParselBitkiModulu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Parseller_KullaniciId",
                table: "Parseller");

            migrationBuilder.DropColumn(
                name: "Tur",
                table: "Bitkiler");

            migrationBuilder.AlterColumn<string>(
                name: "Konum",
                table: "Parseller",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<decimal>(
                name: "Boylam",
                table: "Parseller",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Enlem",
                table: "Parseller",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Il",
                table: "Parseller",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ilce",
                table: "Parseller",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Adet",
                table: "Bitkiler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BitkiTuruId",
                table: "Bitkiler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notlar",
                table: "Bitkiler",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BitkiTurleri",
                columns: table => new
                {
                    BitkiTuruId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Kategori = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OnerilenSulamaAraligiGun = table.Column<int>(type: "int", nullable: false),
                    BitkiBasinaSuLitre = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    GunesIhtiyaci = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitkiTurleri", x => x.BitkiTuruId);
                    table.CheckConstraint("CK_BitkiTuru_Aralik", "[OnerilenSulamaAraligiGun] > 0");
                    table.CheckConstraint("CK_BitkiTuru_Su", "[BitkiBasinaSuLitre] > 0");
                });

            migrationBuilder.InsertData(
                table: "BitkiTurleri",
                columns: new[] { "BitkiTuruId", "Ad", "BitkiBasinaSuLitre", "GunesIhtiyaci", "Kategori", "OnerilenSulamaAraligiGun" },
                values: new object[,]
                {
                    { 1, "Domates", 3.0m, "Tam güneş", "Sebze", 2 },
                    { 2, "Biber", 2.5m, "Tam güneş", "Sebze", 2 },
                    { 3, "Marul", 1.5m, "Yarı gölge", "Yeşillik", 1 },
                    { 4, "Fesleğen", 0.5m, "Tam güneş", "Aromatik", 3 },
                    { 5, "Salatalık", 3.0m, "Tam güneş", "Sebze", 1 },
                    { 6, "Çilek", 1.0m, "Tam güneş", "Meyve", 2 },
                    { 7, "Nane", 0.5m, "Yarı gölge", "Aromatik", 2 },
                    { 8, "Patlıcan", 3.0m, "Tam güneş", "Sebze", 2 }
                });

            migrationBuilder.UpdateData(
                table: "Bitkiler",
                keyColumn: "BitkiId",
                keyValue: 1,
                columns: new[] { "Adet", "BitkiTuruId", "Notlar" },
                values: new object[] { 10, 1, null });

            migrationBuilder.UpdateData(
                table: "Bitkiler",
                keyColumn: "BitkiId",
                keyValue: 2,
                columns: new[] { "Adet", "BitkiTuruId", "Notlar" },
                values: new object[] { 8, 2, null });

            migrationBuilder.UpdateData(
                table: "Bitkiler",
                keyColumn: "BitkiId",
                keyValue: 3,
                columns: new[] { "Adet", "BitkiTuruId", "Notlar" },
                values: new object[] { 20, 3, null });

            migrationBuilder.UpdateData(
                table: "Bitkiler",
                keyColumn: "BitkiId",
                keyValue: 4,
                columns: new[] { "Adet", "BitkiTuruId", "Notlar" },
                values: new object[] { 3, 4, null });

            migrationBuilder.UpdateData(
                table: "Parseller",
                keyColumn: "ParselId",
                keyValue: 1,
                columns: new[] { "Boylam", "Enlem", "Il", "Ilce" },
                values: new object[] { 40.498300m, 38.885500m, "Bingöl", "Merkez" });

            migrationBuilder.UpdateData(
                table: "Parseller",
                keyColumn: "ParselId",
                keyValue: 2,
                columns: new[] { "Boylam", "Enlem", "Il", "Ilce" },
                values: new object[] { null, null, "Bingöl", "Merkez" });

            migrationBuilder.UpdateData(
                table: "Parseller",
                keyColumn: "ParselId",
                keyValue: 3,
                columns: new[] { "Boylam", "Enlem", "Il", "Ilce" },
                values: new object[] { null, null, "Bingöl", "Genç" });

            migrationBuilder.CreateIndex(
                name: "IX_Parseller_KullaniciId_Ad",
                table: "Parseller",
                columns: new[] { "KullaniciId", "Ad" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Parsel_Boylam",
                table: "Parseller",
                sql: "[Boylam] IS NULL OR ([Boylam] >= -180 AND [Boylam] <= 180)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Parsel_Enlem",
                table: "Parseller",
                sql: "[Enlem] IS NULL OR ([Enlem] >= -90 AND [Enlem] <= 90)");

            migrationBuilder.CreateIndex(
                name: "IX_Bitkiler_BitkiTuruId",
                table: "Bitkiler",
                column: "BitkiTuruId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Bitki_Adet",
                table: "Bitkiler",
                sql: "[Adet] > 0");

            migrationBuilder.CreateIndex(
                name: "IX_BitkiTurleri_Ad",
                table: "BitkiTurleri",
                column: "Ad",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bitkiler_BitkiTurleri_BitkiTuruId",
                table: "Bitkiler",
                column: "BitkiTuruId",
                principalTable: "BitkiTurleri",
                principalColumn: "BitkiTuruId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bitkiler_BitkiTurleri_BitkiTuruId",
                table: "Bitkiler");

            migrationBuilder.DropTable(
                name: "BitkiTurleri");

            migrationBuilder.DropIndex(
                name: "IX_Parseller_KullaniciId_Ad",
                table: "Parseller");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Parsel_Boylam",
                table: "Parseller");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Parsel_Enlem",
                table: "Parseller");

            migrationBuilder.DropIndex(
                name: "IX_Bitkiler_BitkiTuruId",
                table: "Bitkiler");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Bitki_Adet",
                table: "Bitkiler");

            migrationBuilder.DropColumn(
                name: "Boylam",
                table: "Parseller");

            migrationBuilder.DropColumn(
                name: "Enlem",
                table: "Parseller");

            migrationBuilder.DropColumn(
                name: "Il",
                table: "Parseller");

            migrationBuilder.DropColumn(
                name: "Ilce",
                table: "Parseller");

            migrationBuilder.DropColumn(
                name: "Adet",
                table: "Bitkiler");

            migrationBuilder.DropColumn(
                name: "BitkiTuruId",
                table: "Bitkiler");

            migrationBuilder.DropColumn(
                name: "Notlar",
                table: "Bitkiler");

            migrationBuilder.AlterColumn<string>(
                name: "Konum",
                table: "Parseller",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tur",
                table: "Bitkiler",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Bitkiler",
                keyColumn: "BitkiId",
                keyValue: 1,
                column: "Tur",
                value: "Sebze");

            migrationBuilder.UpdateData(
                table: "Bitkiler",
                keyColumn: "BitkiId",
                keyValue: 2,
                column: "Tur",
                value: "Sebze");

            migrationBuilder.UpdateData(
                table: "Bitkiler",
                keyColumn: "BitkiId",
                keyValue: 3,
                column: "Tur",
                value: "Yeşillik");

            migrationBuilder.UpdateData(
                table: "Bitkiler",
                keyColumn: "BitkiId",
                keyValue: 4,
                column: "Tur",
                value: "Aromatik");

            migrationBuilder.CreateIndex(
                name: "IX_Parseller_KullaniciId",
                table: "Parseller",
                column: "KullaniciId");
        }
    }
}
