using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Admin
{
    public class StaffListDTO
    {
        public int TotalStaff { get; set; }
        public IEnumerable<StaffManaDTO> Staffs { get; set; }
    }
}
