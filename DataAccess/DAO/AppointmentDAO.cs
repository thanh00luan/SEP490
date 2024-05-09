using AutoMapper;
using BussinessObject.Data;
using BussinessObject.Models;
using DataAccess.DTO.Appointment;
using DataAccess.DTO.Clinic;
using DataAccess.DTO.DPet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace DataAccess.DAO
{
    public class AppointmentDAO
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public AppointmentDAO(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorClinicDTO>> GetAllDoctorsInClinic(string clinicId)
        {
            try
            {
                var doctors = await _context.Doctors
                    .Where(d => d.ClinicId == clinicId)
                    .ToListAsync();

                var doctorDTOs = _mapper.Map<IEnumerable<DoctorClinicDTO>>(doctors);

                return doctorDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllDoctorsInClinic: {ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<ClinicDTO>> GetAllClinic()
        {
            try
            {
                var clinics = await _context.Clinics.ToListAsync();

                var clinicDTOs = _mapper.Map<IEnumerable<ClinicDTO>>(clinics);

                return clinicDTOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllDoctorsInClinic: {ex.Message}");
                throw;
            }
        }
        public async Task<GetALLDTOCount> GetAll(string userId, int limit, int offset)
        {
            try
            {
                var currentTime = DateTime.Now;
                var slots = new List<SlotDTO>();

                TimeSpan slotDuration = TimeSpan.FromHours(1);
                TimeSpan startTime = TimeSpan.FromHours(8);

                for (int i = 0; i <= 8; i++)
                {
                    var slot = new SlotDTO
                    {
                        SlotNumber = i,
                        StartTime = currentTime.Date.Add(startTime),
                        EndTime = currentTime.Date.Add(startTime + slotDuration)
                    };

                    slots.Add(slot);
                    startTime = startTime.Add(slotDuration);
                }

                var sortedSlots = slots.OrderBy(s => s.StartTime).ToList();
                sortedSlots = sortedSlots.OrderBy(s => s.StartTime > currentTime ? 0 : 1).ToList();

                var appointments = await _context.Appointments
                    .Include(a => a.Pet)
                    .Include(a => a.Clinic)
                    .Include(a => a.User)
                    .Where(a => a.UserId == userId)
                    .OrderBy(a =>
                        a.Status == "inProgress" ? (a.AppointmentDate <= currentTime ? 0 : 1) :
                        a.Status == "waiting" ? (a.AppointmentDate <= currentTime ? 2 : 3) :
                        a.Status == "pending" ? (a.AppointmentDate <= currentTime ? 4 : 5) : 6)
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync();

              foreach (var appointment in appointments)
                {
                    if (appointment.Status != "pending")
                    {
                        _context.Entry(appointment).Reference(a => a.Doctor).Load();
                    }
                }

                var appointmentDTOs = new List<GetAllDTO>();
                foreach (var appointment in appointments)
                {
                    var appointmentDTO = _mapper.Map<GetAllDTO>(appointment);
                    var slot = slots.FirstOrDefault(s => s.SlotNumber == appointment.SlotNumber);
                    appointmentDTOs.Add(appointmentDTO);
                }

                var appointmentDTOWithCount = new GetALLDTOCount();
                appointmentDTOWithCount.AppointmentDTOs = appointmentDTOs;
                appointmentDTOWithCount.Total = await _context.Appointments.Where(a => a.UserId == userId).CountAsync();
                return appointmentDTOWithCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAll: {ex.Message}");
                throw;
            }
        }

        public void BookAppointment(DoctorClinicDTO appointment)
        {
            Appointment newAppointment = _mapper.Map<Appointment>(appointment);
            newAppointment.SlotNumber = appointment.Slot;
            newAppointment.AppointmentDate = appointment.Date;
            newAppointment.AppointmentId = Guid.NewGuid().ToString();
            newAppointment.Status = "pending";

            if (appointment.PetId == null)
            {
                Pet newPet = _mapper.Map<Pet>(appointment.PetDTO);
                newPet.PetId = Guid.NewGuid().ToString();
                newAppointment.PetId = newPet.PetId;
                newAppointment.Pet = newPet;
                newAppointment.Pet.UserId = appointment.UserId;
                _context.Pets.Add(newPet);
            }
            else
            {
                Pet existingPet = _context.Pets.Find(appointment.PetId);
                newAppointment.PetId = appointment.PetId;
                newAppointment.Pet = existingPet;
                newAppointment.Pet.UserId = appointment.UserId;
            }

            _context.Appointments.Add(newAppointment);
            _context.SaveChanges();
        }




        public async Task<ClinicSlotsResponse> GetAvailableSlots(string clinicId, DateTime date)
        {
            try
            {
                var availableSlots = await _context.Appointments
                    .Where(a => a.ClinicId == clinicId && a.AppointmentDate.Date == date.Date)
                    .Select(a => a.SlotNumber)
                    .ToListAsync();

                List<int> allSlots = Enumerable.Range(0, 9).ToList();

                foreach (var bookedSlot in availableSlots)
                {
                    allSlots.Remove(bookedSlot);
                }

                var response = new ClinicSlotsResponse
                {
                    ClinicId = clinicId,
                    Date = date.ToString("yyyy-MM-dd"),
                    Slots = allSlots
                };

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAvailableSlots: {ex.Message}");
                throw;
            }
        }



        public async Task<bool> SetDoctorAvailableSlots(string doctorId, string clinicId, DateTime date, List<int> availableSlots)
        {
            try
            {

                foreach (var slot in availableSlots)
                {
                    bool isSlotAvailable = await _context.Appointments
                        .AnyAsync(a => a.DoctorId == doctorId && a.ClinicId == clinicId && a.AppointmentDate.Date == date.Date && a.SlotNumber == slot && a.Status == "Available");

                    if (!isSlotAvailable)
                    {
                        Appointment newAppointment = new Appointment
                        {
                            AppointmentId = Guid.NewGuid().ToString(),
                            DoctorId = doctorId,
                            ClinicId = clinicId,
                            AppointmentDate = date,
                            SlotNumber = slot,
                            Status = "Available"
                        };

                        _context.Appointments.Add(newAppointment);
                    }
                }


                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SetDoctorAvailableSlots: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ClinicSlotsResponse>> GetAvailableSlotsInRange(string clinicId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var clinic = await _context.Clinics
                    .Include(c => c.Doctors)
                    .FirstOrDefaultAsync(c => c.ClinicId == clinicId);

                if (clinic == null)
                {
                    throw new Exception($"Clinic with ID {clinicId} not found.");
                }

                var doctorIds = clinic.Doctors.Select(d => d.DoctorId).ToList();

                var doctorSlots = await _context.DoctorSlots
                    .Where(ds => doctorIds.Contains(ds.DoctorId) && ds.RegisterDate >= startDate.Date && ds.RegisterDate <= endDate.Date)
                    .ToListAsync();

                var availableSlots = new List<ClinicSlotsResponse>();

                foreach (var doctorSlot in doctorSlots)
                {
                    if (!string.IsNullOrEmpty(doctorSlot.Slots)) 
                    {
                        var slots = doctorSlot.Slots.Split(',').Select(int.Parse).ToList();

                        var clinicSlotResponse = new ClinicSlotsResponse
                        {
                            ClinicId = clinicId,
                            Date = doctorSlot.RegisterDate.ToString("yyyy-MM-dd"),
                            Slots = slots
                        };

                        availableSlots.Add(clinicSlotResponse);
                    }
                    else
                    {
                        var clinicSlotResponse = new ClinicSlotsResponse
                        {
                            ClinicId = clinicId,
                            Date = doctorSlot.RegisterDate.ToString("yyyy-MM-dd"),
                            Slots = new List<int> {  }
                        };
                        availableSlots.Add(clinicSlotResponse);

                        Console.WriteLine($"Slots are empty for doctor slot on {doctorSlot.RegisterDate.ToString("yyyy-MM-dd")}");
                    }
                }

                return availableSlots;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAvailableSlotsInRangeForDoctor: {ex.Message}");
                throw;
            }
        }

        public async Task<List<PetManaDTO>> GetPetCategoryByUserId(string userId, string clinicId)
        {
            var pets = await _context.Pets
                .Include(p => p.PetType)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var clinicPetTypes = await _context.Clinics
                .Include(c => c.PetTypeClinics)
                .Where(c => c.ClinicId == clinicId)
                .SelectMany(c => c.PetTypeClinics)
                .Select(pc => pc.PetTypeId)
                .ToListAsync();

            var petsAvailableForClinic = pets
                .Where(p => clinicPetTypes.Contains(p.PetType.PetTypeId))
                .Select(p => new PetManaDTO
                {
                    PetId = p.PetId,
                    PetName = p.PetName,
                    PetTypeId = p.PetType.PetTypeId,
                    PetAge = p.PetAge,
                    PetColor = p.PetColor,
                    PetGender = p.PetGender,
                    PetSpecies = p.PetSpecies,
                    UserId = p.UserId,
                })
                .ToList();

            return petsAvailableForClinic;
        }






    }
}
