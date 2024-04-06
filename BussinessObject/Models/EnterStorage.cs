using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class EnterStorage
    {
        [Key]
        public string ImportCode { get; set; }
        public string SuplierCompany { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal TotalAmount { get; set; }
        public float Discount { get; set; }

        public List<StorageStatus> StorageStatuses { get; set; }
    }
}
