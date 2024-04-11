using DataAccess.DAO;
using DataAccess.DTO.Appointment;
using DataAccess.DTO.Precscription;
using DataAccess.IRepository;
using DataAccess.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DoctorDAO _doctorDAO;

        public DoctorRepository(DoctorDAO doctorDAO)
        {
            _doctorDAO = doctorDAO;
        }
        public void ConfirmAppointment(string doctorId, string appointmentId)
            => _doctorDAO.ConfirmAppointment(doctorId, appointmentId);
        public List<SetDoctorRequest> GetDoctorAvailability(string doctorId, DateTime startDate, DateTime endDate)
            => _doctorDAO.GetDoctorAvailability(doctorId, startDate,endDate);

        public Task<GetALLDTOCount> GetDoctorAppointList(int limit, int offset, string doctorId, DateTime date)
            => _doctorDAO.GetDoctorAppointList(limit, offset, doctorId,date);

        public void SetDoctorAvailability(List<SetDoctorRequest> requests) 
            => _doctorDAO.SetDoctorAvailability(requests);

        public Task<PresDTO> GeneratePres(CreateDTO dto)
         =>_doctorDAO.GeneratePres(dto);
    }
}
