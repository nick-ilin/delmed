using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Medicine_ref : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Medicines");

            migrationBuilder.RenameColumn(
                name: "RequiresPrescription",
                table: "Medicines",
                newName: "IsRequiredPrescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRequiredPrescription",
                table: "Medicines",
                newName: "RequiresPrescription");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Medicines",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Medicines",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
