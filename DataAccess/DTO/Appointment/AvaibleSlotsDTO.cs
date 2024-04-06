using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Appointment
{
    public class AvaibleSlotsDTO
    {
        public string ClinicId { get; set; }
        public string DoctorId { get; set; }
        public DateTime Date { get; set; }
        public List<int> AvailableSlots { get; set; }
    }
}
