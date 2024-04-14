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

        //Medicine
        [HttpPost("AddMedicine")]
        public async Task<IActionResult> AddMedicine(MedicineManaDTO dto)
        {
            try
            {
                await _repository.CreateMedicineAsync(dto);
                return Ok("Medicine added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllMedicine")]
        public async Task<IActionResult> GetAllMedicine(int limit, int offset)
        {
            try
            {
                var medicines = await _repository.getAllMedicineAsync(limit, offset);
                return Ok(medicines);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetMedicineById/{id}")]
        public async Task<IActionResult> GetMedicineById(string id)
        {
            try
            {
                var medicine = await _repository.GetMedicineByIdAsync(id);
                if (medicine != null)
                    return Ok(medicine);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateMedicine")]
        public async Task<IActionResult> UpdateMedicine(MedicineManaDTO dto)
        {
            try
            {
                await _repository.UpdateMedicineAsync(dto);
                return Ok("Medicine updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("{medicineId}")]
        public async Task<IActionResult> DeleteMedicineAsync(string medicineId)
        {
            await _repository.DeleteMedicineAsync(medicineId);
            return Ok("Medicine deleted successfully.");
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
        public async Task<IActionResult> GetAllStaff(int limit, int offset)
        {
            try
            {
                var staff = await _repository.GetAllStaff(limit,offset);
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
        [HttpDelete("DeleteStaff/{staffId}")]
        public async Task<IActionResult> DeleteStaff(string staffId)
        {
            try
            {
                await _repository.DeleteStaffAsync(staffId);
                return Ok("Staff deleted successfully");
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
        public async Task<IActionResult> GetAllUsers(int limit, int offset)
        {
            try
            {
                var users = await _repository.GetAllUsersAsync(limit, offset);
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

        //Doctor
        [HttpDelete("DeleteDoctor/{doctorId}")]
        public async Task<IActionResult> DeleteDoctor(string doctorId)
        {
            try
            {
                await _repository.DeleteDoctor(doctorId);
                return Ok("doctor deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctor(int limit, int offset)
        {
            try
            {
                var doctors = await _repository.GetAllDoctors(limit, offset);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetDoctorById/{doctorId}")]
        public async Task<IActionResult> GetDoctorById(string doctorId)
        {
            try
            {
                var doctor = await _repository.GetDoctorById(doctorId);
                if (doctor != null)
                    return Ok(doctor);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateDoctor")]
        public async Task<IActionResult> UpdateDoctor(DoctorManaDTO doctor)
        {
            try
            {
                await _repository.UpdateDoctor(doctor);
                return Ok("Doctor updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
