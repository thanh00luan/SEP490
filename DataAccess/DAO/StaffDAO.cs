using AutoMapper;
using BussinessObject.Data;
using BussinessObject.Models;
using DataAccess.DTO.Appointment;
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
    public class StaffDAO
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public StaffDAO(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddStaff(StaffDTO staffDTO)
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
                        throw new Exception("User does not exist.");
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




        public async Task<IEnumerable<StaffDTO>> GetAllStaff()
        {
            try
            {
                
                var employees = await _context.Employees
                    .Include(e => e.User) 
                    .ToListAsync();

                var staffDTOs = employees.Select(e => new StaffDTO
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

        public async Task UpdateStaff(StaffDTO staffDTO)
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




        public async Task<StaffDTO> GetStaffById(string id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                return _mapper.Map<StaffDTO>(employee);
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

        public async Task<GetALLDTOCount> GetPendingAppointment(DateTime appointmentDate, int limit, int offset)
        {
            try
            {
                var appointments = await _context.Appointments
                        .Include(a => a.Pet)
                        .Include(a => a.Clinic)
                        .Include(a => a.User)
                        .Where(a => a.Status == "pending" && a.AppointmentDate.Date == appointmentDate.Date)
                        .OrderByDescending(a => a.AppointmentDate)
                        .Skip(offset)
                        .Take(limit)
                        .ToListAsync();

                var appointmentDTOs = _mapper.Map<List<GetAllDTO>>(appointments);

                var appointmentDTOWithCount = new GetALLDTOCount();
                appointmentDTOWithCount.AppointmentDTOs = appointmentDTOs;
                appointmentDTOWithCount.Total = await _context.Appointments
                    .Where(a => a.Status == "pending" && a.AppointmentDate.Date == appointmentDate.Date)
                    .CountAsync();

                return appointmentDTOWithCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPendingAppointmentsByDate: {ex.Message}");
                throw;
            }
        }


        public List<int> GetDoctorAvailability(string doctorId)
        {
            try
            {
                var doctor = _context.Doctors.Find(doctorId);
                if (doctor != null && !string.IsNullOrEmpty(doctor.Slots))
                {
                    List<int> availabilitySlots = doctor.Slots.Split(',').Select(int.Parse).ToList();
                    return availabilitySlots;
                }
                return new List<int>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDoctorAvailability: {ex.Message}");
                throw;
            }
        }

        public async Task<List<AvaibleDoctorDTO>> GetAvailableDoctors(int customerSlot, string clinicId)
        {
            try
            {
                var doctors = await _context.Doctors
            .Include(d => d.User) 
            .Include(d => d.Clinic) 
            .Where(d => d.ClinicId == clinicId)
            .ToListAsync();
                var availableDoctors = new List<AvaibleDoctorDTO>();
                foreach (var doctor in doctors)
                {
                    var availabilitySlots = GetDoctorAvailability(doctor.DoctorId);
                    if (availabilitySlots.Contains(customerSlot))
                    {
                        
                        var doctorDTO = new AvaibleDoctorDTO
                        {
                            
                            DoctorName = doctor.User.FullName, 
                            DoctorStatus = doctor.Status,
                            ClinicName = doctor.Clinic.ClinicName,
                            Address = doctor.User.Address,
                            PhoneNumber = doctor.User.PhoneNumber,
                            Specialized = doctor.Specialized,
                        };

                        availableDoctors.Add(doctorDTO);
                    }
                }

                return availableDoctors;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAvailableDoctors: {ex.Message}");
                throw;
            }
        }

        public async Task AssignDoctorToAppointment(string appointmentId, string doctorId)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(appointmentId);

                
                if (appointment == null)
                {
                    throw new ArgumentException("Appointment not found.");
                }

                
                var doctor = await _context.Doctors.FindAsync(doctorId);
                if (doctor == null)
                {
                    throw new ArgumentException("Doctor not found.");
                }
                List<int> availabilitySlots = doctor.Slots.Split(',').Select(int.Parse).ToList();

                availabilitySlots.Remove(appointment.SlotNumber);

                doctor.Slots = string.Join(",", availabilitySlots);

                appointment.DoctorId = doctorId;
                appointment.Doctor = doctor;
                appointment.Status = "waiting";
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning doctor to appointment: {ex.Message}");
                throw;
            }
        }






    }
}
