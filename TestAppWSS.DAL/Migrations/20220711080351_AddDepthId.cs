using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAppWSS.DAL.Migrations
{
    public partial class AddDepthId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepthId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepthId",
                table: "Departments");
        }
    }
}
