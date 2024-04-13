using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Admin
{
    public class DoctorManaDTO
    {
        public string DoctorName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialized { get; set; }
        public bool DoctorStatus { get; set; }
        public string Degree { get; set; }
        public string ClinicName { get; set; }
    }
}
