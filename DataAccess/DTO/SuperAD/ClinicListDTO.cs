using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.SuperAD
{
    public class ClinicListDTO
    {
        public int TotalClinic { get; set; }

        public IEnumerable<ClinicManaDTO>  Clinics { get; set; }
    }
}
