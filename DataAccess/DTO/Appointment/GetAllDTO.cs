using DataAccess.DTO.Clinic;
using DataAccess.DTO.DDoctor;
using DataAccess.DTO.DPet;
using DataAccess.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Appointment
{
    public class GetAllDTO
    {
        public string AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }

        public PetDTO Pet { get; set; }

        public DoctorDTO Doctor { get; set; }

        public ClinicDTO Clinic { get; set; }

        public int SlotNumber { get; set; }

        public string Status { get; set; }

        public UserReadDTO User { get; set; }
    }
}
