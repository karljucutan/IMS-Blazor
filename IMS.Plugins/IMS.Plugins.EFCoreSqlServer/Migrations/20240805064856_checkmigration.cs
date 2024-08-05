using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Plugins.EFCoreSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class checkmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransaction_Inventories_InventoryId",
                table: "InventoryTransaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryTransaction",
                table: "InventoryTransaction");

            migrationBuilder.RenameTable(
                name: "InventoryTransaction",
                newName: "InventoryTransactions");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransaction_InventoryId",
                table: "InventoryTransactions",
                newName: "IX_InventoryTransactions_InventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryTransactions",
                table: "InventoryTransactions",
                column: "InventoryTransactionId");

            migrationBuilder.CreateTable(
                name: "ProductTransactions",
                columns: table => new
                {
                    ProductTransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesOrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    QuantityBefore = table.Column<int>(type: "int", nullable: false),
                    ActivityType = table.Column<int>(type: "int", nullable: false),
                    QuantityAfter = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoneBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTransactions", x => x.ProductTransactionId);
                    table.ForeignKey(
                        name: "FK_ProductTransactions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTransactions_ProductId",
                table: "ProductTransactions",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_Inventories_InventoryId",
                table: "InventoryTransactions",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_Inventories_InventoryId",
                table: "InventoryTransactions");

            migrationBuilder.DropTable(
                name: "ProductTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryTransactions",
                table: "InventoryTransactions");

            migrationBuilder.RenameTable(
                name: "InventoryTransactions",
                newName: "InventoryTransaction");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransactions_InventoryId",
                table: "InventoryTransaction",
                newName: "IX_InventoryTransaction_InventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryTransaction",
                table: "InventoryTransaction",
                column: "InventoryTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransaction_Inventories_InventoryId",
                table: "InventoryTransaction",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
