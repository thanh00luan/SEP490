using BussinessObject.Models;
using DataAccess.DTO.Admin;
using DataAccess.DTO.Appointment;
using DataAccess.DTO.DDoctor;
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
        List<SetDoctorRequest> GetDoctorAvailability(string doctorId, DateTime startDate, DateTime endDate);
        void ConfirmAppointment(string doctorId, string appointmentId);

        Task<GetALLDTOCount> GetDoctorAppointList(int limit, int offset, string doctorId, DateTime date);
        Task<PresDTO> GeneratePres(GeneratePrescriptionDTO dto);

        Task<List<MedicineManaDTO>> SearchMedicineByName(string name);

        Task<MedicineListDTO> getMedicineByCate(string clinicId, string cateId, int limit, int offset);

        Task<PresDTO> GetPrescription(string appointmentId);
        Task<MedicineListDTO> SearchMedicineByCategoryAndKeyword(string clinicId, string categoryId, string keyword, int limit, int offset);

    }
}
