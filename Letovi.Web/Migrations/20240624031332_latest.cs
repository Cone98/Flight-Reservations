using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Letovi.Web.Migrations
{
    /// <inheritdoc />
    public partial class Latest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a545458f-fe9e-4509-9644-078becddcc2a", "51bbff46-1080-455d-84c9-7b2de57b9f38" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "cc51a88b-231e-4a5a-8621-dfbfcb6f246e", "51bbff46-1080-455d-84c9-7b2de57b9f38" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "eecf777e-c3b5-452a-9235-7b8f1c0c051b", "51bbff46-1080-455d-84c9-7b2de57b9f38" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "51bbff46-1080-455d-84c9-7b2de57b9f38");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "61aa864b-cf91-404f-92dc-cbdc053d9e5f", 0, "25dcd91f-ad0c-4d0c-93ad-79ee3cb296e0", "nemanjatomic98@gmail.com", false, false, null, "NEMANJATOMIC98@GMAIL.COM", "NEMANJATOMIC98@GMAIL.COM", "AQAAAAIAAYagAAAAECdexuyjSZenDJY+vx4Mat7BDMshkb6lFsPedEfsmkT0mG3PDWqiBTrFxELTdrEpvw==", null, false, "5390fbb5-af6d-4314-9954-63cd37e23585", false, "nemanjatomic98@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "a545458f-fe9e-4509-9644-078becddcc2a", "61aa864b-cf91-404f-92dc-cbdc053d9e5f" },
                    { "cc51a88b-231e-4a5a-8621-dfbfcb6f246e", "61aa864b-cf91-404f-92dc-cbdc053d9e5f" },
                    { "eecf777e-c3b5-452a-9235-7b8f1c0c051b", "61aa864b-cf91-404f-92dc-cbdc053d9e5f" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a545458f-fe9e-4509-9644-078becddcc2a", "61aa864b-cf91-404f-92dc-cbdc053d9e5f" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "cc51a88b-231e-4a5a-8621-dfbfcb6f246e", "61aa864b-cf91-404f-92dc-cbdc053d9e5f" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "eecf777e-c3b5-452a-9235-7b8f1c0c051b", "61aa864b-cf91-404f-92dc-cbdc053d9e5f" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "61aa864b-cf91-404f-92dc-cbdc053d9e5f");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "51bbff46-1080-455d-84c9-7b2de57b9f38", 0, "b714036d-6da3-4a7e-a217-c5683ae161c3", "nemanjatomic98@gmail.com", false, false, null, "NEMANJATOMIC98@GMAIL.COM", "NEMANJATOMIC98@GMAIL.COM", "AQAAAAIAAYagAAAAEM5PGRt5RfqhoYZ1XKVNQsucXhuVsIXyRkqKrwYYLPntw/QFGt8TUJNIfwTtcUPQlA==", null, false, "339326e2-ec27-4692-8321-00ace5d4bc02", false, "nemanjatomic98@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "a545458f-fe9e-4509-9644-078becddcc2a", "51bbff46-1080-455d-84c9-7b2de57b9f38" },
                    { "cc51a88b-231e-4a5a-8621-dfbfcb6f246e", "51bbff46-1080-455d-84c9-7b2de57b9f38" },
                    { "eecf777e-c3b5-452a-9235-7b8f1c0c051b", "51bbff46-1080-455d-84c9-7b2de57b9f38" }
                });
        }
    }
}
