using DataAccess.DTO.Admin;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using DataAccess.DTO.Employee;

namespace DoctorPetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _repository;
        public AdminController(IAdminRepository repository)
        {
            _repository = repository;
        }
        //Staff 
        [HttpPost("AddStaff")]
        public async Task<IActionResult> AddStaff(StaffManaDTO staffDTO)
        {
            try
            {
                await _repository.AddStaff(staffDTO);
                return Ok("Staff added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllStaff")]
        public async Task<IActionResult> GetAllStaff()
        {
            try
            {
                var staff = await _repository.GetAllStaff();
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetStaffById/{id}")]
        public async Task<IActionResult> GetStaffById(string id)
        {
            try
            {
                var staff = await _repository.GetStaffById(id);
                if (staff != null)
                    return Ok(staff);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateStaff")]
        public async Task<IActionResult> UpdateStaff(StaffManaDTO staffDTO)
        {
            try
            {
                await _repository.UpdateStaff(staffDTO);
                return Ok("Staff updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //User
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                await _repository.DeleteUserAsync(userId);
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _repository.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var user = await _repository.GetUserByIdAsync(userId);
                if (user != null)
                    return Ok(user);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserManaDTO user)
        {
            try
            {
                await _repository.UpdateUserAsync(user);
                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
