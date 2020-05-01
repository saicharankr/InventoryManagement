using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryProject.Data.Migrations
{
    public partial class addingcatagorygroupassignedTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "InventoryItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "InventoryItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserGroup",
                table: "InventoryItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "UserGroup",
                table: "InventoryItems");
        }
    }
}