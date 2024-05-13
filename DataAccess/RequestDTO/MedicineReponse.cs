using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.RequestDTO
{
    public class MedicineReponse
    {
        public double DuePrice { get; set; }
        public List<MedicineReport> MedicineReport { get; set; }
    }
}
