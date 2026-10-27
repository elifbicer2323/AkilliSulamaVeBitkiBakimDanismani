using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AkilliSulama.Data.Migrations
{
    /// <inheritdoc />
    public partial class GubrelemePlani2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SulamaPlanlari_BitkiId",
                table: "SulamaPlanlari");

            migrationBuilder.AlterColumn<decimal>(
                name: "SuMiktari",
                table: "SulamaPlanlari",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2);

            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "SulamaPlanlari",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GubreAraligiGun",
                table: "SulamaPlanlari",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GubreMiktariGram",
                table: "SulamaPlanlari",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BitkiBasinaGubreGram",
                table: "BitkiTurleri",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "GubrelemeAraligiGun",
                table: "BitkiTurleri",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "BitkiTurleri",
                keyColumn: "BitkiTuruId",
                keyValue: 1,
                columns: new[] { "BitkiBasinaGubreGram", "GubrelemeAraligiGun" },
                values: new object[] { 10m, 14 });

            migrationBuilder.UpdateData(
                table: "BitkiTurleri",
                keyColumn: "BitkiTuruId",
                keyValue: 2,
                columns: new[] { "BitkiBasinaGubreGram", "GubrelemeAraligiGun" },
                values: new object[] { 8m, 14 });

            migrationBuilder.UpdateData(
                table: "BitkiTurleri",
                keyColumn: "BitkiTuruId",
                keyValue: 3,
                columns: new[] { "BitkiBasinaGubreGram", "GubrelemeAraligiGun" },
                values: new object[] { 5m, 21 });

            migrationBuilder.UpdateData(
                table: "BitkiTurleri",
                keyColumn: "BitkiTuruId",
                keyValue: 4,
                columns: new[] { "BitkiBasinaGubreGram", "GubrelemeAraligiGun" },
                values: new object[] { 2m, 30 });

            migrationBuilder.UpdateData(
                table: "BitkiTurleri",
                keyColumn: "BitkiTuruId",
                keyValue: 5,
                columns: new[] { "BitkiBasinaGubreGram", "GubrelemeAraligiGun" },
                values: new object[] { 10m, 14 });

            migrationBuilder.UpdateData(
                table: "BitkiTurleri",
                keyColumn: "BitkiTuruId",
                keyValue: 6,
                columns: new[] { "BitkiBasinaGubreGram", "GubrelemeAraligiGun" },
                values: new object[] { 5m, 21 });

            migrationBuilder.UpdateData(
                table: "BitkiTurleri",
                keyColumn: "BitkiTuruId",
                keyValue: 7,
                columns: new[] { "BitkiBasinaGubreGram", "GubrelemeAraligiGun" },
                values: new object[] { 2m, 30 });

            migrationBuilder.UpdateData(
                table: "BitkiTurleri",
                keyColumn: "BitkiTuruId",
                keyValue: 8,
                columns: new[] { "BitkiBasinaGubreGram", "GubrelemeAraligiGun" },
                values: new object[] { 10m, 14 });

            migrationBuilder.UpdateData(
                table: "SulamaPlanlari",
                keyColumn: "SulamaPlaniId",
                keyValue: 1,
                columns: new[] { "Aciklama", "GubreAraligiGun", "GubreMiktariGram", "SuMiktari" },
                values: new object[] { "Örnek plan", 14, 100m, 30m });

            migrationBuilder.UpdateData(
                table: "SulamaPlanlari",
                keyColumn: "SulamaPlaniId",
                keyValue: 2,
                columns: new[] { "Aciklama", "GubreAraligiGun", "GubreMiktariGram", "SuMiktari" },
                values: new object[] { "Örnek plan", 14, 64m, 20m });

            migrationBuilder.UpdateData(
                table: "SulamaPlanlari",
                keyColumn: "SulamaPlaniId",
                keyValue: 3,
                columns: new[] { "Aciklama", "GubreAraligiGun", "GubreMiktariGram", "SuMiktari" },
                values: new object[] { "Örnek plan", 21, 100m, 30m });

            migrationBuilder.UpdateData(
                table: "SulamaPlanlari",
                keyColumn: "SulamaPlaniId",
                keyValue: 4,
                columns: new[] { "Aciklama", "GubreAraligiGun", "GubreMiktariGram", "SuMiktari" },
                values: new object[] { "Örnek plan", 30, 6m, 1.5m });

            migrationBuilder.CreateIndex(
                name: "IX_SulamaPlanlari_BitkiId_AktifMi",
                table: "SulamaPlanlari",
                columns: new[] { "BitkiId", "AktifMi" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Plan_Gubre",
                table: "SulamaPlanlari",
                sql: "[GubreAraligiGun] IS NULL OR [GubreAraligiGun] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Plan_Su",
                table: "SulamaPlanlari",
                sql: "[SuMiktari] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BitkiTuru_GubreAralik",
                table: "BitkiTurleri",
                sql: "[GubrelemeAraligiGun] > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SulamaPlanlari_BitkiId_AktifMi",
                table: "SulamaPlanlari");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Plan_Gubre",
                table: "SulamaPlanlari");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Plan_Su",
                table: "SulamaPlanlari");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BitkiTuru_GubreAralik",
                table: "BitkiTurleri");

            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "SulamaPlanlari");

            migrationBuilder.DropColumn(
                name: "GubreAraligiGun",
                table: "SulamaPlanlari");

            migrationBuilder.DropColumn(
                name: "GubreMiktariGram",
                table: "SulamaPlanlari");

            migrationBuilder.DropColumn(
                name: "BitkiBasinaGubreGram",
                table: "BitkiTurleri");

            migrationBuilder.DropColumn(
                name: "GubrelemeAraligiGun",
                table: "BitkiTurleri");

            migrationBuilder.AlterColumn<decimal>(
                name: "SuMiktari",
                table: "SulamaPlanlari",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.UpdateData(
                table: "SulamaPlanlari",
                keyColumn: "SulamaPlaniId",
                keyValue: 1,
                column: "SuMiktari",
                value: 3.5m);

            migrationBuilder.UpdateData(
                table: "SulamaPlanlari",
                keyColumn: "SulamaPlaniId",
                keyValue: 2,
                column: "SuMiktari",
                value: 2.5m);

            migrationBuilder.UpdateData(
                table: "SulamaPlanlari",
                keyColumn: "SulamaPlaniId",
                keyValue: 3,
                column: "SuMiktari",
                value: 1.5m);

            migrationBuilder.UpdateData(
                table: "SulamaPlanlari",
                keyColumn: "SulamaPlaniId",
                keyValue: 4,
                column: "SuMiktari",
                value: 0.5m);

            migrationBuilder.CreateIndex(
                name: "IX_SulamaPlanlari_BitkiId",
                table: "SulamaPlanlari",
                column: "BitkiId");
        }
    }
}
