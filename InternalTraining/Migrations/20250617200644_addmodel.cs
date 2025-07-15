using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalTraining.Migrations
{
    /// <inheritdoc />
    public partial class addmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyContactsUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsAnswered = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyContactsUs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyContactsUs_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyContactsUs_ApplicationUserId",
                table: "CompanyContactsUs",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyContactsUs");
        }
    }
}
