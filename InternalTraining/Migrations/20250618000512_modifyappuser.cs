using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalTraining.Migrations
{
    /// <inheritdoc />
    public partial class modifyappuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyContactsUs_AspNetUsers_ApplicationUserId",
                table: "CompanyContactsUs");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "CompanyContactsUs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyContactsUs_AspNetUsers_ApplicationUserId",
                table: "CompanyContactsUs",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyContactsUs_AspNetUsers_ApplicationUserId",
                table: "CompanyContactsUs");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "CompanyContactsUs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyContactsUs_AspNetUsers_ApplicationUserId",
                table: "CompanyContactsUs",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
