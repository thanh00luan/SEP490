using BussinessObject.Models;
using DataAccess.DTO.Admin;
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
        //Medicine Management
        //Task<IEnumerable<MedicineDTO>> GetAllMedicineAsync();
        //Task AddMedicineAsync(MedicineDTO Medicine);
        //Task UpdateMedicineAsync(MedicineDTO Medicine);
        //Task DeleteMedicineAsync(string MedicineId);


        //Category Management
        //Task<IEnumerable<MedicineCategory>> GetAllCategoryAsync();
        //Task AddCategoryAsync(MedicineCategory category);
        //Task UpdateCategoryAsync(MedicineCategory category);
        //Task DeleteCategoryAsync(string cateId);

        //Booking Management


        // User Management
        Task<IEnumerable<UserManaDTO>> GetAllUsersAsync();
        Task<UserManaDTO> GetUserByIdAsync(string userId);
        Task UpdateUserAsync(UserManaDTO user);
        Task DeleteUserAsync(string userId);

        // Statistic
        //Task<int> GetTotalNumberOfMedicinesAsync();
        //Task<int> GetTotalNumberOfBookingAsync();
        //Task<decimal> GetTotalSalesAsync();
        
    }
}
