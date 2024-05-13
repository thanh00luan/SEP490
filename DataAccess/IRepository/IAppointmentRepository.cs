using DataAccess.DTO.Appointment;
using DataAccess.DTO.Clinic;
using DataAccess.DTO.DPet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IAppointmentRepository
    {

        Task<ClinicSlotsResponse> GetAvailableSlots(string clinicId, DateTime date);
        Task<IEnumerable<ClinicDTO>> GetAllClinic();
        Task<List<ClinicSlotsResponse>> GetAvailableSlotsInRange(string clinicId, DateTime startDate, DateTime endDate);
        void BookAppointment(string userId, DoctorClinicDTO appointment);
        Task<bool> SetDoctorAvailableSlots(string doctorId, string clinicId, DateTime date, List<int> availableSlots);

        Task<GetALLDTOCount> GetAll(string userId, int limit, int offset);
        Task<List<PetManaDTO>> GetPetCategoryByUserId(string userId, string clinicId);

    }
}
