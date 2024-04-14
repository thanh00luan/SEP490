using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Admin
{
    public class DoctorListDTO
    {
        public int TotalDoctor { get; set; }
        public IEnumerable<DoctorManaDTO> Doctors { get; set; }
    }
}
