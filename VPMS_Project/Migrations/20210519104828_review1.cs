using Microsoft.EntityFrameworkCore.Migrations;

namespace VPMS_Project.Migrations
{
    public partial class review1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Communication",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeesId = table.Column<int>(nullable: false),
                    CommunicationVal = table.Column<int>(nullable: false),
                    GivenEmpId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communication_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkQuality",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(nullable: false),
                    ProjectName = table.Column<string>(nullable: true),
                    TaskName = table.Column<string>(nullable: true),
                    GivenEmp = table.Column<string>(nullable: true),
                    EmployeesId = table.Column<int>(nullable: false),
                    Quality = table.Column<int>(nullable: false),
                    GivenEmpId = table.Column<int>(nullable: false),
                    TasksId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkQuality", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkQuality_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkQuality_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Communication_EmployeesId",
                table: "Communication",
                column: "EmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkQuality_EmployeesId",
                table: "WorkQuality",
                column: "EmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkQuality_TasksId",
                table: "WorkQuality",
                column: "TasksId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Communication");

            migrationBuilder.DropTable(
                name: "WorkQuality");
        }
    }
}
