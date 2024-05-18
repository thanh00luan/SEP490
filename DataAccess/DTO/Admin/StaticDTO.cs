using DataAccess.DTO.SuperAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Admin
{
    public class StaticDTO
    {
        public int TotalAppointment { get; set; }

        public List<AppointmentStatisticReponse> AppointmentStatistics { get; set; }
    }
}
