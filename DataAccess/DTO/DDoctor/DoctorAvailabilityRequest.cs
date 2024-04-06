using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.DDoctor
{
    public class DoctorAvailabilityRequest
    {
        public string DoctorId { get; set; }
        public List<int> AvailabilitySlots { get; set; }
    }
}
