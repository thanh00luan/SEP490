using AutoMapper;
using BussinessObject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class GenerateID
    {
        private readonly ApplicationDbContext _context;
        public GenerateID(ApplicationDbContext context)
        {
            _context = context;
        }
        public string GenerateNewId(string prefix)
        {
            var allCategories = _context.MedicineCategories.ToList();

            var existingIds = new List<string>();

            foreach (var category in allCategories)
            {
                existingIds.Add(category.MedicineCateId);
            }

            if (existingIds.Count == 0)
            {
                return prefix+"01";
            }

            existingIds.Sort();

            var lastId = existingIds.Last();
            var newIdNumber = int.Parse(lastId.Substring(2)) + 1;
            var newId = prefix + newIdNumber.ToString("D2");

            return newId;
        }

        public string GenerateNewDoctorId(string prefix)
        {
            var doctorDegrees = _context.DoctorDegrees.ToList();

            var existingIds = new List<string>();

            foreach (var category in doctorDegrees)
            {
                existingIds.Add(category.DegreeId);
            }

            if (existingIds.Count == 0)
            {
                return prefix + "01";
            }

            existingIds.Sort();

            var lastId = existingIds.Last();
            var newIdNumber = int.Parse(lastId.Substring(2)) + 1;
            var newId = prefix + newIdNumber.ToString("D2");

            return newId;
        }

        public string GenerateNewPetId()
        {
            var allPets = _context.Pets.ToList();

            var existingIds = new List<string>();

            foreach (var pet in allPets)
            {
                existingIds.Add(pet.PetId);
            }

            if (existingIds.Count == 0)
            {
                return "P01";
            }

            existingIds.Sort();

            var lastId = existingIds.Last();
            var newIdNumber = int.Parse(lastId.Substring(2)) + 1;
            var newId = "P" + newIdNumber.ToString("D2");

            return newId;
        }

    }
}
