using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalTraining.Migrations
{
    /// <inheritdoc />
    public partial class addEmpProg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeChaptersExam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChapterId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    TakenOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeChaptersExam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeChaptersExam_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeChaptersExam_EmployeeUsers_EmployeeUserId",
                        column: x => x.EmployeeUserId,
                        principalTable: "EmployeeUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLessonsProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLessonsProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeLessonsProgress_EmployeeUsers_EmployeeUserId",
                        column: x => x.EmployeeUserId,
                        principalTable: "EmployeeUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeLessonsProgress_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeChaptersExam_ChapterId",
                table: "EmployeeChaptersExam",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeChaptersExam_EmployeeUserId",
                table: "EmployeeChaptersExam",
                column: "EmployeeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLessonsProgress_EmployeeUserId",
                table: "EmployeeLessonsProgress",
                column: "EmployeeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLessonsProgress_LessonId",
                table: "EmployeeLessonsProgress",
                column: "LessonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeChaptersExam");

            migrationBuilder.DropTable(
                name: "EmployeeLessonsProgress");
        }
    }
}
