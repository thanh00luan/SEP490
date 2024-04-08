using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.RequestDTO
{
    public class SetDoctorRequest
    {
        public string DoctorId { get; set; }
        public List<int> availabilitySlots { get; set; }

        public DateTime RegisterDate { get; set; }
    }
}
