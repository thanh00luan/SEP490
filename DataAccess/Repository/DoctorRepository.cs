using DataAccess.DAO;
using DataAccess.DTO.Appointment;
using DataAccess.IRepository;
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
        {
            throw new NotImplementedException();
        }
        public List<int> GetDoctorAvailability(string doctorId)
            => _doctorDAO.GetDoctorAvailability(doctorId);

        public Task<GetALLDTOCount> GetDoctorAppointList(int limit, int offset, string doctorId)
            => _doctorDAO.GetDoctorAppointList(limit, offset, doctorId);

        public void SetDoctorAvailability(string doctorId, List<int> availabilitySlots) 
            => _doctorDAO.SetDoctorAvailability(doctorId, availabilitySlots);

        
    }
}
