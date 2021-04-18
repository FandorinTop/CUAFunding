using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace CUAFunding.DataAccess.Data.ApplicationDb
{
    public partial class AddProjectPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Projects",
                type: "geography",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Projects");
        }
    }
}
