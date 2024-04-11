using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BussinessObject.Data; // Assuming this namespace contains your domain models
using BussinessObject.Models; // Assuming this namespace contains your domain models
using DataAccess.DTO.Admin;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class AdminDAO
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AdminDAO(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AdminDTO> AddAdminAsync(AdminDTO newAdmin)
        {
            var existingAdmin = await _context.Admins.FirstOrDefaultAsync(a => a.AdminId == newAdmin.ClinicId);
            if (existingAdmin != null)
            {
                throw new Exception("Admin already exists");
            }

            var adminEntity = _mapper.Map<Admin>(newAdmin);
            _context.Admins.Add(adminEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<AdminDTO>(adminEntity);
        }

        public async Task<AdminDTO> GetAdminByIdAsync(string clinicId)
        {
            var admin = await _context.Admins.FindAsync(clinicId);
            if (admin == null)
            {
                throw new Exception("Admin not found");
            }

            return _mapper.Map<AdminDTO>(admin);
        }

        public async Task<IEnumerable<AdminDTO>> GetAllAdminsAsync()
        {
            var admins = await _context.Admins.ToListAsync();
            return _mapper.Map<IEnumerable<AdminDTO>>(admins);
        }

        public async Task UpdateAdminAsync(string clinicId, AdminDTO updatedAdmin)
        {
            var existingAdmin = await _context.Admins.FindAsync(clinicId);
            if (existingAdmin == null)
            {
                throw new Exception("Admin not found");
            }

            _mapper.Map(updatedAdmin, existingAdmin);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAdminAsync(string clinicId)
        {
            var admin = await _context.Admins.FindAsync(clinicId);
            if (admin == null)
            {
                throw new Exception("Admin not found");
            }

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
        }
    }
}
