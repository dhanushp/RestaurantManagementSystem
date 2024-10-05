using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class UserModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("406309e7-57f2-4f79-924e-7b73a56c32e8"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("86dce64c-4b0c-4ea1-a233-04210be48c2e"));

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("2efdcf78-bef2-4b24-bae7-021a2dce3c50"), new DateTime(2024, 10, 4, 17, 44, 6, 420, DateTimeKind.Utc).AddTicks(106), null, "Default customer role", "Customer", null },
                    { new Guid("e08fd565-0c29-4465-959e-3a9474d0288d"), new DateTime(2024, 10, 4, 17, 44, 6, 420, DateTimeKind.Utc).AddTicks(109), null, "Administrator role", "Admin", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2efdcf78-bef2-4b24-bae7-021a2dce3c50"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e08fd565-0c29-4465-959e-3a9474d0288d"));

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("406309e7-57f2-4f79-924e-7b73a56c32e8"), new DateTime(2024, 10, 4, 6, 29, 17, 43, DateTimeKind.Utc).AddTicks(8358), null, "Default customer role", "Customer", null },
                    { new Guid("86dce64c-4b0c-4ea1-a233-04210be48c2e"), new DateTime(2024, 10, 4, 6, 29, 17, 43, DateTimeKind.Utc).AddTicks(8361), null, "Administrator role", "Admin", null }
                });
        }
    }
}
