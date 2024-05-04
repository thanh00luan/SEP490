using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO.Admin;
using DataAccess.DTO.Employee;
using DataAccess.DTO.Precscription;
using DataAccess.IRepository;
using DataAccess.RequestDTO;
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

        //Pet type Category per clinic
        public Task<string> CreatePetType(string clinicId, string petTypeId)
            =>_adminDAO.CreatePetType(clinicId, petTypeId);

        public Task<IEnumerable<PetTypeManaDTO>> GetAllPetCateAsync(string clinicId)
            => _adminDAO.GetAllPetCateAsync(clinicId);
        public Task<PetTypeManaDTO> GetCateByIdAsync(string clinicId, string id)
            =>_adminDAO.GetCateByIdAsync(clinicId, id);
        public Task UpdatePetCateAsync(PetTypeManaDTO dto)
            => _adminDAO.UpdatePetCateAsync(dto);
        public Task DeletePetCateAsync(string clinicId, string id)
            => _adminDAO.DeletePetCateAsync(clinicId, id);

        //Medicine
        public Task<MedicineListDTO> getAllMedicineAsync(string clinicId, int limit, int offset)
            => _adminDAO.GetAllMedicineAsync(clinicId, limit ,offset);
        public Task CreateMedicineAsync(string clinicId, MedicineManaDTO medicineDTO)
            => _adminDAO.CreateMedicineAsync(clinicId, medicineDTO);
        public Task<MedicineManaDTO> GetMedicineByIdAsync(string clinicId, string medicineId)
            =>_adminDAO.GetMedicineByIdAsync(clinicId, medicineId);
        public Task UpdateMedicineAsync(string clinicId, MedicineManaDTO medicineDTO)
            =>_adminDAO.UpdateMedicineAsync(clinicId, medicineDTO);
        public Task DeleteMedicineAsync(string clinicId, string medicineId)
            =>_adminDAO.DeleteMedicineAsync(clinicId, medicineId);

        //Staff
        public Task UpdateStaff(StaffManaDTO staffDTO)
            => _adminDAO.UpdateStaff(staffDTO);
        public Task AddStaff(StaffManaDTO staffDTO)
            =>_adminDAO.AddStaff(staffDTO);
        public Task<StaffListDTO> GetAllStaff(string clinicId, int limit, int offset)
            => _adminDAO.GetAllStaff(clinicId, limit,offset);
        public Task<StaffManaDTO> GetStaffById(string clinicId, string id)
            => _adminDAO.GetStaffById(clinicId, id);
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
        public Task<PresDTO> GetPrescription(string appointmentId)
            => null;
        public Task AddDoctor(DoctorManaDTO doctorDTO)
            =>_adminDAO.AddDoctor(doctorDTO);
        public Task DeleteDoctor(string id)
            =>_adminDAO.DeleteDoctor(id);
        public Task<DoctorListDTO> GetAllDoctors(string clinicId, int limit, int offset)
            => _adminDAO.GetAllDoctors(clinicId,limit,offset);
        public Task<DoctorManaDTO> GetDoctorById(string clinicId, string id)
            =>_adminDAO.GetDoctorById(clinicId,id);
        public Task<IEnumerable<DoctorManaDTO>> SortDoctorByName()
            =>_adminDAO.SortDoctorByName();
        public Task UpdateDoctor(DoctorManaDTO updateDTO)
            =>_adminDAO.UpdateDoctor(updateDTO);

        //Category
        public Task AddCategory(CateManaDTO dto)
            => _adminDAO.CreateCategoryAsync(dto);  

        public Task DeleteCategory(string id)
            =>_adminDAO.DeleteCateAsync(id);

        public Task<IEnumerable<CateManaDTO>> GetAllCategories()
            =>_adminDAO.GetAllCateAsync();

        public Task<CateManaDTO> GetCateById(string id)
            =>_adminDAO.GetCateByIdAsync(id);

        public Task<IEnumerable<CateManaDTO>> SearchByName(string name)
            => _adminDAO.SearchByName(name);

        public Task UpdateCate(CateManaDTO updateDTO)
            =>_adminDAO.UpdateCateAsync(updateDTO);

        //Pet

        //Pet Category


        //Degree
        public Task AddDegree(DoctorDegree dto)
            => _adminDAO.CreateDegreeAsync(dto);

        public Task<IEnumerable<DoctorDegree>> GetDoctorDegrees()
            => _adminDAO.GetAllDegreeAsync();


    }
}
