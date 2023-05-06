using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLabeling.Data.Migrations
{
    public partial class Add_XmlSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LabeledXmlSource",
                table: "DataSet",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabeledXmlSource",
                table: "DataSet");
        }
    }
}
