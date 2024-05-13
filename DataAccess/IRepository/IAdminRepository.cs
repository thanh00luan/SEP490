using BussinessObject.Models;
using DataAccess.DTO.Admin;
using DataAccess.DTO.Employee;
using DataAccess.DTO.Medicine;
using DataAccess.DTO.Precscription;
using DataAccess.DTO.SuperAD;
using DataAccess.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IAdminRepository
    {
        //Pet interface

        //Pet Category interface
        public Task CreatePetType(PetTypeManaDTO dto);
        Task<PetTypeManaDTO> GetAllPetCateAsync(string clinicId);
        Task<PetCateManaDTO> GetCateByIdAsync(string clinicId, string id);
        Task UpdatePetCateAsync(PetTypeManaDTO dto);

        //Medicine interface
        Task<MedicineListDTO> getAllMedicineAsync(string userId, int limit, int offset);
        Task CreateMedicineAsync(string userId, MedicineManaDTO medicineDTO);
        Task<MedicineManaDTO> GetMedicineByIdAsync(string userId, string medicineId);
        Task UpdateMedicineAsync(MedicineManaDTO medicineDTO);
        Task DeleteMedicineAsync(string medicineId);

        // User interface
        Task<UserListDTO> GetAllUsersAsync(int limit, int offset);
        Task<UserManaDTO> GetUserByIdAsync(string userId);
        Task UpdateUserAsync(UserManaDTO user);
        Task DeleteUserAsync(string userId);

        //Staff interface
        Task AddStaff(string user,StaffManaDTO staffDTO);
        Task<StaffListDTO> GetAllStaff(string clinicId, int limit, int offset);
        Task<StaffManaDTO> GetStaffById(string clinicId,string id);
        Task UpdateStaff(StaffManaDTO staffDTO);

        Task DeleteStaffAsync(string id);

        //Doctor
        Task AddDoctor(string userId, DoctorManaDTO doctorDTO);
        Task DeleteDoctor(string id);
        Task<DoctorListDTO> GetAllDoctors(string userId,int limit, int offset);
        Task<DoctorManaDTO> GetDoctorById(string userId, string id);
        Task<IEnumerable<DoctorManaDTO>> SortDoctorByName();
        Task UpdateDoctor(string userId, DoctorManaDTO updateDTO);
        Task<PresDTO> GetPrescription(string appointmentId);

        //Category medicine
        Task AddCategory(CateManaDTO dto);
        Task DeleteCategory(string id);
        Task<IEnumerable<CateManaDTO>> GetAllCategories();
        Task<CateManaDTO> GetCateById(string id);
        Task<IEnumerable<CateManaDTO>> SearchByName(string name);
        Task UpdateCate(CateManaDTO updateDTO);

        //Degree
        Task AddDegree(DoctorDegree dto);

        Task<IEnumerable<DoctorDegree>> GetDoctorDegrees();

        Task<MedicineReponse> GenerateMedicineSalesReport(DateTime startDate, DateTime endDate, string userId);
    }
}
