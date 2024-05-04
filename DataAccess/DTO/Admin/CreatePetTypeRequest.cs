using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Admin
{
    public class CreatePetTypeRequest
    {
        public string ClinicId { get; set; }

        public string PetTypeId { get; set; }
    }
}
