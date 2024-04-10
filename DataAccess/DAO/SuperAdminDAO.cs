using AutoMapper;
using BussinessObject.Data;
using BussinessObject.Models;
using DataAccess.DTO.Admin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SuperAdminDAO
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SuperAdminDAO(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // UserManagement

        public async Task<IEnumerable<UserManaDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            var usersDTO = _mapper.Map<IEnumerable<UserManaDTO>>(users);
            return usersDTO;
        }

        public async Task<UserManaDTO> GetUserByIdAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return _mapper.Map<UserManaDTO>(user);
        }

        public async Task UpdateUserAsync(UserManaDTO updateUser)
        {
            try
            {
                var exUser = await _context.Users.FindAsync(updateUser.UserId);
                if (exUser == null)
                {
                    throw new Exception("User not found for updating");
                }

                exUser.FullName = updateUser.FullName;
                exUser.Address = updateUser.Address;
                exUser.PhoneNumber = updateUser.PhoneNumber;
                exUser.Email = updateUser.Email;
                exUser.Birthday = updateUser.Birthday;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database error in UpdateUser: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUser: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }



    }
}
