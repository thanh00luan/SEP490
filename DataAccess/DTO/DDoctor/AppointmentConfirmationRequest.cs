using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.DDoctor
{
    public class AppointmentConfirmationRequest
    {
        public string DoctorId { get; set; }
        public string AppointmentId { get; set; }
    }
}
