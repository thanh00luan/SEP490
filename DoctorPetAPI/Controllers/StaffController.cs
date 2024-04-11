using DataAccess.DTO.Employee;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace DoctorPetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepository _StaffRepository;
        public StaffController(IStaffRepository StaffRepository)
        {
            _StaffRepository = StaffRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDTO>>> GetAllStaffs()
        {
            var Staffs = await _StaffRepository.getAlls();
            return Ok(Staffs);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDTO>> GetStaffById(string id)
        {
            var Staff = await _StaffRepository.GetStaffById(id);

            if (Staff == null)
            {
                return NotFound();
            }

            return Ok(Staff);
        }

        [HttpPost]
        public async Task<ActionResult> AddStaff([FromBody] StaffDTO StaffDTO)
        {
            await _StaffRepository.AddStaff(StaffDTO);
            return CreatedAtAction(nameof(GetStaffById), new { id = StaffDTO.EmployeeId }, StaffDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStaff([FromBody] StaffDTO StaffDTO)
        {
            // if (id != StaffDTO.StaffId)
            // {
            //     return BadRequest();
            // }

            await _StaffRepository.UpdateStaff(StaffDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStaff(string id)
        {
            await _StaffRepository.DeleteStaff(id);
            return NoContent();
        }

        [HttpGet("sortedByName")]
        public async Task<IActionResult> GetStaffsSortedByName()
        {
            try
            {
                var sortedStaffs = await _StaffRepository.SortStaffByName();
                return Ok(sortedStaffs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getLists")]
        public async Task<IActionResult> GetPendingList(DateTime startDate, DateTime endDate, int limit, int offset)
        {
            try
            {
                var appointments = await _StaffRepository.GetPendingAppointment(startDate, endDate, limit, offset);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting doctor appointments: {ex.Message}");
            }
        }

        [HttpGet("availableDoctors/{appointmentId}")]
        public async Task<IActionResult> GetAvailableDoctors(string appointmentId)
        {
            try
            {
                var availableDoctors = await _StaffRepository.GetAvailableDoctors(appointmentId);
                return Ok(availableDoctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting available doctors: {ex.Message}");
            }
        }

        [HttpPost("assignDoctor")]
        public async Task<IActionResult> AssignDoctorToAppointment([FromBody] AssignDoctorRequest request)
        {
            try
            {
                await _StaffRepository.AssignDoctorToAppointment(request.AppointmentId, request.DoctorId);
                return Ok("Doctor assigned successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error assigning doctor to appointment: {ex.Message}");
            }
        }

    }
}
