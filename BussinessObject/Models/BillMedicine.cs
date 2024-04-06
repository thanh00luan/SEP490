using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class BillMedicine
    {
        [Key]
        public string BillMedicineId { get; set; }

        [ForeignKey("Bill")]
        public string BillId { get; set; }
        public Bill Bill { get; set; }

        [ForeignKey("Medicine")]
        public string MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public int Quantity { get; set; }
    }
}
