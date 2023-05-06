using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLabeling.Data.Migrations
{
    public partial class Rename_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderPaymanetId",
                table: "Orders",
                newName: "OrderPaymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderPaymentId",
                table: "Orders",
                newName: "OrderPaymanetId");
        }
    }
}
