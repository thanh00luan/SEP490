using DataAccess.DAO;
using DataAccess.DTO.DPet;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class PetRepository : IPetRepository
    {
        private readonly PetDAO _petDAO;
        public PetRepository(PetDAO petDAO)
        {
            _petDAO = petDAO;
        }
        public Task AddPet(PetManaDTO dto)
            =>_petDAO.CreatePetAsync(dto);

        public Task DeletePet(string id)
            =>_petDAO.DeletePetAsync(id);

        public Task<PetListDTO> GetAllPet(string userId)
            =>_petDAO.GetAllPetAsync(userId);

        public Task<PetManaDTO> GetPetById(string id)
            =>_petDAO.GetPetByIdAsync(id);

        public Task UpdatePet(PetManaDTO updateDTO)
            =>_petDAO.UpdatePetAsync(updateDTO);
    }
}
