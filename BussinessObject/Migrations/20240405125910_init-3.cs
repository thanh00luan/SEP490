using Microsoft.EntityFrameworkCore.Migrations;

namespace BussinessObject.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_UserID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Users_UserID",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Users_UserID",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Users_UserID",
                table: "Prescriptions");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Prescriptions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Prescriptions_UserID",
                table: "Prescriptions",
                newName: "IX_Prescriptions_UserId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Pets",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Pets_UserID",
                table: "Pets",
                newName: "IX_Pets_UserId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Bills",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_UserID",
                table: "Bills",
                newName: "IX_Bills_UserId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Appointments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_UserID",
                table: "Appointments",
                newName: "IX_Appointments_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_UserId",
                table: "Appointments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Users_UserId",
                table: "Bills",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Users_UserId",
                table: "Pets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Users_UserId",
                table: "Prescriptions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_UserId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Users_UserId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Users_UserId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Users_UserId",
                table: "Prescriptions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Prescriptions",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Prescriptions_UserId",
                table: "Prescriptions",
                newName: "IX_Prescriptions_UserID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Pets",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Pets_UserId",
                table: "Pets",
                newName: "IX_Pets_UserID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Bills",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_UserId",
                table: "Bills",
                newName: "IX_Bills_UserID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Appointments",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_UserId",
                table: "Appointments",
                newName: "IX_Appointments_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_UserID",
                table: "Appointments",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Users_UserID",
                table: "Bills",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Users_UserID",
                table: "Pets",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Users_UserID",
                table: "Prescriptions",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
