using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Medicine
    {
        [Key]
        public string MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string MedicineUnit { get; set; }
        public double Prices { get; set; }
        public int Inventory { get; set; }
        public string Specifications { get; set; }
        [ForeignKey("MedicineCategory")]
        public string MedicineCateId { get; set; }
        public MedicineCategory MedicineCategory { get; set; }
        public List<Prescription> Prescriptions { get; set; }
        public List<BillMedicine> BillMedicines { get; set; }
        [ForeignKey("ClinicId")]
        public string ClinicId { get; set; }
        public Clinic Clinic { get; set; }
    }
}
