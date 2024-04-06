using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Storage
    {
        [Key]
        public string ImportCode { get; set; }
        public string MedicineName { get; set; }
        public string UnitType { get; set; }

        public DateTime Expiry { get; set; }
        public DateTime ImportDate { get; set; }

        public int UnitInStock { get; set; }
        [ForeignKey("StorageStatus")]
        public string LotCode { get; set; }
        public StorageStatus StorageStatus { get; set; }
    }
}
