using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VPMS_Project.Migrations
{
    public partial class intergration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheetTask_Task_TaskId",
                table: "TimeSheetTask");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropIndex(
                name: "IX_TimeSheetTask_TaskId",
                table: "TimeSheetTask");

            migrationBuilder.AddColumn<int>(
                name: "TasksId",
                table: "TimeSheetTask",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectManagerId",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "Employees",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheetTask_TasksId",
                table: "TimeSheetTask",
                column: "TasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheetTask_Tasks_TasksId",
                table: "TimeSheetTask",
                column: "TasksId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheetTask_Tasks_TasksId",
                table: "TimeSheetTask");

            migrationBuilder.DropIndex(
                name: "IX_TimeSheetTask_TasksId",
                table: "TimeSheetTask");

            migrationBuilder.DropColumn(
                name: "TasksId",
                table: "TimeSheetTask");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "Employees");

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllocatedHours = table.Column<double>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeesId = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaskComplete = table.Column<bool>(type: "bit", nullable: true),
                    TimeSheet = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Task_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheetTask_TaskId",
                table: "TimeSheetTask",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_EmployeesId",
                table: "Task",
                column: "EmployeesId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheetTask_Task_TaskId",
                table: "TimeSheetTask",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
