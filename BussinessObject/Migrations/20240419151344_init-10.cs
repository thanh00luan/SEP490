using Microsoft.EntityFrameworkCore.Migrations;

namespace BussinessObject.Migrations
{
    public partial class init10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClinicId",
                table: "Medicines",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_ClinicId",
                table: "Medicines",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Clinics_ClinicId",
                table: "Medicines",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "ClinicId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_Clinics_ClinicId",
                table: "Medicines");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_ClinicId",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "Medicines");
        }
    }
}
