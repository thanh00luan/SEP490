using AutoMapper;
using BussinessObject.Data;
using BussinessObject.Models;
using DataAccess.DTO.Appointment;
using DataAccess.DTO.DDoctor;
using DataAccess.DTO.Employee;
using DataAccess.RequestDTO;
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

        public async Task<GetALLDTOCount> GetPendingAppointment(DateTime startDate, DateTime endDate, int limit, int offset)
        {
            try
            {
                var appointments = await _context.Appointments
                        .Include(a => a.Pet)
                        .Include(a => a.Clinic)
                        .Include(a => a.User)
                        .Where(a => a.Status == "pending" && a.AppointmentDate.Date >= startDate.Date && a.AppointmentDate.Date <= endDate.Date)
                        .OrderByDescending(a => a.AppointmentDate)
                        .Skip(offset)
                        .Take(limit)
                        .ToListAsync();

                var appointmentDTOs = _mapper.Map<List<GetAllDTO>>(appointments);

                var appointmentDTOWithCount = new GetALLDTOCount();
                appointmentDTOWithCount.AppointmentDTOs = appointmentDTOs;
                appointmentDTOWithCount.Total = await _context.Appointments
                    .Where(a => a.Status == "pending" && a.AppointmentDate.Date >= startDate.Date && a.AppointmentDate.Date <= endDate.Date)
                    .CountAsync();

                return appointmentDTOWithCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPendingAppointmentsByDate: {ex.Message}");
                throw;
            }
        }



        public List<SetDoctorRequest> GetDoctorAvailability(string doctorId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var doctorSlots = _context.DoctorSlots
                    .Where(ds => ds.DoctorId == doctorId && ds.RegisterDate.Date >= startDate.Date && ds.RegisterDate.Date <= endDate.Date)
                    .ToList();

                if (doctorSlots.Any())
                {
                    List<SetDoctorRequest> availabilityList = new List<SetDoctorRequest>();

                    foreach (var slot in doctorSlots)
                    {
                        SetDoctorRequest availability = new SetDoctorRequest
                        {
                            DoctorId = doctorId,
                            availabilitySlots = new List<int>(),
                            RegisterDate = slot.RegisterDate
                        };

                        if (slot.Slots != null)
                        {
                            availability.availabilitySlots.AddRange(slot.Slots.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
                        }

                        availabilityList.Add(availability);
                    }

                    return availabilityList;
                }
                else
                {
                    return new List<SetDoctorRequest>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDoctorAvailability: {ex.Message}");
                throw;
            }
        }

        public async Task<List<AvaibleDoctorDTO>> GetAvailableDoctors(string appointmentId)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Clinic)
                    .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

                if (appointment == null)
                {
                    return new List<AvaibleDoctorDTO>();
                }
                var doctors = _context.Doctors.Include(d => d.User).Include(d=>d.Clinic).Where(d => d.ClinicId == appointment.ClinicId).ToList();
                var availableDoctors = new List<AvaibleDoctorDTO>();

                foreach (var doctor in doctors)
                {
                    var doctorAvailability = GetDoctorAvailability(doctor.DoctorId, appointment.AppointmentDate, appointment.AppointmentDate);


                    foreach (var availabilitySlot in doctorAvailability)
                    {
                        if (availabilitySlot.availabilitySlots.Contains(appointment.SlotNumber))
                        {
                            var isBooked = await _context.Appointments
                                .AnyAsync(a => a.DoctorId == doctor.DoctorId &&
                                               a.AppointmentDate == availabilitySlot.RegisterDate &&
                                               a.SlotNumber == appointment.SlotNumber &&
                                               a.AppointmentId != appointmentId);

                            if (!isBooked)
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

                var doctorSlot = await _context.DoctorSlots.FirstOrDefaultAsync(ds => ds.DoctorId == doctorId && ds.RegisterDate.Date == appointment.AppointmentDate.Date);
                if (doctorSlot == null)
                {
                    throw new ArgumentException("Doctor slot not found for the given date.");
                }

                var availabilitySlots = doctorSlot.Slots.Split(',').Select(int.Parse).ToList();

                if (!availabilitySlots.Contains(appointment.SlotNumber))
                {
                    throw new InvalidOperationException("Selected slot is not available for the doctor.");
                }

                availabilitySlots.Remove(appointment.SlotNumber);

                doctorSlot.Slots = string.Join(",", availabilitySlots);

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
