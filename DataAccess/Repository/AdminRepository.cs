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

        //Medicine
        public Task<IEnumerable<MedicineManaDTO>> getAllMedicineAsync()
            =>_adminDAO.getAllMedicineAsync();
        public Task CreateMedicineAsync(MedicineManaDTO medicineDTO)
            => _adminDAO.CreateMedicineAsync(medicineDTO);
        public Task<MedicineManaDTO> GetMedicineByIdAsync(string medicineId)
            =>_adminDAO.GetMedicineByIdAsync(medicineId);
        public Task UpdateMedicineAsync( MedicineManaDTO medicineDTO)
            =>_adminDAO.UpdateMedicineAsync(medicineDTO);
        public Task DeleteMedicineAsync(string medicineId)
            =>_adminDAO.DeleteMedicineAsync(medicineId);

        //Staff
        public Task UpdateStaff(StaffManaDTO staffDTO)
            => _adminDAO.UpdateStaff(staffDTO);
        public Task AddStaff(StaffManaDTO staffDTO)
            =>_adminDAO.AddStaff(staffDTO);
        public Task<IEnumerable<StaffManaDTO>> GetAllStaff()
            => _adminDAO.GetAllStaff();
        public Task<StaffManaDTO> GetStaffById(string id)
            => _adminDAO.GetStaffById(id);
        public Task DeleteStaffAsync(string id)
            =>_adminDAO.DeleteStaffAsync(id);
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
