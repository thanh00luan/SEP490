using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BussinessObject.Data;
using BussinessObject.Models;
using DataAccess.DTO.DDoctor;
using DataAccess.DTO.Precscription;
using DataAccess.DTO.User;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class UserDAO
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICache _cache;
        private readonly ISendMailService _sendMailService;

        private readonly string ngrokLink = "https://d5f8-2001-ee0-533c-84a0-6122-1d18-8549-4457.ngrok-free.app";
        
        public UserDAO(ApplicationDbContext context, IMapper mapper, ICache cache, ISendMailService sendMailService)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _sendMailService = sendMailService;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public async Task<LoginDTO> LoginAsync(LoginDTO user)
        {
            var loginUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (loginUser == null)
            {
                return null;
            }

            if (HashPassword(user.Password) != loginUser.Password)
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
            string hashedPassword = HashPassword(newUser.Password);

            var user = new User
            {
                UserId = userId,
                UserName = newUser.UserName,
                UserRole = 1,
                Address = newUser.Address,
                PhoneNumber = newUser.PhoneNumber,
                Password = hashedPassword,
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

            string hashedNewPassword = HashPassword(dto.newPass);

            if (HashPassword(dto.oldPass) != user.Password)
            {
                throw new Exception("Old password does not match");
            }

            user.Password = hashedNewPassword;

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
                var user = await _context.Users.FirstOrDefaultAsync(u=>u.UserId == id);
                return new EditProfileDTO
                {
                    Address = user.Address,
                    Birthday = user.Birthday,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName
                };
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

        public async Task<PresDTO> GetPrescription(string appointmentId)
        {
            try
            {
                var prescription = await _context.Prescriptions
                    .Include(p => p.PrescriptionMedicines)
                        .ThenInclude(m => m.Medicine) 
                    .Include(p => p.Appointment)
                        .ThenInclude(a => a.Pet)
                    .Include(p => p.Appointment)
                        .ThenInclude(a => a.User)
                    .FirstOrDefaultAsync(p => p.AppointmentId == appointmentId);

                if (prescription == null)
                {
                    return null;
                }

                var presDTO = new PresDTO
                {
                    PrescriptionId = prescription.PrescriptionId,
                    Diagnose = prescription.Diagnose,
                    ExaminationDay = prescription.ExaminationDay,
                    CreateDay = prescription.CreateDay,
                    Note = prescription.Reason,
                    PetId = prescription.Appointment.Pet.PetId,
                    PetName = prescription.Appointment.Pet.PetName,
                    UserId = prescription.Appointment.User.UserId,
                    FullName = prescription.Appointment.User.FullName,
                    PrescriptionMedicines = prescription.PrescriptionMedicines.Select(m => new PrescriptionMedicineInfoDTO
                    {
                        MedicineId = m.MedicineId,
                        Quantity = m.Quantity,
                        MedicineName = m.Medicine.MedicineName,
                        PricePerUnit = m.Medicine.Prices, 
                        TotalPrice = m.Quantity * m.Medicine.Prices 
                    }).ToList(),
                    TotalPrices = prescription.PrescriptionMedicines.Sum(m => m.Quantity * m.Medicine.Prices) 
                };

                return presDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving prescription by appointmentId: {ex.Message}");
                throw;
            }
        }

        //Forgot pass
        public async Task ForgotPasswordAsync(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                throw new Exception("User not found");
            }
            string resetToken = GenerateResetToken();

            _cache.Set($"resetToken:{userName}", resetToken, TimeSpan.FromHours(1));
            await _sendMailService.SendForgotPasswordEmailAsync(user.Email, resetToken);
        }

        public async Task ResetPasswordAsync(string userName, string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            string cachedToken = _cache.Get<string>($"resetToken:{userName}");

            if (token != cachedToken)
            {
                throw new Exception("Invalid or expired token");
            }

            string hashedNewPassword = HashPassword(newPassword);
            user.Password = hashedNewPassword;

            await _context.SaveChangesAsync();

            _cache.Remove($"resetToken:{userName}");
        }
        private string GenerateResetToken()
        {
            const int tokenLength = 32;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var random = new Random();
            var token = new char[tokenLength];

            for (int i = 0; i < tokenLength; i++)
            {
                token[i] = chars[random.Next(chars.Length)];
            }

            return new string(token);
        }




    }

}
