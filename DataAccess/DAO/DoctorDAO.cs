using AutoMapper;
using BussinessObject.Data;
using BussinessObject.Models;
using DataAccess.DTO.Appointment;
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

        public List<int> GetDoctorAvailability(string doctorId, DateTime registerDate)
        {
            try
            {
                var doctorSlot = _context.DoctorSlots.FirstOrDefault(ds => ds.DoctorId == doctorId && ds.RegisterDate.Date == registerDate.Date);

                if (doctorSlot != null)
                {
                    return doctorSlot.Slots?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() ?? new List<int>();
                }
                else
                {
                    return null;
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
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ConfirmAppointment: {ex.Message}");
                throw;
            }
        }

        public async Task<PresDTO> GeneratePres(CreateDTO dto)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Pet)
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.AppointmentId == dto.AppointmentId);

                if (appointment == null)
                {
                    throw new ArgumentException("Appointment not found.");
                }
                var medicine = await _context.Medicines.FindAsync(dto.MedicineId);
                if (medicine == null)
                {
                    throw new ArgumentException("Medicine not found.");
                }

                if (medicine.Inventory < dto.Quantity)
                {
                    throw new InvalidOperationException("Not enough medicine in stock.");
                }

                var prescription = new Prescription
                {
                    PrescriptionId = Guid.NewGuid().ToString(),
                    Diagnose = dto.Diagnose,
                    ExaminationDay = appointment.AppointmentDate,
                    CreateDay = DateTime.UtcNow,
                    Reason = dto.Reason,
                    PetId = appointment.PetId,
                    MedicineId = dto.MedicineId,
                    UserId = appointment.UserId
                };

                medicine.Inventory -= dto.Quantity;

                _context.Prescriptions.Add(prescription);

                appointment.Status = "done";

                await _context.SaveChangesAsync();

                var presDTO = new PresDTO
                {
                    PrescriptionId = prescription.PrescriptionId,
                    Diagnose = prescription.Diagnose,
                    ExaminationDay = prescription.ExaminationDay,
                    CreateDay = prescription.CreateDay,
                    Reason = prescription.Reason,
                    PetId = prescription.PetId,
                    PetName = prescription.Pet.PetName,
                    UserId = prescription.UserId,
                    FullName = prescription.User.FullName,
                    MedicineId = prescription.MedicineId,
                    MedicineName = prescription.Medicine.MedicineName
                };

                return presDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating prescription from appointment: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GenerateInvoiceFromAppointment(GenerateInvoiceDTO dto)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Pet)
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.AppointmentId == dto.AppointmentId);

                if (appointment == null)
                {
                    throw new ArgumentException("Appointment not found.");
                }

                var invoice = new Bill
                {
                    BillId = Guid.NewGuid().ToString(),
                    CreateDate = DateTime.UtcNow,
                    TotalPrices =await CalculateTotalAmount(appointment, dto.MedicineIds),
                    Quantity = 1,
                    PaymentMethod = dto.PaymentMethod, 
                    IsPaid = dto.IsPaid, 
                    Discount = dto.Discount, 
                    Note = dto.Note, 
                    UserId = appointment.UserId
                };

                _context.Bills.Add(invoice);

                foreach (var medicineId in dto.MedicineIds)
                {
                    var billMedicine = new BillMedicine
                    {
                        BillMedicineId = Guid.NewGuid().ToString(),
                        BillId = invoice.BillId,
                        MedicineId = medicineId,
                        Quantity = 1 
                    };

                    _context.BillMedicines.Add(billMedicine);
                }

                await _context.SaveChangesAsync();

                return invoice.BillId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating invoice from appointment: {ex.Message}");
                throw;
            }
        }


        private async Task<double> CalculateTotalAmount(Appointment appointment, List<string> medicineIds)
        {
            double totalAmount = 0;
            foreach (var medicineId in medicineIds)
            {
                var medicine = await _context.Medicines.FindAsync(medicineId);

                if (medicine != null)
                {
                    totalAmount += medicine.Prices; 
                }
            }
            return totalAmount;
        }






    }
}
