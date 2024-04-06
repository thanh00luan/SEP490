using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Pet
    {
        [Key]
        public string PetId { get; set; }
        public string PetName { get; set; }
        [ForeignKey("PetType")]

        public string PetTypeId { get; set; }
        public PetType PetType { get; set; }

        public string PetSpecies { get; set; }
        public int PetAge { get; set; }
        public bool PetGender { get; set; }
        public string PetColor { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        public List<Prescription> Prescriptions { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
