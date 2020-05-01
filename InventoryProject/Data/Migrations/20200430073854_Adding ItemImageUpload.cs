using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryProject.Data.Migrations
{
    public partial class AddingItemImageUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "InventoryItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "InventoryItems");
        }
    }
}