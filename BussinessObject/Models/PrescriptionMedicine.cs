using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class PrescriptionMedicine
    {
        [Key]
        public string PrescriptionMedicineId { get; set; }

        [ForeignKey("Prescription")]
        public string PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }

        [ForeignKey("Medicine")]
        public string MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public int Quantity { get; set; }
    }
}
