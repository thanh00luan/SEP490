using BussinessObject.Models;
using DataAccess.DTO.Admin;
using DataAccess.DTO.Employee;
using DataAccess.DTO.Medicine;
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

        //Medicine interface
        Task<MedicineListDTO> getAllMedicineAsync(int limit, int offset);
        Task CreateMedicineAsync(MedicineManaDTO medicineDTO);
        Task<MedicineManaDTO> GetMedicineByIdAsync(string medicineId);
        Task UpdateMedicineAsync(MedicineManaDTO medicineDTO);
        Task DeleteMedicineAsync(string medicineId);

        // User interface
        Task<UserListDTO> GetAllUsersAsync(int limit, int offset);
        Task<UserManaDTO> GetUserByIdAsync(string userId);
        Task UpdateUserAsync(UserManaDTO user);
        Task DeleteUserAsync(string userId);

        //Staff interface
        Task AddStaff(StaffManaDTO staffDTO);
        Task<StaffListDTO> GetAllStaff(int limit, int offset);
        Task<StaffManaDTO> GetStaffById(string id);
        Task UpdateStaff(StaffManaDTO staffDTO);

        Task DeleteStaffAsync(string id);

        //Doctor
        Task AddDoctor(DoctorManaDTO doctorDTO);
        Task DeleteDoctor(string id);
        Task<DoctorListDTO> GetAllDoctors(int limit, int offset);
        Task<DoctorManaDTO> GetDoctorById(string id);
        Task<IEnumerable<DoctorManaDTO>> SortDoctorByName();
        Task UpdateDoctor(DoctorManaDTO updateDTO);

        // Statistic
        //Task<int> GetTotalNumberOfMedicinesAsync();
        //Task<int> GetTotalNumberOfBookingAsync();
        //Task<decimal> GetTotalSalesAsync();

    }
}
