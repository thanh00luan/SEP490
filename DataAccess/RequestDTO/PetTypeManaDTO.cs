using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.RequestDTO
{
    public class PetTypeManaDTO
    {
        public string ClinicId { get; set; }
        public string PetTypeId { get; set; }
        public string PetTypeName { get; set; }

        public string ClinicName { get; set; }
    }
}
