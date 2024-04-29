using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.DPet
{
    public class PetListDTO
    {
        public int PetTotal { get; set; }
        public IEnumerable<PetManaDTO> PetLists { get; set; }
    }
}
