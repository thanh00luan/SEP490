using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Employee
{
    public class AssignDoctorRequest
    {
        public string AppointmentId { get; set; }
        public string DoctorId { get; set; }
    }
}
