using Microsoft.EntityFrameworkCore.Migrations;

namespace BussinessObject.Migrations
{
    public partial class init12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Users_UserId",
                table: "Prescriptions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Prescriptions",
                newName: "AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Prescriptions_UserId",
                table: "Prescriptions",
                newName: "IX_Prescriptions_AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Appointments_AppointmentId",
                table: "Prescriptions",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Appointments_AppointmentId",
                table: "Prescriptions");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "Prescriptions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Prescriptions_AppointmentId",
                table: "Prescriptions",
                newName: "IX_Prescriptions_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Users_UserId",
                table: "Prescriptions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
