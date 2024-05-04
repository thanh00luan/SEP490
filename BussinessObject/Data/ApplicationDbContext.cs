using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-RP8V9AM\\SQLEXPRESS;Initial Catalog=SEP490_DoctorPet;User Id = sa; pwd = 123456;Trust Server certificate = true;");
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillMedicine> BillMedicines { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<EnterStorage> EnterStorages { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineCategory> MedicineCategories { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<StorageStatus> StorageStatus { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<DoctorSlot> DoctorSlots { get; set; }

        public DbSet<PrescriptionMedicine> prescriptionMedicines { get; set; }

        public DbSet<DoctorDegree> DoctorDegrees { get; set;}

        public DbSet<PetTypePerClinic> PetTypePerClinics { get; set; }
    }
}
