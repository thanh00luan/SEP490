using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class StorageStatus
    {
        [Key]
        public string LotCode { get; set; }
        public DateTime Expiry { get; set; }
        public int ImportQuantity { get; set; }
        public int InventoryQuantity { get; set; }

        [ForeignKey("EnterStorage")]

        public string ImportCode { get; set; }
        public EnterStorage EnterStorage { get; set; }

        public List<Storage> Storages { get; set; }
    }
}
