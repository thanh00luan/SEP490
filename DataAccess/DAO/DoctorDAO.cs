using AutoMapper;
using BussinessObject.Data;
using BussinessObject.Models;
using DataAccess.DTO.Admin;
using DataAccess.DTO.Appointment;
using DataAccess.DTO.DDoctor;
using DataAccess.DTO.Precscription;
using DataAccess.RequestDTO;
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

        public void SetDoctorAvailability(List<SetDoctorRequest> requests)
        {
            try
            {
                foreach (var request in requests)
                {
                    var doctorSlot = _context.DoctorSlots.FirstOrDefault(ds => ds.DoctorId == request.DoctorId && ds.RegisterDate.Date == request.RegisterDate.Date);

                    if (doctorSlot == null)
                    {
                        doctorSlot = new DoctorSlot
                        {
                            DoctorSId = Guid.NewGuid().ToString(),
                            DoctorId = request.DoctorId,
                            RegisterDate = request.RegisterDate.Date,
                            Slots = string.Join(",", request.availabilitySlots.OrderBy(slot => slot))
                        };
                        _context.DoctorSlots.Add(doctorSlot);
                    }
                    else
                    {
                        var existingSlots = doctorSlot.Slots?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() ?? new List<int>();
                        existingSlots.AddRange(request.availabilitySlots);
                        var uniqueSlots = existingSlots.Distinct().OrderBy(slot => slot).ToList();
                        doctorSlot.Slots = string.Join(",", uniqueSlots);
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SetDoctorAvailability: {ex.Message}");
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




        public async Task<GetALLDTOCount> GetDoctorAppointList(int limit, int offset, string doctorId, DateTime date)
        {
            try
            {
                var currentTime = DateTime.UtcNow;

                var appointments = await _context.Appointments
                    .Include(a => a.Pet)
                    .Include(a => a.Clinic)
                    .Include(a => a.User)
                    .Include(a => a.Doctor)
                    .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date.Date)
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
                    .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date.Date)
                    .CountAsync();

                return appointmentDTOWithCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDoctorAppointList: {ex.Message}");
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
                    throw new ArgumentException("Appointment not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ConfirmAppointment: {ex.Message}");
                throw;
            }
        }

        public async Task<PresDTO> GeneratePres(GeneratePrescriptionDTO dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var appointment = await _context.Appointments
                        .Include(a => a.Pet)
                        .Include(a => a.User)
                        .FirstOrDefaultAsync(a => a.AppointmentId == dto.CreateDTO.AppointmentId);

                    if (appointment == null)
                    {
                        throw new ArgumentException("Appointment not found.");
                    }

                    if (string.IsNullOrEmpty(dto.CreateDTO.Diagnose) || string.IsNullOrEmpty(dto.CreateDTO.Reason))
                    {
                        throw new ArgumentException("Diagnose and Reason cannot be empty.");
                    }

                    foreach (var medicineInfo in dto.PrescriptionMedicines)
                    {
                        var medicineId = medicineInfo.MedicineId;
                        var quantity = medicineInfo.Quantity;

                        var medicine = await _context.Medicines.FindAsync(medicineId);

                        if (medicine == null)
                        {
                            throw new ArgumentException($"Medicine with ID {medicineId} not found.");
                        }

                        if (medicine.Inventory < quantity)
                        {
                            throw new InvalidOperationException($"Not enough {medicine.MedicineName} in stock.");
                        }
                        else
                        {
                            medicine.Inventory -= quantity;
                        }
                    }
                    var prescriptionId = Guid.NewGuid().ToString();

                    var prescription = new Prescription
                    {
                        PrescriptionId = prescriptionId,
                        Diagnose = dto.CreateDTO.Diagnose,
                        ExaminationDay = appointment.AppointmentDate,
                        CreateDay = DateTime.UtcNow,
                        Reason = dto.CreateDTO.Reason,
                        PetId = appointment.PetId,
                        UserId = appointment.UserId,
                        
                        PrescriptionMedicines = dto.PrescriptionMedicines
                            .Select(medicineInfo => new PrescriptionMedicine
                            {
                                PrescriptionMedicineId = Guid.NewGuid().ToString(),
                                MedicineId = medicineInfo.MedicineId,
                                Quantity = medicineInfo.Quantity
                            }).ToList()
                    };

                    _context.Prescriptions.Add(prescription);
                    

                    appointment.Status = "done";

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    var presDTO = new PresDTO
                    {
                        PrescriptionId = prescription.PrescriptionId,
                        Diagnose = prescription.Diagnose,
                        ExaminationDay = prescription.ExaminationDay,
                        CreateDay = prescription.CreateDay,
                        Reason = prescription.Reason,
                        PetId = prescription.PetId,
                        PetName = appointment.Pet.PetName,
                        UserId = prescription.UserId,
                        FullName = appointment.User.FullName
                    };

                    return presDTO;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error generating prescription from appointment: {ex.Message}");
                    throw;
                }
            }
        }
       
        public async Task<MedicineManaDTO> SearchMedicineByName(string medicineName)
        {
            var medicine = await _context.Medicines.Include(m=>m.MedicineCategory).FirstOrDefaultAsync(m => m.MedicineName == medicineName );

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
                MedicineCateId = medicine.MedicineCateId,
                MedicineCateName = medicine.MedicineCategory.CategoryName
            };
        }


        public async Task<MedicineListDTO> getMedicineByCate(string clinicId, string categoryId, int limit, int offset)
        {
            try
            {
                var categoryMedicinesQuery = _context.Medicines
                    .Where(m => m.ClinicId == clinicId && m.MedicineCateId == categoryId)
                    .Include(m => m.MedicineCategory)
                    .OrderBy(m => m.MedicineId);

                var totalMedicines = await categoryMedicinesQuery.CountAsync();

                var categoryMedicines = await categoryMedicinesQuery.Skip(offset).Take(limit).ToListAsync();

                var medicineDTOs = categoryMedicines.Select(medicine => new MedicineManaDTO
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
                Console.WriteLine($"Database Error in GetMedicineByCategoryAsync: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMedicineByCategoryAsync: {ex.Message}");
                throw;
            }
        }

    }
}
