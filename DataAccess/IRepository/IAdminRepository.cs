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
        Task<IEnumerable<MedicineManaDTO>> getAllMedicineAsync();
        Task CreateMedicineAsync(MedicineManaDTO medicineDTO);
        Task<MedicineManaDTO> GetMedicineByIdAsync(string medicineId);
        Task UpdateMedicineAsync(MedicineManaDTO medicineDTO);
        Task DeleteMedicineAsync(string medicineId);

        // User interface
        Task<IEnumerable<UserManaDTO>> GetAllUsersAsync();
        Task<UserManaDTO> GetUserByIdAsync(string userId);
        Task UpdateUserAsync(UserManaDTO user);
        Task DeleteUserAsync(string userId);

        //Staff interface
        Task AddStaff(StaffManaDTO staffDTO);
        Task<IEnumerable<StaffManaDTO>> GetAllStaff();
        Task<StaffManaDTO> GetStaffById(string id);
        Task UpdateStaff(StaffManaDTO staffDTO);

        Task DeleteStaffAsync(string id);

        // Statistic
        //Task<int> GetTotalNumberOfMedicinesAsync();
        //Task<int> GetTotalNumberOfBookingAsync();
        //Task<decimal> GetTotalSalesAsync();

    }
}
