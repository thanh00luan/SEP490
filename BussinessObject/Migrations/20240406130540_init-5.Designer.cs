﻿// <auto-generated />
using System;
using BussinessObject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BussinessObject.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240406130540_init-5")]
    partial class init5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BussinessObject.Models.Appointment", b =>
                {
                    b.Property<string>("AppointmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ClinicId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DoctorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SlotNumber")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AppointmentId");

                    b.HasIndex("ClinicId");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PetId");

                    b.HasIndex("UserId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("BussinessObject.Models.Bill", b =>
                {
                    b.Property<string>("BillId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Discount")
                        .HasColumnType("float");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<double>("TotalPrices")
                        .HasColumnType("float");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BillId");

                    b.HasIndex("UserId");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("BussinessObject.Models.BillMedicine", b =>
                {
                    b.Property<string>("BillMedicineId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BillId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MedicineId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("BillMedicineId");

                    b.HasIndex("BillId");

                    b.HasIndex("MedicineId");

                    b.ToTable("BillMedicines");
                });

            modelBuilder.Entity("BussinessObject.Models.Clinic", b =>
                {
                    b.Property<string>("ClinicId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClinicName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClinicPhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.HasKey("ClinicId");

                    b.ToTable("Clinics");
                });

            modelBuilder.Entity("BussinessObject.Models.Doctor", b =>
                {
                    b.Property<string>("DoctorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClinicId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Slots")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Specialized")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DoctorId");

                    b.HasIndex("ClinicId");

                    b.HasIndex("UserId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("BussinessObject.Models.Employee", b =>
                {
                    b.Property<string>("EmployeeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("EmployeeStatus")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("EmployeeId");

                    b.HasIndex("UserId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("BussinessObject.Models.EnterStorage", b =>
                {
                    b.Property<string>("ImportCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("Discount")
                        .HasColumnType("real");

                    b.Property<string>("SuplierCompany")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ImportCode");

                    b.ToTable("EnterStorages");
                });

            modelBuilder.Entity("BussinessObject.Models.Medicine", b =>
                {
                    b.Property<string>("MedicineId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Inventory")
                        .HasColumnType("int");

                    b.Property<string>("MedicineCateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MedicineName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MedicineUnit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Prices")
                        .HasColumnType("float");

                    b.Property<string>("Specifications")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MedicineId");

                    b.HasIndex("MedicineCateId");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("BussinessObject.Models.MedicineCategory", b =>
                {
                    b.Property<string>("MedicineCateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MedicineCateId");

                    b.ToTable("MedicineCategories");
                });

            modelBuilder.Entity("BussinessObject.Models.Pet", b =>
                {
                    b.Property<string>("PetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MedicineCategoryMedicineCateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PetAge")
                        .HasColumnType("int");

                    b.Property<string>("PetColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PetGender")
                        .HasColumnType("bit");

                    b.Property<string>("PetName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PetSpecies")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PetTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PetId");

                    b.HasIndex("MedicineCategoryMedicineCateId");

                    b.HasIndex("PetTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Pets");
                });

            modelBuilder.Entity("BussinessObject.Models.PetType", b =>
                {
                    b.Property<string>("PetTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PetTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PetTypeId");

                    b.ToTable("PetTypes");
                });

            modelBuilder.Entity("BussinessObject.Models.Prescription", b =>
                {
                    b.Property<string>("PrescriptionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("Diagnose")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExaminationDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("MedicineId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PrescriptionId");

                    b.HasIndex("MedicineId");

                    b.HasIndex("PetId");

                    b.HasIndex("UserId");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("BussinessObject.Models.Storage", b =>
                {
                    b.Property<string>("ImportCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Expiry")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ImportDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LotCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MedicineName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnitInStock")
                        .HasColumnType("int");

                    b.Property<string>("UnitType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ImportCode");

                    b.HasIndex("LotCode");

                    b.ToTable("Storages");
                });

            modelBuilder.Entity("BussinessObject.Models.StorageStatus", b =>
                {
                    b.Property<string>("LotCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Expiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImportCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ImportQuantity")
                        .HasColumnType("int");

                    b.Property<int>("InventoryQuantity")
                        .HasColumnType("int");

                    b.HasKey("LotCode");

                    b.HasIndex("ImportCode");

                    b.ToTable("StorageStatus");
                });

            modelBuilder.Entity("BussinessObject.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BussinessObject.Models.Appointment", b =>
                {
                    b.HasOne("BussinessObject.Models.Clinic", "Clinic")
                        .WithMany("Appointments")
                        .HasForeignKey("ClinicId");

                    b.HasOne("BussinessObject.Models.Doctor", "Doctor")
                        .WithMany("Appointments")
                        .HasForeignKey("DoctorId");

                    b.HasOne("BussinessObject.Models.Pet", "Pet")
                        .WithMany("Appointments")
                        .HasForeignKey("PetId");

                    b.HasOne("BussinessObject.Models.User", "User")
                        .WithMany("Appointments")
                        .HasForeignKey("UserId");

                    b.Navigation("Clinic");

                    b.Navigation("Doctor");

                    b.Navigation("Pet");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObject.Models.Bill", b =>
                {
                    b.HasOne("BussinessObject.Models.User", "User")
                        .WithMany("Bills")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObject.Models.BillMedicine", b =>
                {
                    b.HasOne("BussinessObject.Models.Bill", "Bill")
                        .WithMany("BillMedicines")
                        .HasForeignKey("BillId");

                    b.HasOne("BussinessObject.Models.Medicine", "Medicine")
                        .WithMany("BillMedicines")
                        .HasForeignKey("MedicineId");

                    b.Navigation("Bill");

                    b.Navigation("Medicine");
                });

            modelBuilder.Entity("BussinessObject.Models.Doctor", b =>
                {
                    b.HasOne("BussinessObject.Models.Clinic", "Clinic")
                        .WithMany("Doctors")
                        .HasForeignKey("ClinicId");

                    b.HasOne("BussinessObject.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Clinic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObject.Models.Employee", b =>
                {
                    b.HasOne("BussinessObject.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObject.Models.Medicine", b =>
                {
                    b.HasOne("BussinessObject.Models.MedicineCategory", "MedicineCategory")
                        .WithMany("Medicines")
                        .HasForeignKey("MedicineCateId");

                    b.Navigation("MedicineCategory");
                });

            modelBuilder.Entity("BussinessObject.Models.Pet", b =>
                {
                    b.HasOne("BussinessObject.Models.MedicineCategory", null)
                        .WithMany("Pets")
                        .HasForeignKey("MedicineCategoryMedicineCateId");

                    b.HasOne("BussinessObject.Models.PetType", "PetType")
                        .WithMany("Pets")
                        .HasForeignKey("PetTypeId");

                    b.HasOne("BussinessObject.Models.User", "User")
                        .WithMany("Pets")
                        .HasForeignKey("UserId");

                    b.Navigation("PetType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObject.Models.Prescription", b =>
                {
                    b.HasOne("BussinessObject.Models.Medicine", "Medicine")
                        .WithMany("Prescriptions")
                        .HasForeignKey("MedicineId");

                    b.HasOne("BussinessObject.Models.Pet", "Pet")
                        .WithMany("Prescriptions")
                        .HasForeignKey("PetId");

                    b.HasOne("BussinessObject.Models.User", "User")
                        .WithMany("Prescriptions")
                        .HasForeignKey("UserId");

                    b.Navigation("Medicine");

                    b.Navigation("Pet");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BussinessObject.Models.Storage", b =>
                {
                    b.HasOne("BussinessObject.Models.StorageStatus", "StorageStatus")
                        .WithMany("Storages")
                        .HasForeignKey("LotCode");

                    b.Navigation("StorageStatus");
                });

            modelBuilder.Entity("BussinessObject.Models.StorageStatus", b =>
                {
                    b.HasOne("BussinessObject.Models.EnterStorage", "EnterStorage")
                        .WithMany("StorageStatuses")
                        .HasForeignKey("ImportCode");

                    b.Navigation("EnterStorage");
                });

            modelBuilder.Entity("BussinessObject.Models.Bill", b =>
                {
                    b.Navigation("BillMedicines");
                });

            modelBuilder.Entity("BussinessObject.Models.Clinic", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Doctors");
                });

            modelBuilder.Entity("BussinessObject.Models.Doctor", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("BussinessObject.Models.EnterStorage", b =>
                {
                    b.Navigation("StorageStatuses");
                });

            modelBuilder.Entity("BussinessObject.Models.Medicine", b =>
                {
                    b.Navigation("BillMedicines");

                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("BussinessObject.Models.MedicineCategory", b =>
                {
                    b.Navigation("Medicines");

                    b.Navigation("Pets");
                });

            modelBuilder.Entity("BussinessObject.Models.Pet", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("BussinessObject.Models.PetType", b =>
                {
                    b.Navigation("Pets");
                });

            modelBuilder.Entity("BussinessObject.Models.StorageStatus", b =>
                {
                    b.Navigation("Storages");
                });

            modelBuilder.Entity("BussinessObject.Models.User", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Bills");

                    b.Navigation("Pets");

                    b.Navigation("Prescriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
