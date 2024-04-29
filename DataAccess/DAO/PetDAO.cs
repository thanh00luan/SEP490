using AutoMapper;
using BussinessObject.Data;
using BussinessObject.Models;
using DataAccess.DTO.Admin;
using DataAccess.DTO.DPet;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class PetDAO
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public PetDAO(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreatePetAsync(PetManaDTO PetDTO)
        {
            var Pet = new Pet
            {
                PetId = Guid.NewGuid().ToString(),
                PetName = PetDTO.PetName,
                PetAge = PetDTO.PetAge,
                PetColor = PetDTO.PetColor, 
                PetGender = PetDTO.PetGender,
                PetSpecies  = PetDTO.PetSpecies,
                PetTypeId = PetDTO.PetTypeId,
                UserId = PetDTO.UserId,
            };

            _context.Pets.Add(Pet);
            await _context.SaveChangesAsync();
        }

        public async Task<PetListDTO> GetAllPetAsync(string userId)
        {
            try
            {
                var clinicPetsQuery = _context.Pets
                    .Where(m => m.UserId == userId)
                    .Include(m => m.PetType)
                    .OrderBy(m => m.PetId);

                var totalPets = await clinicPetsQuery.CountAsync();

                var clinicPets = await clinicPetsQuery.ToListAsync();

                var PetDTOs = clinicPets.Select(Pet => new PetManaDTO
                {
                    PetId = Pet.PetId,
                    PetName = Pet.PetName,
                    PetAge = Pet.PetAge,
                    PetColor = Pet.PetColor,
                    PetGender = Pet.PetGender,  
                    PetSpecies = Pet.PetSpecies,
                    PetTypeId = Pet.PetTypeId,
                    UserId = Pet.UserId,
                });

                return new PetListDTO
                {
                    PetTotal = totalPets,
                    PetLists = PetDTOs
                };
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in GetAllPetAsync: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllPetAsync: {ex.Message}");
                throw;
            }
        }
        public async Task<PetManaDTO> GetPetByIdAsync( string PetId)
        {
            var Pet = await _context.Pets
                .FindAsync(PetId);

            if (Pet == null )
            {
                return null;
            }

            return new PetManaDTO
            {
                PetId = Pet.PetId,
                PetName = Pet.PetName,
                PetAge = Pet.PetAge,
                PetColor= Pet.PetColor,
                PetGender = Pet.PetGender,
                PetSpecies = Pet.PetSpecies,
                PetTypeId = Pet.PetTypeId,
                UserId = Pet.UserId,
            };
        }

        public async Task UpdatePetAsync(PetManaDTO PetDTO)
        {
            var Pet = await _context.Pets.FindAsync(PetDTO.PetId);

            if (Pet == null)
            {
                return;
            }
            Pet.PetId = PetDTO.PetId;   
            Pet.PetName = PetDTO.PetName;
            Pet.PetGender = PetDTO.PetGender;
            Pet.PetColor = PetDTO.PetColor;
            Pet.PetAge = PetDTO.PetAge;
            Pet.PetSpecies = PetDTO.PetSpecies;
            Pet.PetTypeId = PetDTO.PetTypeId;
            if(PetDTO.UserId == "")
            {
                PetDTO.UserId = Pet.UserId;
            }

            _context.Pets.Update(Pet);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePetAsync( string PetId)
        {
            var Pet = await _context.Pets.FindAsync(PetId);

            if (Pet == null)
            {
                return;
            }

            _context.Pets.Remove(Pet);
            await _context.SaveChangesAsync();
        }
    }
}
