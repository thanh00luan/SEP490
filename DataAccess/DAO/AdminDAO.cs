using BussinessObject.Data;
using BussinessObject.Models;
using DataAccess.DTO.SuperAD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class AdminDAO
    {
        private readonly ApplicationDbContext _context;

        public AdminDAO(ApplicationDbContext context) 
        {
            _context = context;
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

        public async Task<AppointmentStatisticReponse> appointmentStatistics(DateTime start, DateTime end, string clinicId)
        {
            IQueryable<Appointment> query = _context.Appointments;

            query = query.Where(a => a.AppointmentDate.Date >= start.Date && a.AppointmentDate.Date <= end.Date);

            if (!string.IsNullOrEmpty(clinicId))
            {
                query = query.Where(a => a.ClinicId == clinicId);
            }

            int totalAppointments = await query.CountAsync();
            int doneQuantity = await query.CountAsync(a => a.Status == "done");
            int inProgressQuantity = await query.CountAsync(a => a.Status == "inProgress");
            int waitingQuantity = await query.CountAsync(a => a.Status == "waiting");
            int pendingQuantity = await query.CountAsync(a => a.Status == "pending");

            AppointmentStatisticReponse response = new AppointmentStatisticReponse
            {
                total = totalAppointments,
                doneQuantity = doneQuantity,
                inProgressQuantity = inProgressQuantity,
                watingQuantity = waitingQuantity,
                pendingQuantity = pendingQuantity
            };

            return response;
        }

        public async Task<double> moneyStatistic(DateTime start, DateTime end, string clinicId)
        {
            double totalRevenue = 0;

            // Lọc đơn thuốc trong phạm vi thời gian và chi nhánh cụ thể
            var prescriptions = _context.Prescriptions
                .Where(p => p.ExaminationDay >= start && p.ExaminationDay <= end && p.Appointment.ClinicId == clinicId && p.Appointment.Status == "done")
                .Include(p => p.PrescriptionMedicines)
                .ThenInclude(pm => pm.Medicine);

            // Tính tổng doanh thu từ các đơn thuốc
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

        internal async Task<int> countCustomer(string clinicId)
        {
            int customerCount = await _context.Appointments
                .Where(a => a.ClinicId == clinicId)
                .Select(a => a.UserId)  
                .Distinct()  
                .CountAsync();  

            return customerCount;
        }
    }
}
