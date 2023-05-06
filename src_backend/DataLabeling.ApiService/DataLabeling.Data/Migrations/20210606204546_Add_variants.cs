using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLabeling.Data.Migrations
{
    public partial class Add_variants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Variants",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Variants",
                table: "Orders");
        }
    }
}
