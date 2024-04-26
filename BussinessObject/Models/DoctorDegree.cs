using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class DoctorDegree
    {
        [Key]
        public string DegreeId { get; set; }
        public string DegreeName { get; set; }
    }
}
