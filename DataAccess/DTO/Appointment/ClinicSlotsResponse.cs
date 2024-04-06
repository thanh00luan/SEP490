using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Appointment
{
    public class ClinicSlotsResponse
    {
        public string ClinicId { get; set; }
        public String Date { get; set; }
        public List<int> Slots { get; set; }


    }
}
