using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Medicine
{
    public class MedicineDTO
    {
        public string MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string MedicineUnit { get; set; }
        public double Prices { get; set; }
        public int Inventory { get; set; }
        public string Specifications { get; set; }
        public string MedicineCateId { get; set; }
        public string MedicineCateName { get; set; }

    }
}
