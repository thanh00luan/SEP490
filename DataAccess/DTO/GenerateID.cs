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
        public string GenerateNewCategoryId()
        {
            var allCategories = _context.MedicineCategories.ToList();

            var existingIds = new List<string>();

            foreach (var category in allCategories)
            {
                existingIds.Add(category.MedicineCateId);
            }

            if (existingIds.Count == 0)
            {
                return "MC01";
            }

            existingIds.Sort();

            var lastId = existingIds.Last();
            var newIdNumber = int.Parse(lastId.Substring(2)) + 1;
            var newId = "MC" + newIdNumber.ToString("D2");

            return newId;
        }

    }
}
