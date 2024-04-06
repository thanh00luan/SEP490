using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Appointment
{
    public class CreatePetDTO
    {
        public string PetName { get; set; }
        public string PetTypeId { get; set; }
        public string PetSpecies { get; set; }
        public int PetAge { get; set; }
        public bool PetGender { get; set; }
        public string PetColor { get; set; }
        //public PetType PetType { get; set; } 
    }
}
