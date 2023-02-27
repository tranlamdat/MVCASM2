using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCASM2.Migrations
{
    public partial class AddpropOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Order_Order_Id",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Cus_Phone",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryLocal",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Order_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Orders_Order_Id",
                table: "Customers",
                column: "Order_Id",
                principalTable: "Orders",
                principalColumn: "Order_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Orders_Order_Id",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Cus_Phone",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryLocal",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Order_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Order_Order_Id",
                table: "Customers",
                column: "Order_Id",
                principalTable: "Order",
                principalColumn: "Order_Id");
        }
    }
}
