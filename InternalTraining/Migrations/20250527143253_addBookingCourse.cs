using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalTraining.Migrations
{
    /// <inheritdoc />
    public partial class addBookingCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstPaymentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingCourses_firstPayments_FirstPaymentId",
                        column: x => x.FirstPaymentId,
                        principalTable: "firstPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingCourses_CourseId",
                table: "BookingCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingCourses_FirstPaymentId",
                table: "BookingCourses",
                column: "FirstPaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingCourses");
        }
    }
}
