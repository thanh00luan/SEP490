using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class PetType
    {
        [Key]
        public string PetTypeId { get; set; }
        public string PetTypeName { get; set; }

        public List<Pet> Pets { get; set; }
        public List<PetTypePerClinic> PetTypeClinics { get; set; }
    }
}
