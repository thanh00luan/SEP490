using AutoMapper;
using BussinessObject.Data;
using BussinessObject.Models;
using DataAccess.DTO;
using DataAccess.DTO.Admin;
using DataAccess.DTO.DDoctor;
using DataAccess.DTO.Employee;
using DataAccess.DTO.SuperAD;
using DataAccess.Repository;
using DataAccess.RequestDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SuperAdminDAO
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly SendMailService _sendMailService;

        public SuperAdminDAO(ApplicationDbContext context, IMapper mapper, SendMailService sendMailService)
        {
            _context = context;
            _mapper = mapper;
            _sendMailService = sendMailService;
        }

        // UserManagement

        public async Task<UserListDTO> GetAllUsersAsync(int limit, int offset)
        {
            try
            {
                var usersQuery = _context.Users.OrderBy(u => u.UserId);

                var totalUsers = await usersQuery.CountAsync();

                var users = await usersQuery.Skip(offset).Take(limit).ToListAsync();

                var usersDTO = _mapper.Map<IEnumerable<UserManaDTO>>(users);

                return new UserListDTO
                {
                    TotalUsers = totalUsers,
                    Users = usersDTO
                };
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in GetAllUsersAsync: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllUsersAsync: {ex.Message}");
                throw;
            }
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
        public async Task AddStaff(string userId, StaffManaDTO staffDTO)
        {
            try
            {
                var emp = await _context.Employees.FirstOrDefaultAsync(e=>e.UserId == userId);

                if (!string.IsNullOrEmpty(staffDTO.UserId))
                {
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == staffDTO.UserId);

                    if (existingUser != null)
                    {
                        existingUser.UserRole = 3;
                        var Employee = new Employee
                        {
                            EmployeeId = existingUser.UserId,
                            UserId = existingUser.UserId,
                            ClinicId = emp.ClinicId,
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
                        UserRole = 3,
                    };
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    var newEmployee = new Employee
                    {
                        EmployeeId = user.UserId,
                        UserId = user.UserId,
                        EmployeeStatus = staffDTO.EmployeeStatus,
                        ClinicId = emp.ClinicId,
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
        public async Task<StaffListDTO> GetAllStaff(string userId, int limit, int offset)
        {
            try
            {
                var emp = _context.Employees
                    .Include(e=>e.Clinic)
                    .FirstOrDefault(e => e.UserId == userId);
                var staffQuery = _context.Employees
                    .Include(e =>e.Clinic)
                    .Include(e => e.User).OrderBy(e => e.UserId);

                
                if(emp.ClinicId != null)
                {
                    staffQuery = _context.Employees
                        .Where(e=>e.ClinicId == emp.ClinicId)
                        .Include(e => e.User).OrderBy(e => e.UserId);
                }

                var totalStaff = await staffQuery.CountAsync();

                var employees = await staffQuery.Skip(offset).Take(limit).ToListAsync();

                var staffDTOs = employees.Select(e => new StaffManaDTO
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.User.FullName,
                    Address = e.User.Address,
                    PhoneNumber = e.User.PhoneNumber,
                    Email = e.User.Email,
                    Birthday = e.User.Birthday,
                    EmployeeStatus = e.EmployeeStatus,
                    UserId = e.UserId,
                    ClinicId = emp.ClinicId,
                });

                return new StaffListDTO
                {
                    TotalStaff = totalStaff,
                    Staffs = staffDTOs
                };
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

        //public async Task<StaffManaDTO> GetStaffById( string id)
        //{
        //    try
        //    {
        //        var employee = await _context.Employees.FindAsync(id);
        //        var user = await _context.Users.FindAsync(employee.UserId);
        //        var emp = new StaffManaDTO
        //        {
        //            EmployeeStatus = employee.EmployeeStatus,
        //            EmployeeId = employee.EmployeeId,
        //            EmployeeName = user.FullName,
        //            Address = user.Address,
        //            Birthday = user.Birthday,
        //            UserId = user.UserId,
        //            Email = user.Email,
        //            PhoneNumber = user.PhoneNumber

        //        };
        //        return emp;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        Console.WriteLine($"Database Error: {ex.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        throw;
        //    }
        //}

        public async Task<StaffManaDTO> GetStaffById(string userId, string id)
        {
            try
            {
                var empl = await _context.Employees.FirstOrDefaultAsync(d => d.UserId == userId);
                var staff = await _context.Employees
                    .Include(d => d.User)
                    .Include(d => d.Clinic)
                    .FirstOrDefaultAsync(d => d.EmployeeId == id);

                if (empl.ClinicId != null)
                {
                    staff = await _context.Employees
                    .Where(e => e.ClinicId == empl.ClinicId)
                    .Include(d => d.User)
                    .Include(d => d.Clinic)
                    .FirstOrDefaultAsync(d => d.EmployeeId == id);
                }

                if (staff == null)
                {

                    return null;
                }
                
                var emp = new StaffManaDTO
                {
                    EmployeeStatus = staff.EmployeeStatus,
                    EmployeeId = staff.EmployeeId,
                    EmployeeName = staff.User.FullName,
                    Address = staff.User.Address,
                    Birthday = staff.User.Birthday,
                    UserId = staff.UserId,
                    Email = staff.User.Email,
                    PhoneNumber = staff.User.PhoneNumber,
                    ClinicId = staff.ClinicId,
                };

                return emp;
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in GetDoctorById: {ex.Message}");
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
        public async Task AddDoctor(string userId, DoctorManaDTO doctorDTO)
        {
            try
            {
                var emp = await _context.Employees.FirstOrDefaultAsync(d=>d.UserId==userId);
                if (!string.IsNullOrEmpty(doctorDTO.UserId))
                {
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == doctorDTO.UserId);

                    if (existingUser != null)
                    {
                        existingUser.UserRole = 2;
                        var doctor = new Doctor
                        {
                            DoctorId = existingUser.UserId,
                            UserId = existingUser.UserId,
                            DegreeId = doctorDTO.DegreeId,
                            Specialized = doctorDTO.Specialized,
                            Status = doctorDTO.DoctorStatus,
                            ClinicId = emp.ClinicId
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
                    var uniqueUsername = await GenerateUniqueUsername(doctorDTO.DoctorName);

                    var user = new User
                    {
                        UserId = Guid.NewGuid().ToString(), 
                        UserName = uniqueUsername,
                        FullName = doctorDTO.DoctorName,
                        Email = doctorDTO.Email,
                        PhoneNumber = doctorDTO.PhoneNumber,
                        Address = doctorDTO.Address,
                        Birthday = doctorDTO.BirthDate,
                        UserRole = 2,
                        
                        
                    };
                    var password = "12345678";

                    var hashedPassword = HashPassword(password);

                    user.Password = hashedPassword;

                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    var newDoctor = new Doctor
                    {
                        DoctorId = user.UserId,
                        UserId = user.UserId,
                        DegreeId = doctorDTO.DegreeId,
                        Specialized = doctorDTO.Specialized,
                        Status = doctorDTO.DoctorStatus,
                        ClinicId = emp.ClinicId
                    };

                    await _context.Doctors.AddAsync(newDoctor);
                    await _context.SaveChangesAsync();

                    var email = doctorDTO.Email;
                    var subject = "Thông tin tài khoản của bạn";
                    var body = $"Xin chào {user.FullName},\n\nTài khoản của bạn đã được tạo thành công.\n\nTên đăng nhập: {user.UserName}\nMật khẩu: {password}\n\nXin cảm ơn.";

                    await _sendMailService.SendEmailAsync(email, subject, body);
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Lỗi Cơ sở dữ liệu: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                throw;
            }
        }

        private async Task<string> GenerateUniqueUsername(string doctorName)
        {
            string baseUsername = GenerateBaseUsername(doctorName);

            string username = baseUsername;
            int counter = 1;
            while (await IsUsernameExists(username))
            {
                username = $"{baseUsername}{counter}";
                counter++;
            }

            return username;
        }

        private async Task<bool> IsUsernameExists(string username)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == username);
            return existingUser != null;
        }

        private string GenerateBaseUsername(string doctorName)
        {
            Dictionary<char, char> unikeyMap = new Dictionary<char, char>
    {
        {'á', 'a'},
        {'à', 'a'},
        {'ả', 'a'},
        {'ã', 'a'},
        {'ạ', 'a'},
        {'ă', 'a'},
        {'ắ', 'a'},
        {'ằ', 'a'},
        {'ẳ', 'a'},
        {'ẵ', 'a'},
        {'ặ', 'a'},
        {'â', 'a'},
        {'ấ', 'a'},
        {'ầ', 'a'},
        {'ẩ', 'a'},
        {'ẫ', 'a'},
        {'ậ', 'a'},
        {'đ', 'd'},
        {'é', 'e'},
        {'è', 'e'},
        {'ẻ', 'e'},
        {'ẽ', 'e'},
        {'ẹ', 'e'},
        {'ê', 'e'},
        {'ế', 'e'},
        {'ề', 'e'},
        {'ể', 'e'},
        {'ễ', 'e'},
        {'ệ', 'e'},
        {'í', 'i'},
        {'ì', 'i'},
        {'ỉ', 'i'},
        {'ĩ', 'i'},
        {'ị', 'i'},
        {'ó', 'o'},
        {'ò', 'o'},
        {'ỏ', 'o'},
        {'õ', 'o'},
        {'ọ', 'o'},
        {'ô', 'o'},
        {'ố', 'o'},
        {'ồ', 'o'},
        {'ổ', 'o'},
        {'ỗ', 'o'},
        {'ộ', 'o'},
        {'ơ', 'o'},
        {'ớ', 'o'},
        {'ờ', 'o'},
        {'ở', 'o'},
        {'ỡ', 'o'},
        {'ợ', 'o'},
        {'ú', 'u'},
        {'ù', 'u'},
        {'ủ', 'u'},
        {'ũ', 'u'},
        {'ụ', 'u'},
        {'ư', 'u'},
        {'ứ', 'u'},
        {'ừ', 'u'},
        {'ử', 'u'},
        {'ữ', 'u'},
        {'ự', 'u'},
        {'ý', 'y'},
        {'ỳ', 'y'},
        {'ỷ', 'y'},
        {'ỹ', 'y'},
        {'ỵ', 'y'}
    };

            string lowercaseName = doctorName.ToLower();

            StringBuilder usernameBuilder = new StringBuilder();
            foreach (char c in lowercaseName)
            {
                if (unikeyMap.ContainsKey(c))
                {
                    usernameBuilder.Append(unikeyMap[c]);
                }
                else if (char.IsLetterOrDigit(c))
                {
                    usernameBuilder.Append(c);
                }
            }
            return usernameBuilder.ToString();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
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

        public async Task<DoctorListDTO> GetAllDoctors(string userId, int limit, int offset)
        {
            try
            {
                var emp = _context.Employees.FirstOrDefault(e => e.UserId == userId);
                var doctorsQuery = _context.Doctors.Include(d => d.User).Include(d => d.DoctorDegree).OrderBy(d => d.DoctorId);
                if (emp.ClinicId != null)
                {
                    doctorsQuery = _context.Doctors
                        .Where(d => d.ClinicId == emp.ClinicId)
                        .Include(d => d.User)
                        .Include(d => d.DoctorDegree)
                        .OrderBy(d => d.DoctorId);
                }
                

                var totalDoctors = await doctorsQuery.CountAsync();

                var doctors = await doctorsQuery.Skip(offset).Take(limit).ToListAsync();

                var doctorDTOs = doctors.Select(e => new DoctorManaDTO
                {
                    DoctorId = e.DoctorId,
                    DoctorName = e.User.FullName,
                    Address = e.User.Address,
                    PhoneNumber = e.User.PhoneNumber,
                    Email = e.User.Email,
                    BirthDate = e.User.Birthday,
                    DoctorStatus = e.Status,
                    DegreeId = e.DegreeId,  
                    Specialized = e.Specialized,
                    ClinicId = e.ClinicId,
                    UserId = e.UserId
                });
                return new DoctorListDTO
                {
                    TotalDoctor = totalDoctors,
                    Doctors = doctorDTOs
                };
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




        public async Task<DoctorManaDTO> GetDoctorById(string userId,string id)
        {
            try
            {
                var emp = _context.Employees.FirstOrDefault(e => e.UserId == userId);
                var doctor = await _context.Doctors
                    .Include(d => d.User)
                    .Include(d =>d.DoctorDegree)
                    .FirstOrDefaultAsync(d => d.DoctorId == id);

                if(emp.ClinicId != null)
                {
                    doctor = await _context.Doctors
                   .Where(d => d.ClinicId == emp.ClinicId)
                   .Include(d => d.User)
                   .Include(d => d.DoctorDegree)
                   .FirstOrDefaultAsync(d => d.DoctorId == id);
                }

                if (doctor == null)
                {

                    return null;
                }

                var doctorDTO = new DoctorManaDTO
                {
                    DoctorId = doctor.DoctorId,
                    DoctorName = doctor.User.FullName,
                    Address = doctor.User.Address,
                    PhoneNumber = doctor.User.PhoneNumber,
                    Email = doctor.User.Email,
                    BirthDate = doctor.User.Birthday,
                    DoctorStatus = doctor.Status,
                    DegreeId = doctor.DegreeId,
                    Specialized = doctor.Specialized,
                    UserId = doctor.UserId
                };

                return doctorDTO;
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in GetDoctorById: {ex.Message}");
                throw;
            }
        }


        public async Task<IEnumerable<DoctorManaDTO>> SortDoctorByName()
        {
            try
            {
                var sortedDoctors = await _context.Doctors.Include(d => d.User).Include(d => d.DoctorDegree).OrderBy(d => d.User.FullName).ToListAsync();
                var sortedDoctorDTOs = _mapper.Map<IEnumerable<DoctorManaDTO>>(sortedDoctors);
                return sortedDoctorDTOs;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in SortDoctorByName: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SortDoctorByName: {ex.Message}");
                throw;
            }
        }



        public async Task UpdateDoctor(string userId, DoctorManaDTO updateDTO)
        {

            try
            {

                var existingDoctor = await _context.Doctors.Include(d => d.User).Include(d => d.DoctorDegree)
                    .FirstOrDefaultAsync(d => d.DoctorId == updateDTO.DoctorId);
                if (existingDoctor == null)
                {
                    throw new Exception("Error in get doctor.");
                }
                existingDoctor.User.FullName = updateDTO.DoctorName;
                existingDoctor.User.Address = updateDTO.Address;
                existingDoctor.User.PhoneNumber = updateDTO.PhoneNumber;
                existingDoctor.Specialized = updateDTO.Specialized;
                existingDoctor.Status = updateDTO.DoctorStatus;
                existingDoctor.User.Birthday = updateDTO.BirthDate;
                existingDoctor.User.Email = updateDTO.Email;
                existingDoctor.DegreeId = updateDTO.DegreeId;


                //existingDoctor.ClinicId = updateDTO.ClinicId;

                var doctor = _mapper.Map<Doctor>(existingDoctor);
                _context.Doctors.Update(doctor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in UpdateClinic: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateClinic: {ex.Message}");
                throw;
            }
        }

        //Medicine
        public async Task CreateMedicineAsync(string userId, MedicineManaDTO medicineDTO)
        {
            var emp = _context.Employees.FirstOrDefault(e => e.UserId == userId);   
            var medicine = new Medicine
            {
                MedicineId = Guid.NewGuid().ToString(),
                MedicineName = medicineDTO.MedicineName,
                MedicineUnit = medicineDTO.MedicineUnit,
                Prices = medicineDTO.Prices,
                Inventory = medicineDTO.Inventory,
                Specifications = medicineDTO.Specifications,
                MedicineCateId = medicineDTO.MedicineCateId,
                ClinicId = emp.ClinicId 
            };

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();
        }

        public async Task<MedicineListDTO> GetAllMedicineAsync(string userId, int limit, int offset)
        {
            try
            {
                var emp = _context.Employees.FirstOrDefault(e => e.UserId == userId);
                var clinicMedicinesQuery = _context.Medicines
                    .Where(m => m.ClinicId == emp.ClinicId) 
                    .Include(m => m.MedicineCategory)
                    .OrderBy(m => m.MedicineId);

                var totalMedicines = await clinicMedicinesQuery.CountAsync();

                var clinicMedicines = await clinicMedicinesQuery.Skip(offset).Take(limit).ToListAsync();

                var medicineDTOs = clinicMedicines.Select(medicine => new MedicineManaDTO
                {
                    MedicineId = medicine.MedicineId,
                    MedicineName = medicine.MedicineName,
                    MedicineUnit = medicine.MedicineUnit,
                    Prices = medicine.Prices,
                    Inventory = medicine.Inventory,
                    Specifications = medicine.Specifications,
                    MedicineCateId = medicine.MedicineCateId,
                    MedicineCateName = medicine.MedicineCategory.CategoryName
                });

                return new MedicineListDTO
                {
                    TotalMedicine = totalMedicines,
                    Medicines = medicineDTOs
                };
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in GetAllMedicineAsync: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllMedicineAsync: {ex.Message}");
                throw;
            }
        }
        public async Task<MedicineManaDTO> GetMedicineByIdAsync(string userId, string medicineId)
        {
            var medicine = await _context.Medicines.FindAsync(medicineId);
            var emp = _context.Employees.FirstOrDefault(e => e.UserId == userId);

            if (medicine == null || medicine.ClinicId != emp.ClinicId)
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

        public async Task UpdateMedicineAsync(MedicineManaDTO medicineDTO)
        {
            var medicine = await _context.Medicines.FindAsync(medicineDTO.MedicineId);

            if (medicine == null )
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

        //Category
        public async Task CreateCategoryAsync(CateManaDTO dto)
        {
            var generateID = new GenerateID(_context);
            
            var cate = new MedicineCategory
            {
                MedicineCateId = generateID.GenerateNewId("MC"),
                CategoryName = dto.CategoryName
            };

            _context.MedicineCategories.Add(cate);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CateManaDTO>> GetAllCateAsync()
        {
            try
            {
                var cateQuery = _context.MedicineCategories.OrderBy(m => m.MedicineCateId);

                var totalCates = await cateQuery.CountAsync();

                var cate = await cateQuery.ToListAsync();

                var cateDTOs = cate.Select(cate => new CateManaDTO
                {
                    MedicineCateId = cate.MedicineCateId,
                    CategoryName = cate.CategoryName,
                });

                return cateDTOs;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in GetAllCateAsync: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllCateAsync: {ex.Message}");
                throw;
            }
        }



        public async Task<CateManaDTO> GetCateByIdAsync(string id)
        {
            var cate = await _context.MedicineCategories.FindAsync(id);

            if (cate == null)
            {
                return null;
            }
            
            return new CateManaDTO
            {
                MedicineCateId = cate.MedicineCateId,
                CategoryName = cate.CategoryName,
            };
        }

        public async Task UpdateCateAsync(CateManaDTO dto)
        {
            var cate = await _context.MedicineCategories.FindAsync(dto.MedicineCateId);

            if (cate == null)
            {
                return;
            }

            cate.CategoryName = dto.CategoryName;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCateAsync(string id)
        {
            var cate = await _context.MedicineCategories.FindAsync(id);

            if (cate == null)
            {
                return;
            }

            _context.MedicineCategories.Remove(cate);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CateManaDTO>> SearchByName(string name)
        {
            try
            {
                var cateQuery = _context.MedicineCategories
                                    .Where(c => c.CategoryName.Contains(name))
                                    .OrderBy(c => c.MedicineCateId);

                var totalCates = await cateQuery.CountAsync();

                var cateList = await cateQuery.ToListAsync();

                var cateDTOs = cateList.Select(cate => new CateManaDTO
                {
                    MedicineCateId = cate.MedicineCateId,
                    CategoryName = cate.CategoryName,
                });

                return cateDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchByName: {ex.Message}");
                throw;
            }
        }

        //
        public async Task<IEnumerable<DoctorDegree>> GetAllDegreeAsync()
        {
            try
            {
                var cateQuery = _context.DoctorDegrees.OrderBy(m => m.DegreeId);

                var totalCates = await cateQuery.CountAsync();

                var cate = await cateQuery.ToListAsync();

                var degreeDTOs = cate.Select(cate => new DoctorDegree
                {
                    DegreeId = cate.DegreeId,
                    DegreeName = cate.DegreeName,
                });

                return degreeDTOs;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in GetAll degree Async: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAll Degree Async: {ex.Message}");
                throw;
            }
        }

        public async Task CreateDegreeAsync(DoctorDegree dto)
        {
            var generateID = new GenerateID(_context);

            var degree = new DoctorDegree
            {
                DegreeId = generateID.GenerateNewDoctorId("DD"),
                DegreeName = dto.DegreeName,
            };

            _context.DoctorDegrees.Add(degree);
            await _context.SaveChangesAsync();
        }

        //public async Task CreatePetCateAsync(PetTypeManaDTO dto)
        //{
        //    try
        //    {
        //        foreach (var petCate in dto.PetTypeList)
        //        {
                    
        //            var existingPetTypePerClinic = await _context.PetTypePerClinics
        //                .FirstOrDefaultAsync(d => d.ClinicId == dto.ClinicId && d.PetTypeId == petCate.PetTypeId);

        //            if (existingPetTypePerClinic == null)
        //            {
        //                var newPetTypePerClinic = new PetTypePerClinic
        //                {
        //                    ClinicPetTypeId = Guid.NewGuid().ToString(),  
        //                    PetTypeId = petCate.PetTypeId,
        //                };

        //                _context.PetTypePerClinics.Add(newPetTypePerClinic);
        //            }
        //        }

        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        Console.WriteLine($"Error:  CreatePetTypePerClinicsAsync: {ex.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error:  CreatePetTypePerClinicsAsync: {ex.Message}");
        //        throw;
        //    }
        //}


        public async Task<PetTypeManaDTO> GetAllPetCateAsync(string clinicId)
        {
            try
            {
                var cateQuery = _context.PetTypePerClinics
                    .Where(d => d.ClinicId == clinicId)
                    .Include(d => d.PetType)
                    .OrderBy(m => m.ClinicPetTypeId);

                var cate = await cateQuery.ToListAsync();

                var cateDTOs = cate.Select(c => new PetCateManaDTO
                {
                    PetTypeId = c.PetTypeId,
                    PetTypeName = c.PetType.PetTypeName
                }).ToList();

                var petTypeManaDTO = new PetTypeManaDTO
                {
                    PetTypeList = cateDTOs
                };

                return petTypeManaDTO;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in GetAllPetCateAsync: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllPetCateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<PetTypeManaDTO> GetPetsCateAsync(string userId)
        {
            try
            {
                var emp = _context.Employees.FirstOrDefault(e => e.UserId == userId);
                var cateQuery = _context.PetTypePerClinics
                    .Where(d => d.ClinicId == emp.ClinicId)
                    .Include(d => d.PetType)
                    .OrderBy(m => m.ClinicPetTypeId);

                var cate = await cateQuery.ToListAsync();

                var cateDTOs = cate.Select(c => new PetCateManaDTO
                {
                    PetTypeId = c.PetTypeId,
                    PetTypeName = c.PetType.PetTypeName
                }).ToList();

                var petTypeManaDTO = new PetTypeManaDTO
                {
                    PetTypeList = cateDTOs
                };

                return petTypeManaDTO;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in GetAllPetCateAsync: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllPetCateAsync: {ex.Message}");
                throw;
            }
        }



        public async Task<PetCateManaDTO> GetCateByIdAsync(string clinicId, string id)
        {
            if (string.IsNullOrEmpty(clinicId) || string.IsNullOrEmpty(id))
            {
                return null;
            }

            var cate = await _context.PetTypePerClinics
                .Include(d => d.PetType)
                .Include(d => d.Clinic)
                .FirstOrDefaultAsync(d => d.ClinicId == clinicId && d.PetTypeId == id);

            if (cate == null)
            {
                return null;
            }

            return new PetCateManaDTO
            {
                
                PetTypeId = cate.PetTypeId,
                PetTypeName = cate.PetType.PetTypeName
            };
        }


        public async Task UpdatePetCateAsync(string userId, List<PetCateManaDTO> PetTypeList)
        {
            try
            {
                var emp = _context.Employees.FirstOrDefault(e => e.UserId == userId);

                foreach (var petCate in PetTypeList)
                {
                    var existingPetType = await _context.PetTypePerClinics
                        .Include(d => d.PetType)
                        .Include(d => d.Clinic)
                        .FirstOrDefaultAsync(d => d.ClinicId == emp.ClinicId && d.PetTypeId == petCate.PetTypeId);

                    if (existingPetType == null)
                    {
                        var newPetType = new PetTypePerClinic
                        {
                            ClinicPetTypeId = Guid.NewGuid().ToString(),
                            ClinicId = emp.ClinicId,
                            PetTypeId = petCate.PetTypeId
                        };
                        _context.PetTypePerClinics.Add(newPetType);
                    }
                    else
                    {
                        existingPetType.PetType.PetTypeName = petCate.PetTypeName;
                        _context.Entry(existingPetType).State = EntityState.Modified;
                    }
                }

                var existingPetTypes = await _context.PetTypePerClinics
                    .Where(d => d.ClinicId == emp.ClinicId)
                    .Select(d => d.PetTypeId)
                    .ToListAsync();

                foreach (var existingPetTypeId in existingPetTypes)
                {
                    if (!PetTypeList.Any(p => p.PetTypeId == existingPetTypeId))
                    {
                        var petTypeToDelete = await _context.PetTypePerClinics
                            .FirstOrDefaultAsync(d => d.ClinicId == emp.ClinicId && d.PetTypeId == existingPetTypeId);

                        _context.PetTypePerClinics.Remove(petTypeToDelete);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error: UpdatePetTypePerClinicsAsync: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: UpdatePetTypePerClinicsAsync: {ex.Message}");
                throw;
            }
        }

        // 
        public async Task<MedicineReponse> GenerateMedicineSalesReport(DateTime startDate, DateTime endDate, string userId)
        {
            try
            {
                var emp = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);
                if (emp == null)
                {
                    throw new ArgumentException("ClinicId is required.");
                }

                var prescriptions = _context.Prescriptions
                .Where(p => p.ExaminationDay >= startDate && p.ExaminationDay <= endDate && p.Appointment.ClinicId == emp.ClinicId && p.Appointment.Status == "done")
                .Include(p => p.PrescriptionMedicines)
                .ThenInclude(pm => pm.Medicine);

                var medicineReport = new List<MedicineReport>();

                double duePrice = 0;

                foreach (var prescription in prescriptions)
                {
                    foreach (var medicine in prescription.PrescriptionMedicines)
                    {
                        var existingMedicine = medicineReport.FirstOrDefault(m => m.MedicineName == medicine.Medicine.MedicineName);

                        if (existingMedicine != null)
                        {
                            existingMedicine.Quantity += medicine.Quantity;
                            existingMedicine.TotalPrice += medicine.Quantity * medicine.Medicine.Prices;
                        }
                        else
                        {
                            medicineReport.Add(new MedicineReport
                            {
                                MedicineName = medicine.Medicine.MedicineName,
                                Unit = medicine.Medicine.MedicineUnit,
                                Quantity = medicine.Quantity,
                                Price = medicine.Medicine.Prices,
                                TotalPrice = (medicine.Quantity * medicine.Medicine.Prices)
                            });
                        }

                        duePrice += medicine.Quantity * medicine.Medicine.Prices;
                    }
                }

                var responseDTO = new MedicineReponse
                {
                    DuePrice = duePrice,
                    MedicineReport = medicineReport
                };

                return responseDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating medicine sales report.", ex);
            }
        }


    }
}



