using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("250bcdbd-071a-44ae-ac48-ef378f27ab7e"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a0eb2dc8-c76e-4653-821a-e9392dac608d"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1b778710-1349-4fde-ba71-90e6a8d04138"), new DateTime(2024, 10, 5, 16, 58, 50, 263, DateTimeKind.Utc).AddTicks(4036), null, "Default customer role", "Customer", null },
                    { new Guid("38ef36c0-372c-4a8c-8680-dcd54d1d11a5"), new DateTime(2024, 10, 5, 16, 58, 50, 263, DateTimeKind.Utc).AddTicks(4041), null, "Administrator role", "Admin", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1b778710-1349-4fde-ba71-90e6a8d04138"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("38ef36c0-372c-4a8c-8680-dcd54d1d11a5"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("250bcdbd-071a-44ae-ac48-ef378f27ab7e"), new DateTime(2024, 10, 5, 16, 57, 57, 891, DateTimeKind.Utc).AddTicks(9580), null, "Administrator role", "Admin", null },
                    { new Guid("a0eb2dc8-c76e-4653-821a-e9392dac608d"), new DateTime(2024, 10, 5, 16, 57, 57, 891, DateTimeKind.Utc).AddTicks(9576), null, "Default customer role", "Customer", null }
                });
        }
    }
}
