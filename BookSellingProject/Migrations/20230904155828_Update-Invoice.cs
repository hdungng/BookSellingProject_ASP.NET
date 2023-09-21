using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSellingProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookInvoice");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItemInvoice");

            migrationBuilder.CreateTable(
                name: "BookInvoice",
                columns: table => new
                {
                    BooksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoicesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookInvoice", x => new { x.BooksId, x.InvoicesId });
                    table.ForeignKey(
                        name: "FK_BookInvoice_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookInvoice_Invoices_InvoicesId",
                        column: x => x.InvoicesId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookInvoice_InvoicesId",
                table: "BookInvoice",
                column: "InvoicesId");
        }
    }
}
