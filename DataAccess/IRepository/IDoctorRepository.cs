using BussinessObject.Models;
using DataAccess.DTO.Appointment;
using DataAccess.DTO.Precscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IDoctorRepository
    {
        void SetDoctorAvailability(string doctorId, List<int> availabilitySlots);
        List<int> GetDoctorAvailability(string doctorId);
        void ConfirmAppointment(string doctorId, string appointmentId);

        Task<GetALLDTOCount> GetDoctorAppointList(int limit, int offset, string doctorId);
        Task<PresDTO> GeneratePres(CreateDTO dto);

    }
}
