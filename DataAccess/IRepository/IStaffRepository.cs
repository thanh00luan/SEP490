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
        public Task<IEnumerable<StaffDTO>> getAlls();
        public Task<StaffDTO> GetStaffById(string id);
        public Task AddStaff(StaffDTO StaffDTO);
        public Task UpdateStaff(StaffDTO Staff);
        public Task DeleteStaff(string id);

        public Task<IEnumerable<StaffDTO>> SortStaffByName();

        Task<GetALLDTOCount> GetPendingAppointment(int limit, int offset);

        Task<List<AvaibleDoctorDTO>> GetAvailableDoctors(int customerSlot, string clinicId);
        Task AssignDoctorToAppointment(string appointmentId, string doctorId, int slotNumber);


    }
}
