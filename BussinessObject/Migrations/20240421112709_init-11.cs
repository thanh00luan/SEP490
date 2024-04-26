using Microsoft.EntityFrameworkCore.Migrations;

namespace BussinessObject.Migrations
{
    public partial class init11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "DegreeId",
                table: "Doctors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DoctorDegrees",
                columns: table => new
                {
                    DegreeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DegreeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorDegrees", x => x.DegreeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DegreeId",
                table: "Doctors",
                column: "DegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorDegrees_DegreeId",
                table: "Doctors",
                column: "DegreeId",
                principalTable: "DoctorDegrees",
                principalColumn: "DegreeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorDegrees_DegreeId",
                table: "Doctors");

            migrationBuilder.DropTable(
                name: "DoctorDegrees");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_DegreeId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DegreeId",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
