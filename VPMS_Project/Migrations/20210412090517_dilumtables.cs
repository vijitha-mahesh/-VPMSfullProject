using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VPMS_Project.Migrations
{
    public partial class dilumtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreSalesCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    contactNumber = table.Column<int>(nullable: false),
                    address = table.Column<string>(nullable: true),
                    emailAddress = table.Column<string>(nullable: true),
                    projectCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSalesCustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreSalesDeleteBudgets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(nullable: true),
                    LbHou = table.Column<int>(nullable: false),
                    LbRate = table.Column<int>(nullable: false),
                    MtUnit = table.Column<int>(nullable: false),
                    MtunitPr = table.Column<int>(nullable: false),
                    cost = table.Column<int>(nullable: false),
                    BudgetCost = table.Column<int>(nullable: false),
                    Actual = table.Column<int>(nullable: false),
                    Under = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSalesDeleteBudgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreSalesDeletedCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    contactNumber = table.Column<int>(nullable: false),
                    address = table.Column<string>(nullable: true),
                    emailAddress = table.Column<string>(nullable: true),
                    projectCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSalesDeletedCustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreSalesDeletedProjects",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProjectType = table.Column<string>(nullable: true),
                    value = table.Column<double>(nullable: false),
                    ProjectBudget = table.Column<double>(nullable: false),
                    CustomersId = table.Column<int>(nullable: false),
                    projectManagerId = table.Column<int>(nullable: false),
                    startDate = table.Column<DateTime>(nullable: false),
                    endDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSalesDeletedProjects", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PreSalesDeletedTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Assignee = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSalesDeletedTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreSalesProjectManagers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Descrption = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSalesProjectManagers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreSalesProjects",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProjectType = table.Column<string>(nullable: true),
                    value = table.Column<double>(nullable: false),
                    ProjectBudget = table.Column<double>(nullable: false),
                    CustomersId = table.Column<int>(nullable: false),
                    projectManagerId = table.Column<int>(nullable: false),
                    startDate = table.Column<DateTime>(nullable: false),
                    endDate = table.Column<DateTime>(nullable: false),
                    PreSalesCustomersId = table.Column<int>(nullable: true),
                    PreSalesprojectManagerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSalesProjects", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PreSalesProjects_PreSalesCustomers_PreSalesCustomersId",
                        column: x => x.PreSalesCustomersId,
                        principalTable: "PreSalesCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreSalesProjects_PreSalesProjectManagers_PreSalesprojectManagerId",
                        column: x => x.PreSalesprojectManagerId,
                        principalTable: "PreSalesProjectManagers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreSalesBudget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LbHou = table.Column<int>(nullable: false),
                    LbRate = table.Column<int>(nullable: false),
                    MtUnit = table.Column<int>(nullable: false),
                    MtunitPr = table.Column<int>(nullable: false),
                    cost = table.Column<int>(nullable: false),
                    BudgetCost = table.Column<int>(nullable: false),
                    Actual = table.Column<int>(nullable: false),
                    Under = table.Column<int>(nullable: false),
                    ProjectsID = table.Column<int>(nullable: false),
                    PreSalesProjectsID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSalesBudget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreSalesBudget_PreSalesProjects_PreSalesProjectsID",
                        column: x => x.PreSalesProjectsID,
                        principalTable: "PreSalesProjects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreSalescollection",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    value = table.Column<int>(nullable: false),
                    ProjecstID = table.Column<int>(nullable: false),
                    PreSalesProjectsID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSalescollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreSalescollection_PreSalesProjects_PreSalesProjectsID",
                        column: x => x.PreSalesProjectsID,
                        principalTable: "PreSalesProjects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreSalesDocument",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScopeDocumentUrl = table.Column<string>(nullable: true),
                    ActionPlanUrl = table.Column<string>(nullable: true),
                    TimePlanUrl = table.Column<string>(nullable: true),
                    ProjectsID = table.Column<int>(nullable: false),
                    PreSalesProjectsID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSalesDocument", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PreSalesDocument_PreSalesProjects_PreSalesProjectsID",
                        column: x => x.PreSalesProjectsID,
                        principalTable: "PreSalesProjects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreSalesTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Assignee = table.Column<string>(nullable: true),
                    ProjectsID = table.Column<int>(nullable: false),
                    PreSalesProjectsID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSalesTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreSalesTasks_PreSalesProjects_PreSalesProjectsID",
                        column: x => x.PreSalesProjectsID,
                        principalTable: "PreSalesProjects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreSalesBudget_PreSalesProjectsID",
                table: "PreSalesBudget",
                column: "PreSalesProjectsID");

            migrationBuilder.CreateIndex(
                name: "IX_PreSalescollection_PreSalesProjectsID",
                table: "PreSalescollection",
                column: "PreSalesProjectsID");

            migrationBuilder.CreateIndex(
                name: "IX_PreSalesDocument_PreSalesProjectsID",
                table: "PreSalesDocument",
                column: "PreSalesProjectsID");

            migrationBuilder.CreateIndex(
                name: "IX_PreSalesProjects_PreSalesCustomersId",
                table: "PreSalesProjects",
                column: "PreSalesCustomersId");

            migrationBuilder.CreateIndex(
                name: "IX_PreSalesProjects_PreSalesprojectManagerId",
                table: "PreSalesProjects",
                column: "PreSalesprojectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_PreSalesTasks_PreSalesProjectsID",
                table: "PreSalesTasks",
                column: "PreSalesProjectsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreSalesBudget");

            migrationBuilder.DropTable(
                name: "PreSalescollection");

            migrationBuilder.DropTable(
                name: "PreSalesDeleteBudgets");

            migrationBuilder.DropTable(
                name: "PreSalesDeletedCustomers");

            migrationBuilder.DropTable(
                name: "PreSalesDeletedProjects");

            migrationBuilder.DropTable(
                name: "PreSalesDeletedTasks");

            migrationBuilder.DropTable(
                name: "PreSalesDocument");

            migrationBuilder.DropTable(
                name: "PreSalesTasks");

            migrationBuilder.DropTable(
                name: "PreSalesProjects");

            migrationBuilder.DropTable(
                name: "PreSalesCustomers");

            migrationBuilder.DropTable(
                name: "PreSalesProjectManagers");
        }
    }
}
