using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryProject.Data.Migrations
{
    public partial class Addedforgikey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillinfoAddItemViewModel",
                columns: table => new
                {
                    ItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(nullable: true),
                    ItemName = table.Column<string>(nullable: true),
                    PurchaseDate = table.Column<DateTime>(nullable: false),
                    PurchaseHours = table.Column<DateTime>(nullable: false),
                    History = table.Column<string>(nullable: true),
                    BillInfo = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    UserGroup = table.Column<string>(nullable: true),
                    AssignedTo = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    QrCodeName = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillinfoAddItemViewModel", x => x.ItemID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillinfoAddItemViewModel");
        }
    }
}
