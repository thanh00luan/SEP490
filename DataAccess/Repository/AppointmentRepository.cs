﻿using DataAccess.DAO;
using DataAccess.DTO.Appointment;
using DataAccess.DTO.Clinic;
using DataAccess.DTO.DPet;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDAO _appointmentDAO;

        public AppointmentRepository(AppointmentDAO appointmentDAO)
        {
            _appointmentDAO = appointmentDAO;
        }

        public Task<IEnumerable<ClinicDTO>> GetAllClinic()
        => _appointmentDAO.GetAllClinic();

        public Task<ClinicSlotsResponse> GetAvailableSlots(string clinicId, DateTime date)
        => _appointmentDAO.GetAvailableSlots(clinicId, date);

        public void BookAppointment(string userId, DoctorClinicDTO appointment)
        => _appointmentDAO.BookAppointment(userId,appointment);

        public Task<bool> SetDoctorAvailableSlots(string doctorId, string clinicId, DateTime date, List<int> availableSlots)
        => _appointmentDAO.SetDoctorAvailableSlots(doctorId, clinicId, date, availableSlots);

        public Task<GetALLDTOCount> GetAll(string userId, int limit, int offset)
        => _appointmentDAO.GetAll(userId, limit, offset);

        public Task<List<ClinicSlotsResponse>> GetAvailableSlotsInRange(string clinicId, DateTime startDate, DateTime endDate)
            =>_appointmentDAO.GetAvailableSlotsInRange(clinicId, startDate, endDate);

        public Task<List<PetManaDTO>> GetPetCategoryByUserId(string userId, string clinicId)
            => _appointmentDAO.GetPetCategoryByUserId(userId, clinicId);
    }
}
