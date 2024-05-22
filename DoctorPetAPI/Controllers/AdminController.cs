using DataAccess.DTO.Admin;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using DataAccess.DTO.Employee;
using DataAccess.DAO;
using DataAccess.DTO.Clinic;
using BussinessObject.Models;
using DataAccess.RequestDTO;
using DataAccess.DTO.SuperAD;
using DataAccess.Repository;
using System.Collections.Generic;

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
        public async Task<IActionResult> AddMedicine([FromHeader(Name = "Authorization")] string authorizationHeader, MedicineManaDTO dto)
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
                await _repository.CreateMedicineAsync(userId, dto);
                return Ok("Medicine added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllMedicine")]
        public async Task<IActionResult> GetAllMedicine([FromHeader(Name = "Authorization")] string authorizationHeader, int limit, int offset)
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
                var medicines = await _repository.getAllMedicineAsync(userId, limit, offset);
                return Ok(medicines);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetMedicineById/{id}")]
        public async Task<IActionResult> GetMedicineById([FromHeader(Name = "Authorization")] string authorizationHeader, string id)
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
                var medicine = await _repository.GetMedicineByIdAsync(userId, id);
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
        public async Task<IActionResult> UpdateMedicine([FromHeader(Name = "Authorization")] string authorizationHeader, MedicineManaDTO dto)           
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _repository.UpdateMedicineAsync(dto);
                return Ok("Medicine updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("{medicineId}")]
        public async Task<IActionResult> DeleteMedicineAsync([FromHeader(Name = "Authorization")] string authorizationHeader, string medicineId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _repository.DeleteMedicineAsync(medicineId);
                return Ok("Medicine deleted successfully.");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Delete Error:  {ex.Message}");
            }
            
        }


        //Staff 
        [HttpPost("AddStaff")]
        public async Task<IActionResult> AddStaff([FromHeader(Name = "Authorization")] string authorizationHeader, StaffManaDTO staffDTO)
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

                await _repository.AddStaff(userId,staffDTO);
                return Ok("Staff added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllStaff")]
        public async Task<IActionResult> GetAllStaff([FromHeader(Name = "Authorization")] string authorizationHeader, int limit, int offset)
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
                var staff = await _repository.GetAllStaff(userId, limit,offset);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetStaffById/{id}")]
        public async Task<IActionResult> GetStaffById([FromHeader(Name = "Authorization")] string authorizationHeader, string id)
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
                var staff = await _repository.GetStaffById(userId, id);
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
        public async Task<IActionResult> UpdateStaff([FromHeader(Name = "Authorization")] string authorizationHeader, StaffManaDTO staffDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _repository.UpdateStaff(staffDTO);
                return Ok("Staff updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("DeleteStaff/{staffId}")]
        public async Task<IActionResult> DeleteStaff([FromHeader(Name = "Authorization")] string authorizationHeader, string staffId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _repository.DeleteStaffAsync(staffId);
                return Ok("Staff deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in delete staff: {ex.Message}");
            }
        }

        //User
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser([FromHeader(Name = "Authorization")] string authorizationHeader, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _repository.DeleteUserAsync(userId);
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromHeader(Name = "Authorization")] string authorizationHeader, int limit, int offset)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var users = await _repository.GetAllUsersAsync(limit, offset);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById([FromHeader(Name = "Authorization")] string authorizationHeader, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
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
        public async Task<IActionResult> UpdateUser([FromHeader(Name = "Authorization")] string authorizationHeader, UserManaDTO user)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _repository.UpdateUserAsync(user);
                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Doctor
        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctor([FromHeader(Name = "Authorization")] string authorizationHeader, DoctorManaDTO dto)
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
                await _repository.AddDoctor(userId, dto);
                return Ok("Doctor added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("DeleteDoctor/{doctorId}")]
        public async Task<IActionResult> DeleteDoctor([FromHeader(Name = "Authorization")] string authorizationHeader, string doctorId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _repository.DeleteDoctor(doctorId);
                return Ok("doctor deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctor([FromHeader(Name = "Authorization")] string authorizationHeader, int limit, int offset)
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

                var doctors = await _repository.GetAllDoctors(userId, limit, offset);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetDoctorById/{doctorId}")]
        public async Task<IActionResult> GetDoctorById([FromHeader(Name = "Authorization")] string authorizationHeader, string doctorId)
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
                var doctor = await _repository.GetDoctorById(userId, doctorId);
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
        public async Task<IActionResult> UpdateDoctor([FromHeader(Name = "Authorization")] string authorizationHeader, DoctorManaDTO doctor)
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
                await _repository.UpdateDoctor(userId,doctor);
                return Ok("Doctor updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Medicine Category
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] CateManaDTO dto)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _repository.AddCategory(dto);
                return Ok("Category added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory([FromHeader(Name = "Authorization")] string authorizationHeader, string id)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _repository.DeleteCategory(id);
                return Ok("Category deleted successfully");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error in delete Category: {ex.Message}");
            }
            
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var categories = await _repository.GetAllCategories();
                return Ok(categories);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById([FromHeader(Name = "Authorization")] string authorizationHeader, string id)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            var category = await _repository.GetCateById(id);
            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }

        [HttpGet("SearchCate")]
        public async Task<IActionResult> SearchCategories([FromHeader(Name = "Authorization")] string authorizationHeader, string name)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            var categories = await _repository.SearchByName(name);
            return Ok(categories);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] CateManaDTO updateDTO)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            await _repository.UpdateCate(updateDTO);
            return Ok("Category updated successfully");
        }

        //Pet Category
        //[HttpPost("AddPetCategory")]
        //public async Task<IActionResult> CreatePetType([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] PetTypeManaDTO dto)
        //{
        //    if (string.IsNullOrEmpty(authorizationHeader))
        //    {
        //        return Unauthorized("Authorization header is missing.");
        //    }
        //    await _repository.CreatePetType(dto);
        //    return Ok("Category added successfully");
        //}

        [HttpGet("GetAllPetCategories")]
        public async Task<IActionResult> GetAllPetCategories([FromHeader(Name = "Authorization")] string authorizationHeader, string clinicId)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            var categories = await _repository.GetAllPetCateAsync(clinicId);
            return Ok(categories);
        }

        [HttpGet("GetPetCategories")]
        public async Task<IActionResult> GetPetCategories([FromHeader(Name = "Authorization")] string authorizationHeader)
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
            var categories = await _repository.GetPetsCateAsync(userId);
            return Ok(categories);
        }

        [HttpGet("GetPetCategoryById/{clinicId}/{id}")]
        public async Task<IActionResult> GetPetCategoryById([FromHeader(Name = "Authorization")] string authorizationHeader, string clinicId, string id)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            var category = await _repository.GetCateByIdAsync(clinicId, id);
            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }


        [HttpPut("UpdatePetCategory")]
        public async Task<IActionResult> UpdatePetCategory([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] List<PetCateManaDTO> updateDTO)
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
            await _repository.UpdatePetCateAsync(userId, updateDTO);
            return Ok("Category updated successfully");
        }

        //Degree
        [HttpPost("AddDegree")]
        public async Task<IActionResult> AddDeegree([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] DoctorDegree dto)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            await _repository.AddDegree(dto);
            return Ok("Category added successfully");
        }
        [HttpGet("GetAllDegree")]
        public async Task<IActionResult> GetAllDegrees([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            var categories = await _repository.GetDoctorDegrees();
            return Ok(categories);
        }

        [HttpGet("MedicineReport")]
        public async Task<ActionResult> GenerateMedicineSalesReport([FromHeader(Name = "Authorization")] string authorizationHeader, DateTime startDate, DateTime endDate)
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
                var report = await _repository.GenerateMedicineSalesReport(startDate, endDate, userId);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
