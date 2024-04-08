using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Employee
{
    public class StaffDTO
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }

        public bool EmployeeStatus { get; set; }

        public string UserId { get; set; }
    }
}
