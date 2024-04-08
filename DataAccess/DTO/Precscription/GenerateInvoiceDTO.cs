using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Precscription
{
    public class GenerateInvoiceDTO
    {
        public string AppointmentId { get; set; }
        public List<string> MedicineIds { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
        public double Discount { get; set; }
        public string Note { get; set; }
    }
}
