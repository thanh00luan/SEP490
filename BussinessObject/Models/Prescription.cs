using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Prescription
    {
        [Key]
        public string PrescriptionId { get; set; }
        public string Diagnose { get; set; }
        public DateTime ExaminationDay { get; set; }
        public DateTime CreateDay { get; set; }
        public string Reason { get; set; }
        [ForeignKey("Appointment")]
        public string AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public ICollection<PrescriptionMedicine> PrescriptionMedicines { get; set; }
    }
}
