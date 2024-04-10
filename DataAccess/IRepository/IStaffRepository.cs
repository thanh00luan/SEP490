using BussinessObject.Models;
using DataAccess.DTO.Appointment;
using DataAccess.DTO.DDoctor;
using DataAccess.DTO.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IStaffRepository
    {
         Task<IEnumerable<StaffDTO>> getAlls();
         Task<StaffDTO> GetStaffById(string id);
         Task AddStaff(StaffDTO StaffDTO);
         Task UpdateStaff(StaffDTO Staff);
         Task DeleteStaff(string id);

         Task<IEnumerable<StaffDTO>> SortStaffByName();

        Task<GetALLDTOCount> GetPendingAppointment(DateTime startDate, DateTime endDate, int limit, int offset);

        Task<List<AvaibleDoctorDTO>> GetAvailableDoctors(int customerSlot, string clinicId);
        Task AssignDoctorToAppointment(string appointmentId, string doctorId);


    }
}
