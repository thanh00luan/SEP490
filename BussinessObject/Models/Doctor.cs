using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Doctor
    {
        [Key]
        public string DoctorId { get; set; }
        public string Specialized { get; set; }

        public string Slots { get; set; }

        public bool Status { get; set; }
        [ForeignKey("Clinic")]
        public string ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        [ForeignKey("User")]

        public string UserId { get; set; }
        public User User { get; set; }
        public string Degree { get; set; }

        public List<DoctorSlot> DoctorSlots { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
