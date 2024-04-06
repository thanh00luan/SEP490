using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BussinessObject.Data;
using BussinessObject.Models;
using DataAccess.DTO.User;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class UserDAO
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UserDAO(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<LoginDTO> LoginAsync(LoginDTO user)
        {
            var loginUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (loginUser == null)
            {
                return null;
            }

            if (loginUser.Password != user.Password)
            {
                return null;
            }

            return _mapper.Map<LoginDTO>(loginUser);
        }
        public async Task<RegisterDTO> RegisterUserAsync(RegisterDTO newUser)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == newUser.UserName);
            if (existingUser != null)
            {
                return null;
            }
            string userId = $"U{Guid.NewGuid()}";

            var user = new User
            {
                UserId = userId,
                UserName = newUser.UserName,
                UserRole = 1,
                Address = newUser.Address,
                PhoneNumber = newUser.PhoneNumber,
                Password = newUser.Password,
                Birthday = newUser.Birthday,
                Email = newUser.Email,
                FullName = newUser.FullName
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<RegisterDTO>(user);
        }

        public async Task ChangePasswordAsync(string userId, ChangePassDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (user.Password != dto.oldPass)
            {
                throw new Exception("Old password does not match");
            }
            user.Password = dto.newPass;

            await _context.SaveChangesAsync();
        }

        public async Task<UserDAO> GetUserByIdAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return _mapper.Map<UserDAO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            var usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users);
            return usersDTO;
        }

        public async Task<EditProfileDTO> GetUserById(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                return _mapper.Map<EditProfileDTO>(user);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in Get User By Id: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Get User By Id: {ex.Message}");
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

        public async Task<UserDTO> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUserByUserRoleAsync(int userRole)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserRole == userRole);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return _mapper.Map<UserDTO>(user);
        }

        public async Task UpdateUser(string id, EditProfileDTO updateDTO)
        {

            try
            {

                var exUser = await _context.Users.FindAsync(id);
                if (exUser == null)
                {
                    throw new Exception("Không tìm thấy bac si để cập nhật");
                }

                exUser.FullName = updateDTO.FullName;
                exUser.Address = updateDTO.Address;
                exUser.PhoneNumber = updateDTO.PhoneNumber;
                exUser.Email = updateDTO.Email;
                exUser.Birthday = updateDTO.Birthday;
                //exUser.ClinicId = updateDTO.ClinicId;

                var user = _mapper.Map<User>(exUser);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in UpdateClinic: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateClinic: {ex.Message}");
                throw;
            }
        }

    }
}