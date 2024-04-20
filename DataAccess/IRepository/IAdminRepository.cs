﻿using BussinessObject.Models;
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
        Task<MedicineListDTO> getAllMedicineAsync(string clinicId, int limit, int offset);
        Task CreateMedicineAsync(string clinicId, MedicineManaDTO medicineDTO);
        Task<MedicineManaDTO> GetMedicineByIdAsync(string clinicId, string medicineId);
        Task UpdateMedicineAsync(string clinicId, MedicineManaDTO medicineDTO);
        Task DeleteMedicineAsync(string clinicId, string medicineId);

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

        //Category medicine
        Task AddCategory(CateManaDTO dto);
        Task DeleteCategory(string id);
        Task<IEnumerable<CateManaDTO>> GetAllCategories();
        Task<CateManaDTO> GetCateById(string id);
        Task<IEnumerable<CateManaDTO>> SearchByName(string name);
        Task UpdateCate(CateManaDTO updateDTO);

        // Statistic
        //Task<int> GetTotalNumberOfMedicinesAsync();
        //Task<int> GetTotalNumberOfBookingAsync();
        //Task<decimal> GetTotalSalesAsync();

        //Inventory
        //Task ImportToStorage(StorageDTO storageDTO);
        //Task ExportFromStorage(string importCode, int exportQuantity);
        //Task<IEnumerable<StorageDTO>> GetAllStorageItems(int limit, int offset);
        //Task<StorageDTO> GetStorageItemByImportCode(string importCode);
        //Task<IEnumerable<StorageDTO>> GetExpiredItems(DateTime currentDate);

    }
}
