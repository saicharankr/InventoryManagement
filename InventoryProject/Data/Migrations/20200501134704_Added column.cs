using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryProject.Data.Migrations
{
    public partial class Addedcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrcodeName",
                table: "InventoryItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrcodeName",
                table: "InventoryItems");
        }
    }
}
