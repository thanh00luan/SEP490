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
        public Task<MedicineListDTO> getAllMedicineAsync(int limit, int offset)
            => _adminDAO.GetAllMedicineAsync(limit,offset);
        public Task CreateMedicineAsync(MedicineManaDTO medicineDTO)
            => _adminDAO.CreateMedicineAsync(medicineDTO);
        public Task<MedicineManaDTO> GetMedicineByIdAsync(string medicineId)
            =>_adminDAO.GetMedicineByIdAsync(medicineId);
        public Task UpdateMedicineAsync( MedicineManaDTO medicineDTO)
            =>_adminDAO.UpdateMedicineAsync(medicineDTO);
        public Task DeleteMedicineAsync(string medicineId)
            =>_adminDAO.DeleteMedicineAsync(medicineId);

        //Medicine Category

        //Staff
        public Task UpdateStaff(StaffManaDTO staffDTO)
            => _adminDAO.UpdateStaff(staffDTO);
        public Task AddStaff(StaffManaDTO staffDTO)
            =>_adminDAO.AddStaff(staffDTO);
        public Task<StaffListDTO> GetAllStaff(int limit, int offset)
            => _adminDAO.GetAllStaff(limit,offset);
        public Task<StaffManaDTO> GetStaffById(string id)
            => _adminDAO.GetStaffById(id);
        public Task DeleteStaffAsync(string id)
            =>_adminDAO.DeleteStaffAsync(id);
        //User
        public Task DeleteUserAsync(string userId)
            =>_adminDAO.DeleteUserAsync(userId);

        public Task<UserListDTO> GetAllUsersAsync(int limit, int offset)
            => _adminDAO.GetAllUsersAsync(limit, offset);

        public Task<UserManaDTO> GetUserByIdAsync(string userId)
            =>_adminDAO.GetUserByIdAsync(userId);

        public Task UpdateUserAsync(UserManaDTO user)
            =>_adminDAO.UpdateUserAsync(user);

        //Doctor
        public Task AddDoctor(DoctorManaDTO doctorDTO)
            =>_adminDAO.AddDoctor(doctorDTO);
        public Task DeleteDoctor(string id)
            =>_adminDAO.DeleteDoctor(id);
        public Task<DoctorListDTO> GetAllDoctors(int limit, int offset)
            => _adminDAO.GetAllDoctors(limit,offset);
        public Task<DoctorManaDTO> GetDoctorById(string id)
            =>_adminDAO.GetDoctorById(id);
        public Task<IEnumerable<DoctorManaDTO>> SortDoctorByName()
            =>_adminDAO.SortDoctorByName();
        public Task UpdateDoctor(DoctorManaDTO updateDTO)
            =>_adminDAO.UpdateDoctor(updateDTO);

        //Pet

        //Pet Category


    }
}
