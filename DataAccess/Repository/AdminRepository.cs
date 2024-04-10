using DataAccess.DAO;
using DataAccess.DTO.Admin;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly SuperAdminDAO _adminDAO;
        public AdminRepository(SuperAdminDAO adminDAO)
        {
            _adminDAO = adminDAO;
        }
        public Task DeleteUserAsync(string userId)
            =>_adminDAO.DeleteUserAsync(userId);

        public Task<IEnumerable<UserManaDTO>> GetAllUsersAsync()
            =>_adminDAO.GetAllUsersAsync();

        public Task<UserManaDTO> GetUserByIdAsync(string userId)
            =>_adminDAO.GetUserByIdAsync(userId);

        public Task UpdateUserAsync(UserManaDTO user)
            =>_adminDAO.UpdateUserAsync(user);
    }
}
