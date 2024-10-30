using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Letovi.Web.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "51bbff46-1080-455d-84c9-7b2de57b9f38",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f2b7134d-ad93-4764-9d50-b113bbd355f5", "AQAAAAIAAYagAAAAEOavF7skw/5yyBh5xa+8VyG2ybTmk5XHeS7ezUiY99r/2+ZC5R22Nfisq7dsW2FU5A==", "f1c0371f-5405-438a-9184-dcab685d35b8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "51bbff46-1080-455d-84c9-7b2de57b9f38",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4e90f0eb-9cca-456f-9913-b363069a49d6", "AQAAAAIAAYagAAAAECw5/+8D1WgcFdXgqxJAT19xkz/FQ6821Z9iCwBhfdZvV+V8YMXeygDtvdD+2N9DHg==", "acc6d40f-b535-450a-b866-3500a187155d" });
        }
    }
}
