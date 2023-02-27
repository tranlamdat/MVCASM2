using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCASM2.Migrations
{
    public partial class Date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductImage",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Order_Id",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Order_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cus_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Order_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Order_Id",
                table: "Customers",
                column: "Order_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Order_Order_Id",
                table: "Customers",
                column: "Order_Id",
                principalTable: "Order",
                principalColumn: "Order_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Order_Order_Id",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Order_Id",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Order_Id",
                table: "Customers");

            migrationBuilder.AlterColumn<string>(
                name: "ProductImage",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
