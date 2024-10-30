using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Letovi.Web.Migrations
{
    /// <inheritdoc />
    public partial class test4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_User_UserId1",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_UserId1",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "51bbff46-1080-455d-84c9-7b2de57b9f38",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "84e7f049-bc0d-45f9-aeba-4a26ef4eef1f", "AQAAAAIAAYagAAAAEDvvlWPR53v4cWLhi1hkCaUc5iJ0QeWgy0RaIzG6glFGl1nPeJwxK1vKyGmGoXfw5A==", "08386f56-0edc-4881-b657-36f7bb0fd092" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "51bbff46-1080-455d-84c9-7b2de57b9f38",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5be4a24e-4f02-43f3-b67f-ac39170a1ea0", "AQAAAAIAAYagAAAAEGCmsO8H4zTkkd/35n9dz79ijVej7t9m2ftfYA+USM+HMFkH1LQhUBhy4hu+WNDF4w==", "3df74445-4d4d-47a6-be95-727657e40d6d" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId1",
                table: "Reservations",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_User_UserId1",
                table: "Reservations",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
