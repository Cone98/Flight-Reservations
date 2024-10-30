using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Letovi.Web.Migrations
{
    /// <inheritdoc />
    public partial class _6_26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "61aa864b-cf91-404f-92dc-cbdc053d9e5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "525a3308-def7-498e-ba0b-8ba9d994522a", "AQAAAAIAAYagAAAAEIzUlaE6nNedNsE1IoaOd9jZwqjdfS0SdZBX6/ZZqEQrPVfVpD8Skr6LzQ5BsAkgKA==", "2d50a245-a38f-4136-9a2b-418e18030744" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "61aa864b-cf91-404f-92dc-cbdc053d9e5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4e213a03-708a-4145-bd34-a20345dcd705", "AQAAAAIAAYagAAAAEHpn6bwoYUrODVe9wnv2tnN0DRThXnRC4mzaMDBNTl1DAKk72oQ1RbP/YAE/goZjPA==", "423f5d89-500d-4780-a6cf-9b6190c24411" });
        }
    }
}
