using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Appointment
    {
        [Key]
        public string AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }
        [ForeignKey("Pet")]

        public string PetId { get; set; }
        public Pet Pet { get; set; }
        [ForeignKey("Doctor")]


        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [ForeignKey("Clinic")]

        public string ClinicId { get; set; }
        public Clinic Clinic { get; set; }

        public string Status { get; set; }
        [ForeignKey("Customer")]
        public int SlotNumber { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }

        public List<Prescription> Prescriptions { get; set; }



    }
}
