using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.DDoctor
{
    public class PrescriptionMedicineInfoDTO
    {
        public string MedicineId { get; set; }
        public int Quantity { get; set; }
        public string MedicineName { get; set; }
        public double PricePerUnit { get; set; } 
        public double TotalPrice { get; set; }
    }
}
