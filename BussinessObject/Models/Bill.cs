using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Bill
    {
        [Key]
        public string BillId { get; set; }
        public DateTime CreateDate { get; set; }
        public double TotalPrices { get; set; }
        public int Quantity { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
        public double Discount { get; set; }
        public string Note { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }

        public List<BillMedicine> BillMedicines { get; set; }
    }
}
