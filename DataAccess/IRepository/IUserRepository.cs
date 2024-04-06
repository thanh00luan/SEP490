using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.DTO.User;

namespace DataAccess.IRepository
{
    public interface IUserRepository
    {
       Task<RegisterDTO> RegisterUserAsync(RegisterDTO newUser);
        Task<LoginDTO> LoginAsync(LoginDTO user);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task GrantPermissionAsync(string userId);
        Task RevokePermissionAsync(string userId);
        Task<EditProfileDTO> GetUserByIdAsync(string userId);
        Task<UserDTO> GetUserByUsernameAsync(string username);
        Task DeleteUserAsync(string userId);

        Task ChangePasswordAsync(string userId, ChangePassDTO dto);

        Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(int userRole);
        
    }
}