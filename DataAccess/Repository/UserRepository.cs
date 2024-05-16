using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.DAO;
using DataAccess.DTO.Precscription;
using DataAccess.DTO.User;
using DataAccess.IRepository;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDAO _userDAO;

        public UserRepository(UserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        //Todo Repo đã triển khai
        

        public Task<LoginDTO> LoginAsync(LoginDTO user) => _userDAO.LoginAsync(user);
        public Task<RegisterDTO> RegisterUserAsync(RegisterDTO newUser) => _userDAO.RegisterUserAsync(newUser);
        public Task<IEnumerable<UserDTO>> GetAllUsersAsync() => _userDAO.GetAllUsersAsync();
        public Task<EditProfileDTO> GetUserByIdAsync(string userId) => _userDAO.GetUserById(userId);
        public Task ChangePasswordAsync(string userId, ChangePassDTO dto)=> _userDAO.ChangePasswordAsync(userId, dto);
        public Task DeleteUserAsync(string userId) => _userDAO.DeleteUserAsync(userId);
        public Task<UserDTO> GetUserByUsernameAsync(string username) => _userDAO.GetUserByUsernameAsync(username);
        //public Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(int userRole) => _userDAO.GetUsersByRoleAsync(userRole);


        //Todo Repo chưa triển khai

        public Task GrantPermissionAsync(string userId)
        {
            throw new NotImplementedException();
        }
        public Task RevokePermissionAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(int userRole)
        {
            throw new NotImplementedException();
        }

        public Task<PresDTO> GetPrescription(string appointmentId)
            =>_userDAO.GetPrescription(appointmentId);

        public Task<bool> ResetPasswordAsync(string userId,string newPassword)
            =>_userDAO.ResetPasswordAsync(userId, newPassword);

        public Task<string> ForgotPasswordAsync(string userName)
            =>_userDAO.ForgotPasswordAsync(userName);

        public Task<bool> VerifyOTPAsync(string userId, string oTP)
            =>_userDAO.VerifyOTPAsync(userId, oTP);
    }
}