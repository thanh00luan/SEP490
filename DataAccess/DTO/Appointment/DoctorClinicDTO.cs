using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Appointment
{
    public class DoctorClinicDTO
    {
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string PetId { get; set; }
        public CreatePetDTO PetDTO { get; set; }
        public string ClinicId { get; set; }
        public int Slot { get; set; }


    }
}
