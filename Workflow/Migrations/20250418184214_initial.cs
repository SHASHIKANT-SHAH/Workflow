using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Workflow.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowInstanceInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowInstanceInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_ApplicationStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ApplicationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerDecisionId = table.Column<int>(type: "int", nullable: true),
                    HrDecisionId = table.Column<int>(type: "int", nullable: true),
                    WorkflowInstanceInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Statuses_HrDecisionId",
                        column: x => x.HrDecisionId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Statuses_ManagerDecisionId",
                        column: x => x.ManagerDecisionId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeaveRequests_WorkflowInstanceInfos_WorkflowInstanceInfoId",
                        column: x => x.WorkflowInstanceInfoId,
                        principalTable: "WorkflowInstanceInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    PreviousStatusId = table.Column<int>(type: "int", nullable: false),
                    NewStatusId = table.Column<int>(type: "int", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationHistories_ApplicationStatuses_NewStatusId",
                        column: x => x.NewStatusId,
                        principalTable: "ApplicationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationHistories_ApplicationStatuses_PreviousStatusId",
                        column: x => x.PreviousStatusId,
                        principalTable: "ApplicationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationHistories_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApplicationStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Waiting for review", "Pending" },
                    { 2, "Currently being reviewed", "In Process" },
                    { 3, "Sent for further processing", "Forwarded" },
                    { 4, "Application approved", "Approved" },
                    { 5, "Application rejected", "Rejected" },
                    { 6, "Paused for review", "On Hold" },
                    { 7, "Process completed successfully", "Completed" },
                    { 8, "Application canceled", "Canceled" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Approved" },
                    { 3, "Rejected" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationHistories_ApplicationId",
                table: "ApplicationHistories",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationHistories_NewStatusId",
                table: "ApplicationHistories",
                column: "NewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationHistories_PreviousStatusId",
                table: "ApplicationHistories",
                column: "PreviousStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_StatusId",
                table: "Applications",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_HrDecisionId",
                table: "LeaveRequests",
                column: "HrDecisionId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_ManagerDecisionId",
                table: "LeaveRequests",
                column: "ManagerDecisionId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_WorkflowInstanceInfoId",
                table: "LeaveRequests",
                column: "WorkflowInstanceInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationHistories");

            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "WorkflowInstanceInfos");

            migrationBuilder.DropTable(
                name: "ApplicationStatuses");
        }
    }
}
