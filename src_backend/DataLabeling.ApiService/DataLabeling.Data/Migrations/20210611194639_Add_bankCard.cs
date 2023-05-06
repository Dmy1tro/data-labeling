using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLabeling.Data.Migrations
{
    public partial class Add_bankCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Performers",
                type: "decimal(16,6)",
                precision: 16,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Orders",
                type: "decimal(16,6)",
                precision: 16,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OrderPayments",
                type: "decimal(16,6)",
                precision: 16,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "JobPayments",
                type: "decimal(16,6)",
                precision: 16,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "BankCardNumber",
                table: "JobPayments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankCardNumber",
                table: "JobPayments");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Performers",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,6)",
                oldPrecision: 16,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,6)",
                oldPrecision: 16,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OrderPayments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,6)",
                oldPrecision: 16,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "JobPayments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,6)",
                oldPrecision: 16,
                oldScale: 6);
        }
    }
}
