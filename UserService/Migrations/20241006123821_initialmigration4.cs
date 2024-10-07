using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { new Guid("44d5ab06-d392-4c7a-8544-e1d2d8d1a301"), new DateTime(2024, 10, 6, 12, 38, 21, 120, DateTimeKind.Utc).AddTicks(183), null, "Default customer role", "Customer", null },
                    { new Guid("75782bf0-dbc0-422a-9aaf-10d10ec086ab"), new DateTime(2024, 10, 6, 12, 38, 21, 120, DateTimeKind.Utc).AddTicks(186), null, "Administrator role", "Admin", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("44d5ab06-d392-4c7a-8544-e1d2d8d1a301"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("75782bf0-dbc0-422a-9aaf-10d10ec086ab"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1b778710-1349-4fde-ba71-90e6a8d04138"), new DateTime(2024, 10, 5, 16, 58, 50, 263, DateTimeKind.Utc).AddTicks(4036), null, "Default customer role", "Customer", null },
                    { new Guid("38ef36c0-372c-4a8c-8680-dcd54d1d11a5"), new DateTime(2024, 10, 5, 16, 58, 50, 263, DateTimeKind.Utc).AddTicks(4041), null, "Administrator role", "Admin", null }
                });
        }
    }
}
