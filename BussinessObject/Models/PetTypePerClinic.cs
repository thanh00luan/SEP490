using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class PetTypePerClinic
    {
        [Key]
        public string ClinicPetTypeId { get; set; }
        [ForeignKey("Clinic")]
        public string ClinicId { get; set; }
        public Clinic Clinic { get; set; }

        [ForeignKey("PetType")]
        public string PetTypeId { get; set; }
        public PetType PetType { get; set; }
    }
}
