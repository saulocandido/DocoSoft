using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DOCOsoft.UserManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("7705fa34-fc91-4e60-89b4-ac93c34f4a54"), new DateTime(2025, 2, 28, 10, 29, 24, 898, DateTimeKind.Utc).AddTicks(7351), null, "Admin" },
                    { new Guid("f9209550-5f6b-4733-bb73-836accf5913d"), new DateTime(2025, 2, 28, 10, 29, 24, 898, DateTimeKind.Utc).AddTicks(7361), null, "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7705fa34-fc91-4e60-89b4-ac93c34f4a54"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f9209550-5f6b-4733-bb73-836accf5913d"));
        }
    }
}
