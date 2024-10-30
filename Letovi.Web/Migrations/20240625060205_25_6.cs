using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Letovi.Web.Migrations
{
    /// <inheritdoc />
    public partial class _25_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "61aa864b-cf91-404f-92dc-cbdc053d9e5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4e213a03-708a-4145-bd34-a20345dcd705", "AQAAAAIAAYagAAAAEHpn6bwoYUrODVe9wnv2tnN0DRThXnRC4mzaMDBNTl1DAKk72oQ1RbP/YAE/goZjPA==", "423f5d89-500d-4780-a6cf-9b6190c24411" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "61aa864b-cf91-404f-92dc-cbdc053d9e5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "25dcd91f-ad0c-4d0c-93ad-79ee3cb296e0", "AQAAAAIAAYagAAAAECdexuyjSZenDJY+vx4Mat7BDMshkb6lFsPedEfsmkT0mG3PDWqiBTrFxELTdrEpvw==", "5390fbb5-af6d-4314-9954-63cd37e23585" });
        }
    }
}
