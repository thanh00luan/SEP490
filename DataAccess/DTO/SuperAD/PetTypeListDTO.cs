using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.SuperAD
{
    public class PetTypeListDTO
    {
        public int Total { get; set; }
        public IEnumerable<PetCateManaDTO> PetTypes { get; set; }
    }
}
