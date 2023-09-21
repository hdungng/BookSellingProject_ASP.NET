using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSellingProject.Migrations
{
    /// <inheritdoc />
    public partial class fixedinvoicev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItemInvoice");

            migrationBuilder.AddColumn<Guid>(
                name: "InvoicesId",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_InvoicesId",
                table: "CartItems",
                column: "InvoicesId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Invoices_InvoicesId",
                table: "CartItems",
                column: "InvoicesId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Invoices_InvoicesId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_InvoicesId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "InvoicesId",
                table: "CartItems");

            migrationBuilder.CreateTable(
                name: "CartItemInvoice",
                columns: table => new
                {
                    CartItemsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoicesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemInvoice", x => new { x.CartItemsId, x.InvoicesId });
                    table.ForeignKey(
                        name: "FK_CartItemInvoice_CartItems_CartItemsId",
                        column: x => x.CartItemsId,
                        principalTable: "CartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItemInvoice_Invoices_InvoicesId",
                        column: x => x.InvoicesId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItemInvoice_InvoicesId",
                table: "CartItemInvoice",
                column: "InvoicesId");
        }
    }
}
