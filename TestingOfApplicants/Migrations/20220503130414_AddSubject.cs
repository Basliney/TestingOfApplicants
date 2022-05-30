using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingOfApplicants.Migrations
{
    public partial class AddSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Subjectid",
                table: "TestHeaders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestHeaders_subjects_Subjectid",
                table: "TestHeaders");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropIndex(
                name: "IX_TestHeaders_Subjectid",
                table: "TestHeaders");

            migrationBuilder.DropColumn(
                name: "Subjectid",
                table: "TestHeaders");
        }
    }
}
