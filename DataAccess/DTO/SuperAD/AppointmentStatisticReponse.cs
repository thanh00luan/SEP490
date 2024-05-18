using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.SuperAD
{
    public class AppointmentStatisticReponse
    {
        public DateTime Date { get; set; }
        public int total { get; set; }
        public int doneQuantity { get; set; }
        public int inProgressQuantity { get; set; }
        public int waitingQuantity { get; set; }
        public int pendingQuantity { get; set; }
    }
}
