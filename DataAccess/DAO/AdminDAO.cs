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
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class AdminDAO
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly SendMailService _sendMailService;


        public AdminDAO(ApplicationDbContext context, IMapper mapper, SendMailService sendMail) 
        {
            _context = context;
            _mapper = mapper;   
            _sendMailService = sendMail;
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

        public async Task<int> AppointmentStatisticAsync(int? day, int? month, string appointmentId, string status, string clinicId)
        {
            IQueryable<Appointment> query = _context.Appointments;

            if (day.HasValue && day > 0)
            {
                DateTime startDate = DateTime.Now.AddDays(-day.Value + 1).Date;

                query = query.Where(a => a.AppointmentDate.Date >= startDate && a.AppointmentDate.Date <= DateTime.Now);
            }
            else if (month.HasValue && month > 0)
            {
                DateTime startDate = DateTime.Now.AddMonths(-month.Value).AddDays(1 - DateTime.Now.Day).Date;
                DateTime endDate = DateTime.Now.AddDays(-DateTime.Now.Day).Date; 

                query = query.Where(a => a.AppointmentDate.Date >= startDate && a.AppointmentDate.Date <= endDate);
            }

            if (!string.IsNullOrEmpty(clinicId))
            {
                query = query.Where(a => a.ClinicId == clinicId);
            }

            if (!string.IsNullOrEmpty(appointmentId))
            {
                query = query.Where(a => a.AppointmentId == appointmentId);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(a => a.Status == status);
            }

            return await query.CountAsync();
        }

        public async Task<List<AppointmentStatisticReponse>> appointmentStatistics(DateTime start, DateTime end, string clinicId)
        {
            IQueryable<Appointment> query = _context.Appointments;

            query = query.Where(a => a.AppointmentDate.Date >= start.Date && a.AppointmentDate.Date <= end.Date);

            if (!string.IsNullOrEmpty(clinicId))
            {
                query = query.Where(a => a.ClinicId == clinicId);
            }

            List<AppointmentStatisticReponse> dailyStatistics = new List<AppointmentStatisticReponse>();

            for (DateTime date = start.Date; date <= end.Date; date = date.AddDays(1))
            {
                int totalAppointments = await query.CountAsync(a => a.AppointmentDate.Date == date);
                int doneQuantity = await query.CountAsync(a => a.AppointmentDate.Date == date && a.Status == "done");
                int inProgressQuantity = await query.CountAsync(a => a.AppointmentDate.Date == date && a.Status == "inProgress");
                int waitingQuantity = await query.CountAsync(a => a.AppointmentDate.Date == date && a.Status == "waiting");
                int pendingQuantity = await query.CountAsync(a => a.AppointmentDate.Date == date && a.Status == "pending");

                if (totalAppointments == 0)
                {
                    dailyStatistics.Add(new AppointmentStatisticReponse
                    {
                        Date = date,
                        total = 0,
                        doneQuantity = 0,
                        inProgressQuantity = 0,
                        watingQuantity = 0,
                        pendingQuantity = 0
                    });
                }
                else
                {
                    AppointmentStatisticReponse dailyStatistic = new AppointmentStatisticReponse
                    {
                        Date = date,
                        total = totalAppointments,
                        doneQuantity = doneQuantity,
                        inProgressQuantity = inProgressQuantity,
                        watingQuantity = waitingQuantity,
                        pendingQuantity = pendingQuantity
                    };

                    dailyStatistics.Add(dailyStatistic);
                }
            }

            return dailyStatistics;
        }


        public async Task<double> moneyStatistic(DateTime start, DateTime end, string clinicId)
        {
            double totalRevenue = 0;

            var prescriptions = _context.Prescriptions
                .Where(p => p.ExaminationDay >= start && p.ExaminationDay <= end && p.Appointment.ClinicId == clinicId && p.Appointment.Status == "done")
                .Include(p => p.PrescriptionMedicines)
                .ThenInclude(pm => pm.Medicine);

            foreach (var prescription in prescriptions)
            {
                foreach (var prescriptionMedicine in prescription.PrescriptionMedicines)
                {
                    double medicineTotalCost = prescriptionMedicine.Quantity * prescriptionMedicine.Medicine.Prices;
                    totalRevenue += medicineTotalCost;
                }
            }

            return totalRevenue;
        }
        public async Task<MedicineReponse> GenerateMedicineSalesReport(DateTime startDate, DateTime endDate, string clinicId)
        {
            try
            {
                var allClinics = string.IsNullOrEmpty(clinicId);

                var prescriptions = _context.Prescriptions
                    .Where(p => p.ExaminationDay >= startDate && p.ExaminationDay <= endDate && (allClinics || p.Appointment.ClinicId == clinicId) && p.Appointment.Status == "done")
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





        internal async Task<int> countCustomer(string clinicId)
        {
            int customerCount = await _context.Appointments
                .Where(a => a.ClinicId == clinicId)
                .Select(a => a.UserId)  
                .Distinct()  
                .CountAsync();  

            return customerCount;
        }

        //Clinic Manager
        public async Task CreateClinic( ClinicManaDTO dto)
        {
            var clinic = new Clinic
            {
                ClinicId = Guid.NewGuid().ToString(),
                ClinicName = dto.ClinicName,
                Address = dto.Address,
                ClinicPhoneNumber = dto.ClinicPhoneNumber,  
                Email = dto.Staff.Email,
                Latitude = dto.Latitude,    
                Longitude = dto.Longitude,
            };
            try
            {

                if (!string.IsNullOrEmpty(dto.Staff.UserId))
                {
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.Staff.UserId);

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
                        FullName = dto.Staff.EmployeeName,
                        Email = dto.Staff.Email,
                        PhoneNumber = dto.Staff.PhoneNumber,
                        Address = dto.Staff.Address,
                        Birthday = dto.Staff.Birthday,
                        UserRole = 2,
                    };

                    var password = "12345678";

                    var hashedPassword = HashPassword(password);

                    user.Password = hashedPassword;
                    user.UserName = user.Email;
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    var newEmployee = new Employee
                    {
                        EmployeeId = user.UserId,
                        UserId = user.UserId,
                        EmployeeStatus = dto.Staff.EmployeeStatus,
                    };

                    var email = user.Email;
                    var subject = "Thông tin tài khoản của bạn";
                    var body = $"Xin chào {user.FullName},\n\nTài khoản của bạn đã được tạo thành công.\n\nTên đăng nhập: {user.UserName}\nMật khẩu: {password}\n\nXin cảm ơn.";

                    await _sendMailService.SendEmailAsync(email, subject, body);

                    clinic.EmployeeId = user.UserId;
                    clinic.Employee = newEmployee;

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



            _context.Clinics.Add(clinic);
            await _context.SaveChangesAsync();
        }

        public async Task<ClinicListDTO> GetAllClinic(int limit, int offset)
        {
            try
            {
                var clinicQuery = _context.Clinics.Include(c=>c.Employee).Include(c => c.Employee.User).OrderBy(d => d.ClinicName);

                var totals = await clinicQuery.CountAsync();

                var clinics = await clinicQuery.Skip(offset).Take(limit).ToListAsync();

                

                var clinicDTOs = clinics.Select(e => new ClinicManaDTO
                {
                    ClinicId = e.ClinicId,
                    ClinicName = e.ClinicName,
                    Address = e.Address,
                    ClinicPhoneNumber = e.ClinicPhoneNumber,

                    Latitude = e.Latitude,
                    Longitude = e.Longitude,
                    Staff = new StaffManaDTO
                    {
                        EmployeeName = e.Employee.User.FullName,
                        EmployeeId = e.EmployeeId,
                        Email = e.Email,
                        PhoneNumber = e.Employee.User?.PhoneNumber, 
                        Address = e.Employee.User?.Address,
                        Birthday = e.Employee.User.Birthday,
                        EmployeeStatus = e.Employee.EmployeeStatus
                    }

            }) ;
                return new ClinicListDTO
                {
                    TotalClinic = totals,
                    Clinics = clinicDTOs
                };
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in GetAllClinic: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllClinic: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteClinic(string id)
        {
            try
            {
                var clinic = await _context.Clinics.FindAsync(id);
                if (clinic != null)
                {
                    _context.Clinics.Remove(clinic);
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

        public async Task<ClinicManaDTO> GetClinicByID(string id)
        {
            try
            {
                var e = await _context.Clinics
                    .Include(c=>c.Employee)
                    .Include(c=>c.Employee.User)
                    .FirstOrDefaultAsync(d => d.ClinicId == id);

                if (e == null)
                {

                    return null;
                }

                var dto = new ClinicManaDTO
                {
                    ClinicId = e.ClinicId,
                    ClinicName = e.ClinicName,
                    Address = e.Address,
                    ClinicPhoneNumber = e.ClinicPhoneNumber,
                    Latitude = e.Latitude,
                    Longitude = e.Longitude,
                    Staff = new StaffManaDTO
                    {
                        EmployeeName = e.Employee.User.FullName,
                        EmployeeId = e.EmployeeId,
                        Email = e.Email,
                        PhoneNumber = e.Employee.User?.PhoneNumber,
                        Address = e.Employee.User?.Address,
                        Birthday = e.Employee.User.Birthday,
                        EmployeeStatus = e.Employee.EmployeeStatus
                    }
                };

                return dto;
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in get clinic by id: {ex.Message}");
                throw;
            }
        }

       


        public async Task<IEnumerable<ClinicManaDTO>> SortClinicByName()
        {
            try
            {
                var sortedClinic = await _context.Clinics
                    .Include(c => c.Employee)
                    .Include(c => c.Employee.User)
                    .OrderBy(c => c.ClinicName).ToListAsync();
                var sortedDTO = _mapper.Map<IEnumerable<ClinicManaDTO>>(sortedClinic);
                return sortedDTO;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Error in SortClinicByName: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SortClinicByName: {ex.Message}");
                throw;
            }
        }



        public async Task UpdateClinic(ClinicManaDTO updateDTO)
        {

            try
            {

                var exClinic = await _context.Clinics.Include(c => c.Employee).Include(c => c.Employee.User)
                    .FirstOrDefaultAsync(d => d.ClinicId == updateDTO.ClinicId);
                if (exClinic == null)
                {
                    throw new Exception("Error in get clinic.");
                }
                exClinic.ClinicId = updateDTO.ClinicId;
                exClinic.ClinicName = updateDTO.ClinicName;
                exClinic.ClinicPhoneNumber = updateDTO.ClinicPhoneNumber;   
                exClinic.Address = updateDTO.Address;
                exClinic.Latitude = updateDTO.Latitude;
                exClinic.Longitude = updateDTO.Longitude;
                _context.Clinics.Update(exClinic);
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

        //PetCategory
        public async Task CreateCategoryAsync(PetCateManaDTO dto)
        {
            var generateID = new GenerateID(_context);

            var cate = new PetType
            {
                PetTypeId = generateID.GenerateNewPetId("PT"),
                PetTypeName = dto.PetTypeName
            };

            _context.PetTypes.Add(cate);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PetCateManaDTO>> GetAllCateAsync()
        {
            try
            {
                var cateQuery = _context.PetTypes.OrderBy(m => m.PetTypeId);

                var totalCates = await cateQuery.CountAsync();

                var cate = await cateQuery.ToListAsync();

                var cateDTOs = cate.Select(cate => new PetCateManaDTO
                {
                    PetTypeId = cate.PetTypeId,
                    PetTypeName = cate.PetTypeName,
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



        public async Task<PetCateManaDTO> GetCateByIdAsync(string id)
        {
            var cate = await _context.PetTypes.FindAsync(id);

            if (cate == null)
            {
                return null;
            }

            return new PetCateManaDTO
            {
                PetTypeId = cate.PetTypeId,
                PetTypeName = cate.PetTypeName,
            };
        }

        public async Task UpdateCateAsync(PetCateManaDTO dto)
        {
            var cate = await _context.PetTypes.FindAsync(dto.PetTypeId);

            if (cate == null)
            {
                return;
            }

            cate.PetTypeName = dto.PetTypeName;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCateAsync(string id)
        {
            var cate = await _context.PetTypes.FindAsync(id);

            if (cate == null)
            {
                return;
            }

            _context.PetTypes.Remove(cate);
            await _context.SaveChangesAsync();
        }

        //Doctor
        public async Task<DoctorListDTO> GetAllDoctors(string clinicId, int limit, int offset)
        {
            try
            {
                IQueryable<Doctor> doctorsQuery = _context.Doctors
                    .Include(d => d.User)
                    .Include(d => d.DoctorDegree)
                    .OrderBy(d => d.DoctorId);

                if (clinicId != null)
                {
                    doctorsQuery = doctorsQuery.Where(d => d.ClinicId == clinicId);
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


        //Staff
        public async Task<StaffListDTO> GetAllStaff(string clinicId, int limit, int offset)
        {
            try
            {
                IQueryable<Employee> staffQuery = _context.Employees
                    .Include(e => e.Clinic)
                    .Include(e => e.User)
                    .OrderBy(e => e.UserId);

                if (clinicId != null)
                {
                    staffQuery = staffQuery.Where(e => e.ClinicId == clinicId);
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
                    ClinicId = e.ClinicId,
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


        //public async Task<IEnumerable<CateManaDTO>> SearchByName(string name)
        //{
        //    try
        //    {
        //        var cateQuery = _context.MedicineCategories
        //                            .Where(c => c.CategoryName.Contains(name))
        //                            .OrderBy(c => c.MedicineCateId);

        //        var totalCates = await cateQuery.CountAsync();

        //        var cateList = await cateQuery.ToListAsync();

        //        var cateDTOs = cateList.Select(cate => new CateManaDTO
        //        {
        //            MedicineCateId = cate.MedicineCateId,
        //            CategoryName = cate.CategoryName,
        //        });

        //        return cateDTOs;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in SearchByName: {ex.Message}");
        //        throw;
        //    }
        //}
    }
}
