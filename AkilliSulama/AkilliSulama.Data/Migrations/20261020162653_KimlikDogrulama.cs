using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AkilliSulama.Data.Migrations
{
    /// <inheritdoc />
    public partial class KimlikDogrulama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sifre",
                table: "Kullanicilar",
                newName: "SifreHash");

            migrationBuilder.AddColumn<int>(
                name: "BasarisizGirisSayisi",
                table: "Kullanicilar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "KilitBitisZamani",
                table: "Kullanicilar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Kullanicilar",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Kullanicilar",
                keyColumn: "KullaniciId",
                keyValue: 1,
                columns: new[] { "BasarisizGirisSayisi", "KilitBitisZamani", "Rol", "SifreHash" },
                values: new object[] { 0, null, "Yonetici", "100000.QWtpbGxpU3VsYW1hVHN0MQ==.Cf7JGTzY0P9ak3GZy3nznoRSJ/nJ3KB0yIQtUlyHQww=" });

            migrationBuilder.UpdateData(
                table: "Kullanicilar",
                keyColumn: "KullaniciId",
                keyValue: 2,
                columns: new[] { "BasarisizGirisSayisi", "KilitBitisZamani", "Rol", "SifreHash" },
                values: new object[] { 0, null, "Ciftci", "100000.QWtpbGxpU3VsYW1hRG5tMg==.VCH9gmDqyaNXzXW44a8POmR/rMAQtFFG8byLcDRmSTI=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasarisizGirisSayisi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "KilitBitisZamani",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Kullanicilar");

            migrationBuilder.RenameColumn(
                name: "SifreHash",
                table: "Kullanicilar",
                newName: "Sifre");

            migrationBuilder.UpdateData(
                table: "Kullanicilar",
                keyColumn: "KullaniciId",
                keyValue: 1,
                column: "Sifre",
                value: "GECICI-SIFRE");

            migrationBuilder.UpdateData(
                table: "Kullanicilar",
                keyColumn: "KullaniciId",
                keyValue: 2,
                column: "Sifre",
                value: "GECICI-SIFRE");
        }
    }
}
