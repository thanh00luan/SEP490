using DataAccess.DAO;
using DataAccess.DTO.Admin;
using DataAccess.DTO.Appointment;
using DataAccess.DTO.DDoctor;
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
        public void ConfirmAppointment(string userId, string appointmentId)
            => _doctorDAO.ConfirmAppointment(userId, appointmentId);
        public List<SetDoctorRequest> GetDoctorAvailability(string userId, DateTime startDate, DateTime endDate)
            => _doctorDAO.GetDoctorAvailability(userId, startDate,endDate);

        public Task<GetALLDTOCount> GetDoctorAppointList(int limit, int offset, string userId, DateTime date)
            => _doctorDAO.GetDoctorAppointList(limit, offset, userId, date);

        public void SetDoctorAvailability(List<SetDoctorRequest> requests) 
            => _doctorDAO.SetDoctorAvailability(requests);

        public Task<PresDTO> GeneratePres(GeneratePrescriptionDTO dto)
         => _doctorDAO.GeneratePres(dto);

        public Task<PresDTO> GetPrescription(string appointmentId)
            => _doctorDAO.GetPrescription(appointmentId);

        public Task<List<MedicineManaDTO>> SearchMedicineByName(string name)
            => _doctorDAO.SearchMedicineByName(name);

        public Task<MedicineListDTO> getMedicineByCate(string clinicId, string cateId, int limit, int offset)
            => _doctorDAO.getMedicineByCate(clinicId,cateId, limit, offset);

        public Task<MedicineListDTO> SearchMedicineByCategoryAndKeyword(string clinicId, string categoryId, string keyword, int limit, int offset)
            => _doctorDAO.SearchMedicineByCategoryAndKeyword(clinicId, categoryId, keyword, limit, offset);
    }
}
