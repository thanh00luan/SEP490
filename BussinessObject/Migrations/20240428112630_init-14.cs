using Microsoft.EntityFrameworkCore.Migrations;

namespace BussinessObject.Migrations
{
    public partial class init14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_Users_UserId",
                table: "Clinics");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Clinics",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Clinics_UserId",
                table: "Clinics",
                newName: "IX_Clinics_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_Employees_EmployeeId",
                table: "Clinics",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_Employees_EmployeeId",
                table: "Clinics");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Clinics",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Clinics_EmployeeId",
                table: "Clinics",
                newName: "IX_Clinics_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_Users_UserId",
                table: "Clinics",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
