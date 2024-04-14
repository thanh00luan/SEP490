using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Admin
{
    public class MedicineListDTO
    {
        public int TotalMedicine { get; set; }
        public IEnumerable<MedicineManaDTO> Medicines { get; set; }
    }
}
