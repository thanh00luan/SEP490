using DataAccess.DAO;
using DataAccess.DTO.Admin;
using DataAccess.DTO.Employee;
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

        //Staff
        public Task UpdateStaff(StaffManaDTO staffDTO)
            => _adminDAO.UpdateStaff(staffDTO);
        public Task AddStaff(StaffManaDTO staffDTO)
            =>_adminDAO.AddStaff(staffDTO);
        public Task<IEnumerable<StaffManaDTO>> GetAllStaff()
            => _adminDAO.GetAllStaff();
        public Task<StaffManaDTO> GetStaffById(string id)
            => _adminDAO.GetStaffById(id);
        //User
        public Task DeleteUserAsync(string userId)
            =>_adminDAO.DeleteUserAsync(userId);

        public Task<IEnumerable<UserManaDTO>> GetAllUsersAsync()
            =>_adminDAO.GetAllUsersAsync();

        public Task<UserManaDTO> GetUserByIdAsync(string userId)
            =>_adminDAO.GetUserByIdAsync(userId);

        public Task UpdateUserAsync(UserManaDTO user)
            =>_adminDAO.UpdateUserAsync(user);

        //Pet

        //Pet Category


    }
}
