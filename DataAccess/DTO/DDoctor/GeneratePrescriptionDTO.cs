using DataAccess.DTO.Precscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.DDoctor
{
    public class GeneratePrescriptionDTO
    {
        public CreateDTO CreateDTO { get; set; }
        public List<PrescriptionMedicineInfoDTO> PrescriptionMedicines { get; set; }
    }
}
