using DataAccess.DTO.DDoctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Precscription
{
    public class PresDTO
    {
        public string PrescriptionId { get; set; }
        public string Diagnose { get; set; }
        public DateTime ExaminationDay { get; set; }
        public DateTime CreateDay { get; set; }
        public string Note { get; set; }
        public string PetId { get; set; }
        public string PetName { get; set; } 
        public string UserId { get; set; }
        public string FullName { get; set; }
        public List<PrescriptionMedicineInfoDTO> PrescriptionMedicines { get; set; }

        public double TotalPrices { get; set; }
    }
}
