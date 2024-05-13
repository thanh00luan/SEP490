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
        public async Task<ActionResult<IEnumerable<StaffDTO>>> GetAllStaffs([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            var Staffs = await _StaffRepository.getAlls();
            return Ok(Staffs);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDTO>> GetStaffById([FromHeader(Name = "Authorization")] string authorizationHeader, string id)
        {
            var Staff = await _StaffRepository.GetStaffById(id);

            if (Staff == null)
            {
                return NotFound();
            }

            return Ok(Staff);
        }

        [HttpPost]
        public async Task<ActionResult> AddStaff([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] StaffDTO StaffDTO)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            await _StaffRepository.AddStaff(StaffDTO);
            return CreatedAtAction(nameof(GetStaffById), new { id = StaffDTO.EmployeeId }, StaffDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStaff([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] StaffDTO StaffDTO)
        {
            // if (id != StaffDTO.StaffId)
            // {
            //     return BadRequest();
            // }
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }

            await _StaffRepository.UpdateStaff(StaffDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStaff([FromHeader(Name = "Authorization")] string authorizationHeader, string id)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            await _StaffRepository.DeleteStaff(id);
            return NoContent();
        }

        [HttpGet("sortedByName")]
        public async Task<IActionResult> GetStaffsSortedByName([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var sortedStaffs = await _StaffRepository.SortStaffByName();
                return Ok(sortedStaffs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getLists")]
        public async Task<IActionResult> GetPendingList([FromHeader(Name = "Authorization")] string authorizationHeader, DateTime date, int limit, int offset)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
                if (userId == null)
                {
                    return Unauthorized("Invalid token.");
                }
                var appointments = await _StaffRepository.GetPendingAppointment(userId, date, limit, offset);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting doctor appointments: {ex.Message}");
            }
        }

        [HttpGet("availableDoctors/{appointmentId}")]
        public async Task<IActionResult> GetAvailableDoctors([FromHeader(Name = "Authorization")] string authorizationHeader, string appointmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var availableDoctors = await _StaffRepository.GetAvailableDoctors(appointmentId);
                return Ok(availableDoctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting available doctors: {ex.Message}");
            }
        }

        [HttpPost("assignDoctor")]
        public async Task<IActionResult> AssignDoctorToAppointment([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] AssignDoctorRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
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
