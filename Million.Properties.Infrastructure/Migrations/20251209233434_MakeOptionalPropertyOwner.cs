using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Million.Properties.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeOptionalPropertyOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PropertyTrace",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "PropertyTrace",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PropertyImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "PropertyImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdOwner",
                table: "Properties",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PropertyTrace");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "PropertyTrace");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PropertyImages");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "PropertyImages");

            migrationBuilder.AlterColumn<int>(
                name: "IdOwner",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
