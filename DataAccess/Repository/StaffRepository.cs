using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO.Appointment;
using DataAccess.DTO.DDoctor;
using DataAccess.DTO.Employee;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class StaffRepository : IStaffRepository
    {
        private readonly StaffDAO _StaffDAO;

        public StaffRepository(StaffDAO StaffDAO)
        {
            _StaffDAO = StaffDAO;
        }
        public Task AddStaff(StaffDTO StaffDTO) => _StaffDAO.AddStaff(StaffDTO);

        public Task AssignDoctorToAppointment(string appointmentId, string doctorId, int slotNumber)
            =>_StaffDAO.AssignDoctorToAppointment(appointmentId, doctorId, slotNumber); 

        public Task DeleteStaff(string id)
        {
            throw new NotImplementedException();
        }

        // public Task DeleteStaff(string id)=>_StaffDAO.DeleteStaff(id);
        public Task<IEnumerable<StaffDTO>> getAlls() => _StaffDAO.GetAllStaff();

        public Task<List<AvaibleDoctorDTO>> GetAvailableDoctors(int customerSlot, string clinicId)
         => _StaffDAO.GetAvailableDoctors(customerSlot, clinicId);

        public Task<GetALLDTOCount> GetPendingAppointment(int limit, int offset)
            =>_StaffDAO.GetPendingAppointment(limit, offset);

        public Task<StaffDTO> GetStaffById(string id) => _StaffDAO.GetStaffById(id);

        public Task<IEnumerable<StaffDTO>> SortStaffByName()
        {
            throw new NotImplementedException();
        }

        // public Task<IEnumerable<StaffDTO>> SortStaffByName()=> _StaffDAO.SortStaffByName();
        public Task UpdateStaff(StaffDTO Staff) => _StaffDAO.UpdateStaff(Staff);
    }
}
