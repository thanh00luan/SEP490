using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Admin
{
    public class AdminDTO
    {
        public string ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string Address { get; set; }
        public string ClinicPhoneNumber { get; set; }       
        public string Email { get; set; }
    }
}
