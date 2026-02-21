using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MocnyDom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BuildingManagers",
                table: "BuildingManagers");

            migrationBuilder.DropIndex(
                name: "IX_BuildingManagers_UserId",
                table: "BuildingManagers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BuildingManagers");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Sensors",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Events",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BuildingManagers",
                table: "BuildingManagers",
                columns: new[] { "UserId", "BuildingId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BuildingManagers",
                table: "BuildingManagers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Sensors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BuildingManagers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BuildingManagers",
                table: "BuildingManagers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingManagers_UserId",
                table: "BuildingManagers",
                column: "UserId");
        }
    }
}
