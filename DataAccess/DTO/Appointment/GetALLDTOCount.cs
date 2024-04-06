using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Appointment
{
    public class GetALLDTOCount
    {
        public int Total { get; set; }
        public List<GetAllDTO> AppointmentDTOs { get; set; }
    }
}
