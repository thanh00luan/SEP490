using DataAccess.DTO.Admin;
using DataAccess.DTO.DPet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IPetRepository
    {
        Task AddPet(PetManaDTO dto);
        Task DeletePet(string id);
        Task<PetListDTO> GetAllPet(string userId);
        Task<PetManaDTO> GetPetById(string id);
        Task UpdatePet(PetManaDTO updateDTO);
    }
}
