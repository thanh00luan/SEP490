using BussinessObject.Models;
using DataAccess.DTO.Appointment;
using DataAccess.DTO.Precscription;
using DataAccess.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IDoctorRepository
    {
        void SetDoctorAvailability(List<SetDoctorRequest> requests);
        List<int> GetDoctorAvailability(string doctorId, DateTime startDate, DateTime endDate);
        void ConfirmAppointment(string doctorId, string appointmentId);

        Task<GetALLDTOCount> GetDoctorAppointList(int limit, int offset, string doctorId, DateTime date);
        Task<PresDTO> GeneratePres(CreateDTO dto);

    }
}
