using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class MedicineCategory
    {
        [Key]
        public string MedicineCateId { get; set; }
        public string CategoryName { get; set; }
        public List<Pet> Pets { get; set; }
        public List<Medicine> Medicines { get; set; }
    }
}
