using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingOfApplicants.Migrations
{
    public partial class ExtentionUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestHeaders_subjects_Subjectid",
                table: "TestHeaders");

            migrationBuilder.DropIndex(
                name: "IX_TestHeaders_Subjectid",
                table: "TestHeaders");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Subjectid",
                table: "TestHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "Subjectid",
                table: "TestHeaders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TestHeaders_Subjectid",
                table: "TestHeaders",
                column: "Subjectid");

            migrationBuilder.AddForeignKey(
                name: "FK_TestHeaders_subjects_Subjectid",
                table: "TestHeaders",
                column: "Subjectid",
                principalTable: "subjects",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
