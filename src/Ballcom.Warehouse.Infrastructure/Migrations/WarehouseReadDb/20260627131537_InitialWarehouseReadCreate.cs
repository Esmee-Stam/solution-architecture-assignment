using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ballcom.Warehouse.Infrastructure.Migrations.WarehouseReadDb
{
    /// <inheritdoc />
    public partial class InitialWarehouseReadCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WarehouseOrderReadModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PickedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PackedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseOrderReadModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseOrderItemReadModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseOrderItemReadModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseOrderItemReadModels_WarehouseOrderReadModels_WarehouseOrderId",
                        column: x => x.WarehouseOrderId,
                        principalTable: "WarehouseOrderReadModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseOrderItemReadModels_WarehouseOrderId",
                table: "WarehouseOrderItemReadModels",
                column: "WarehouseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseOrderReadModels_OrderId",
                table: "WarehouseOrderReadModels",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseOrderItemReadModels");

            migrationBuilder.DropTable(
                name: "WarehouseOrderReadModels");
        }
    }
}
