using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryProject.Data.Migrations
{
    public partial class additemstoinvetoryfrombill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfItems",
                table: "BillInfo");

            migrationBuilder.RenameColumn(
                name: "QrcodeName",
                table: "InventoryItems",
                newName: "QrCodeName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QrCodeName",
                table: "InventoryItems",
                newName: "QrcodeName");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfItems",
                table: "BillInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
