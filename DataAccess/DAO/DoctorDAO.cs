using AutoMapper;
using BussinessObject.Data;
using DataAccess.DTO.Appointment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class DoctorDAO
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public DoctorDAO(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void SetDoctorAvailability(string doctorId, List<int> availabilitySlots)
        {
            try
            {
                var doctor = _context.Doctors.Find(doctorId);
                doctor.Slots = string.Join(",", availabilitySlots); 
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SetDoctorAvailability: {ex.Message}");
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

        public async Task<GetALLDTOCount> GetDoctorAppointList(int limit, int offset, string doctorId)
        {
            try
            {
                var currentTime = DateTime.UtcNow;

                var appointments = await _context.Appointments
            .Include(a => a.Pet)
            .Include(a => a.Clinic)
            .Include(a => a.User)
            .Include(a => a.Doctor)
            .Where(a => (a.Status == "waiting" || a.Status == "inProgress" || a.Status == "done") && a.DoctorId == doctorId)
            .OrderBy(a => a.Status == "waiting" ? (a.AppointmentDate <= currentTime ? 0 : 1) :
                          a.Status == "inProgress" ? (a.AppointmentDate <= currentTime ? 2 : 3) : 4)
            .ThenBy(a => a.AppointmentDate)
            .ThenBy(a => a.Status == "done" ? DateTime.MaxValue : a.AppointmentDate) 
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

                var appointmentDTOs = _mapper.Map<List<GetAllDTO>>(appointments);

                var appointmentDTOWithCount = new GetALLDTOCount();
                appointmentDTOWithCount.AppointmentDTOs = appointmentDTOs;
                appointmentDTOWithCount.Total = await _context.Appointments
                    .Where(a => (a.Status == "waiting" || a.Status == "inProgress" || a.Status == "done") && a.DoctorId == doctorId) 
                    .CountAsync();

                return appointmentDTOWithCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetWaitPenApp: {ex.Message}");
                throw;
            }
        }

        public void ConfirmAppointment(string doctorId, string appointmentId)
        {
            try
            {
                var appointment = _context.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId && a.DoctorId == doctorId && a.Status == "waiting");

                if (appointment != null)
                {
                    appointment.Status = "inProgress"; 
                    _context.SaveChanges();
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ConfirmAppointment: {ex.Message}");
                throw;
            }
        }


    }
}
