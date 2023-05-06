using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLabeling.Data.Migrations
{
    public partial class Rename_descr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Orders",
                newName: "Requirements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Requirements",
                table: "Orders",
                newName: "Description");
        }
    }
}
