using Microsoft.EntityFrameworkCore.Migrations;

namespace BussinessObject.Migrations
{
    public partial class init9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedicine_Medicines_MedicineId",
                table: "PrescriptionMedicine");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedicine_Prescriptions_PrescriptionId",
                table: "PrescriptionMedicine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrescriptionMedicine",
                table: "PrescriptionMedicine");

            migrationBuilder.RenameTable(
                name: "PrescriptionMedicine",
                newName: "prescriptionMedicines");

            migrationBuilder.RenameIndex(
                name: "IX_PrescriptionMedicine_PrescriptionId",
                table: "prescriptionMedicines",
                newName: "IX_prescriptionMedicines_PrescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_PrescriptionMedicine_MedicineId",
                table: "prescriptionMedicines",
                newName: "IX_prescriptionMedicines_MedicineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_prescriptionMedicines",
                table: "prescriptionMedicines",
                column: "PrescriptionMedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptionMedicines_Medicines_MedicineId",
                table: "prescriptionMedicines",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "MedicineId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_prescriptionMedicines_Prescriptions_PrescriptionId",
                table: "prescriptionMedicines",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_prescriptionMedicines_Medicines_MedicineId",
                table: "prescriptionMedicines");

            migrationBuilder.DropForeignKey(
                name: "FK_prescriptionMedicines_Prescriptions_PrescriptionId",
                table: "prescriptionMedicines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_prescriptionMedicines",
                table: "prescriptionMedicines");

            migrationBuilder.RenameTable(
                name: "prescriptionMedicines",
                newName: "PrescriptionMedicine");

            migrationBuilder.RenameIndex(
                name: "IX_prescriptionMedicines_PrescriptionId",
                table: "PrescriptionMedicine",
                newName: "IX_PrescriptionMedicine_PrescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_prescriptionMedicines_MedicineId",
                table: "PrescriptionMedicine",
                newName: "IX_PrescriptionMedicine_MedicineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrescriptionMedicine",
                table: "PrescriptionMedicine",
                column: "PrescriptionMedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedicine_Medicines_MedicineId",
                table: "PrescriptionMedicine",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "MedicineId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedicine_Prescriptions_PrescriptionId",
                table: "PrescriptionMedicine",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
