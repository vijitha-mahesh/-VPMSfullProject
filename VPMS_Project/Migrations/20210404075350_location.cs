using Microsoft.EntityFrameworkCore.Migrations;

namespace VPMS_Project.Migrations
{
    public partial class location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "InLongitude",
                table: "TimeTracker",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Inlatitude",
                table: "TimeTracker",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Inlocation",
                table: "TimeTracker",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OutLongitude",
                table: "TimeTracker",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Outlatitude",
                table: "TimeTracker",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Outlocation",
                table: "TimeTracker",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InLongitude",
                table: "TimeTracker");

            migrationBuilder.DropColumn(
                name: "Inlatitude",
                table: "TimeTracker");

            migrationBuilder.DropColumn(
                name: "Inlocation",
                table: "TimeTracker");

            migrationBuilder.DropColumn(
                name: "OutLongitude",
                table: "TimeTracker");

            migrationBuilder.DropColumn(
                name: "Outlatitude",
                table: "TimeTracker");

            migrationBuilder.DropColumn(
                name: "Outlocation",
                table: "TimeTracker");
        }
    }
}
