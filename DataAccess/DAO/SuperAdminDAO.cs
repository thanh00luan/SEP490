using AutoMapper;
using BussinessObject.Data;
using BussinessObject.Models;
using DataAccess.DTO.Admin;
using DataAccess.DTO.DDoctor;
using DataAccess.DTO.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SuperAdminDAO
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SuperAdminDAO(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // UserManagement

        public async Task<IEnumerable<UserManaDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            var usersDTO = _mapper.Map<IEnumerable<UserManaDTO>>(users);
            return usersDTO;
        }

        public async Task<UserManaDTO> GetUserByIdAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return _mapper.Map<UserManaDTO>(user);
        }

        public async Task UpdateUserAsync(UserManaDTO updateUser)
        {
            try
            {
                var exUser = await _context.Users.FindAsync(updateUser.UserId);
                if (exUser == null)
                {
                    throw new Exception("User not found for updating");
                }

                exUser.FullName = updateUser.FullName;
                exUser.Address = updateUser.Address;
                exUser.PhoneNumber = updateUser.PhoneNumber;
                exUser.Email = updateUser.Email;
                exUser.Birthday = updateUser.Birthday;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database error in UpdateUser: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUser: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        //Doctor Management
        //Staff management
        public async Task AddStaff(StaffManaDTO staffDTO)
        {
            try
            {

                if (!string.IsNullOrEmpty(staffDTO.UserId))
                {
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == staffDTO.UserId);

                    if (existingUser != null)
                    {
                        existingUser.UserRole = 2;
                        var Employee = new Employee
                        {
                            EmployeeId = existingUser.UserId,
                            UserId = existingUser.UserId,
                        };

                        await _context.Employees.AddAsync(Employee);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                       
                    }
                }
                else
                {
                    var user = new User
                    {
                        UserId = Guid.NewGuid().ToString(),
                        FullName = staffDTO.EmployeeName,
                        Email = staffDTO.Email,
                        PhoneNumber = staffDTO.PhoneNumber,
                        Address = staffDTO.Address,
                        Birthday = staffDTO.Birthday,
                        UserRole = 2,
                    };
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    var newEmployee = new Employee
                    {
                        EmployeeId = user.UserId,
                        UserId = user.UserId,
                        EmployeeStatus = staffDTO.EmployeeStatus,
                    };

                    await _context.Employees.AddAsync(newEmployee);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<StaffManaDTO>> GetAllStaff()
        {
            try
            {

                var employees = await _context.Employees
                    .Include(e => e.User)
                    .ToListAsync();

                var staffDTOs = employees.Select(e => new StaffManaDTO
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.User.FullName,
                    Address = e.User.Address,
                    PhoneNumber = e.User.PhoneNumber,
                    Email = e.User.Email,
                    Birthday = e.User.Birthday,
                    EmployeeStatus = e.EmployeeStatus,
                    UserId = e.UserId
                });

                return staffDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllStaff: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateStaff(StaffManaDTO staffDTO)
        {
            try
            {
                var employee = await _context.Employees
                           .Include(e => e.User)
                           .FirstOrDefaultAsync(e => e.UserId == staffDTO.UserId);
                if (employee == null)
                {
                    return;
                }
                employee.User.FullName = staffDTO.EmployeeName;
                employee.EmployeeStatus = staffDTO.EmployeeStatus;
                employee.User.Address = staffDTO.Address;
                employee.User.PhoneNumber = staffDTO.PhoneNumber;
                employee.User.Birthday = staffDTO.Birthday;
                employee.User.Email = staffDTO.Email;
                if (!string.IsNullOrEmpty(staffDTO.UserId))
                {
                    var user = await _context.Users.FirstOrDefaultAsync(c => c.UserId == staffDTO.UserId);
                    if (user != null)
                    {
                        employee.UserId = staffDTO.UserId;
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in UpdateStaff: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateStaff: {ex.Message}");
                throw;
            }
        }

        public async Task<StaffManaDTO> GetStaffById(string id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                var user = await _context.Users.FindAsync(employee.UserId);
                var emp = new StaffManaDTO
                {
                    EmployeeStatus = employee.EmployeeStatus,
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = user.FullName,
                    Address = user.Address,
                    Birthday = user.Birthday,
                    UserId = user.UserId,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber

                };
                return emp;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteStaffAsync(string staffId)
        {
            var staff = await _context.Employees.FindAsync(staffId);
            if (staff == null)
            {
                throw new Exception("Staff not found");
            }

            _context.Employees.Remove(staff);
            await _context.SaveChangesAsync();
        }

        //Doctor
        public async Task AddDoctor(DoctorManaDTO doctorDTO)
        {
            try
            {

                if (!string.IsNullOrEmpty(doctorDTO.UserId))
                {
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == doctorDTO.UserId);

                    if (existingUser != null)
                    {
                        existingUser.UserRole = 3;
                        var doctor = new Doctor
                        {
                            DoctorId = existingUser.UserId,
                            UserId = existingUser.UserId,
                            Degree = doctorDTO.Degree,
                            Specialized = doctorDTO.Specialized,
                            Status = doctorDTO.DoctorStatus
                        };

                        await _context.Doctors.AddAsync(doctor);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {

                    }
                }
                else
                {
                    var user = new User
                    {
                        UserId = Guid.NewGuid().ToString(),
                        FullName = doctorDTO.DoctorName,
                        Email = doctorDTO.Email,
                        PhoneNumber = doctorDTO.PhoneNumber,
                        Address = doctorDTO.Address,
                        Birthday = doctorDTO.BirthDate,
                        UserRole = 3,
                    };
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    var newDoctor = new Doctor
                    {
                        DoctorId = user.UserId,
                        UserId = user.UserId,
                        Degree = doctorDTO.Degree,
                        Specialized = doctorDTO.Specialized,
                        Status = doctorDTO.DoctorStatus
                    };

                    await _context.Doctors.AddAsync(newDoctor);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        public async Task DeleteDoctor(string id)
        {
            try
            {
                var Doctor = await _context.Doctors.FindAsync(id);
                if (Doctor != null)
                {
                    _context.Doctors.Remove(Doctor);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in DeleteDoctor: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteDoctor: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<DoctorDTO>> GetAllDoctors()
        {
            try
            {
                var Doctors = await _context.Doctors.ToListAsync();
                var DoctorDTOs = _mapper.Map<IEnumerable<DoctorDTO>>(Doctors);
                return DoctorDTOs;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in GetAllDoctors: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllDoctors: {ex.Message}");
                throw;
            }
        }


        //public async Task<AddNewDoctorDTO> GetDoctorById(string id)
        //{
        //    try
        //    {
        //        var Doctor = await _context.Doctors.FindAsync(id);
        //        return _mapper.Map<AddNewDoctorDTO>(Doctor);
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        Console.WriteLine($"Database Error in GetDoctorDTOById: {ex.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in GetDoctorDTOById: {ex.Message}");
        //        throw;
        //    }
        //}

        //public async Task<IEnumerable<DoctorDTO>> SortDoctorByName()
        //{
        //    try
        //    {
        //        var sortedDoctors = await _context.Doctors.OrderBy(d => d.DoctorName).ToListAsync();
        //        var sortedDoctorDTOs = _mapper.Map<IEnumerable<DoctorDTO>>(sortedDoctors);
        //        return sortedDoctorDTOs;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        Console.WriteLine($"Database Error in SortDoctorByName: {ex.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in SortDoctorByName: {ex.Message}");
        //        throw;
        //    }
        //}



        //public async Task UpdateDoctor(AddNewDoctorDTO updateDTO)
        //{

        //    try
        //    {

        //        var existingDoctor = await _context.Doctors.FindAsync(updateDTO.DoctorId);
        //        if (existingDoctor == null)
        //        {
        //            throw new Exception("Không tìm thấy bac si để cập nhật");
        //        }
        //        existingDoctor.DoctorId = updateDTO.DoctorId;
        //        existingDoctor.DoctorName = updateDTO.DoctorName;
        //        existingDoctor.Address = updateDTO.Address;
        //        existingDoctor.PhoneNumber = updateDTO.PhoneNumber;
        //        existingDoctor.Specialized = updateDTO.Specialized;
        //        existingDoctor.DoctorStatus = updateDTO.DoctorStatus;
        //        //existingDoctor.ClinicId = updateDTO.ClinicId;

        //        var doctor = _mapper.Map<Doctor>(existingDoctor);
        //        _context.Doctors.Update(doctor);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        Console.WriteLine($"Database Error in UpdateClinic: {ex.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in UpdateClinic: {ex.Message}");
        //        throw;
        //    }
        //}

        //Medicine
        public async Task CreateMedicineAsync(MedicineManaDTO medicineDTO)
        {
            var medicine = new Medicine
            {
                MedicineId = medicineDTO.MedicineId,
                MedicineName = medicineDTO.MedicineName,
                MedicineUnit = medicineDTO.MedicineUnit,
                Prices = medicineDTO.Prices,
                Inventory = medicineDTO.Inventory,
                Specifications = medicineDTO.Specifications,
                MedicineCateId = medicineDTO.MedicineCateId
            };

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MedicineManaDTO>> getAllMedicineAsync()
        {
            var medicines = await _context.Medicines.Include(m=>m.MedicineCategory).ToListAsync();

            return medicines.Select(medicine => new MedicineManaDTO
            {
                MedicineId = medicine.MedicineId,
                MedicineName = medicine.MedicineName,
                MedicineUnit = medicine.MedicineUnit,
                Prices = medicine.Prices,
                Inventory = medicine.Inventory,
                Specifications = medicine.Specifications,
                MedicineCateId = medicine.MedicineCateId,
                MedicineCateName = medicine.MedicineCategory.CategoryName 
            }).ToList();
        }

        public async Task<MedicineManaDTO> GetMedicineByIdAsync(string medicineId)
        {
            var medicine = await _context.Medicines.FindAsync(medicineId);

            if (medicine == null)
            {
                return null; 
            }

            return new MedicineManaDTO
            {
                MedicineId = medicine.MedicineId,
                MedicineName = medicine.MedicineName,
                MedicineUnit = medicine.MedicineUnit,
                Prices = medicine.Prices,
                Inventory = medicine.Inventory,
                Specifications = medicine.Specifications,
                MedicineCateId = medicine.MedicineCateId
            };
        }

        public async Task UpdateMedicineAsync( MedicineManaDTO medicineDTO)
        {
            var medicine = await _context.Medicines.FindAsync(medicineDTO.MedicineId);

            if (medicine == null)
            {
                return; 
            }

            medicine.MedicineName = medicineDTO.MedicineName;
            medicine.MedicineUnit = medicineDTO.MedicineUnit;
            medicine.Prices = medicineDTO.Prices;
            medicine.Inventory = medicineDTO.Inventory;
            medicine.Specifications = medicineDTO.Specifications;
            medicine.MedicineCateId = medicineDTO.MedicineCateId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMedicineAsync(string medicineId)
        {
            var medicine = await _context.Medicines.FindAsync(medicineId);

            if (medicine == null)
            {
                return; 
            }

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
        }
    }
}
