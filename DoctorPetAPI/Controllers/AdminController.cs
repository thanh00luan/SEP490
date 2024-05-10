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
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
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
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
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
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
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
        public async Task<IActionResult> UpdateMedicine( MedicineManaDTO dto)           
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
        public async Task<IActionResult> GetAllStaff([FromHeader(Name = "Authorization")] string authorizationHeader, int limit, int offset)
        {
            try
            {
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
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
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
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
        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctor([FromHeader(Name = "Authorization")] string authorizationHeader, DoctorManaDTO dto)
        {
            try
            {
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
                await _repository.AddDoctor(userId, dto);
                return Ok("Doctor added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
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
        public async Task<IActionResult> GetAllDoctor([FromHeader(Name = "Authorization")] string authorizationHeader, int limit, int offset)
        {
            try
            {
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
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
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
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
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
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
        public async Task<IActionResult> AddCategory([FromBody] CateManaDTO dto)
        {
            await _repository.AddCategory(dto);
            return Ok("Category added successfully");
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _repository.DeleteCategory(id);
            return Ok("Category deleted successfully");
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _repository.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var category = await _repository.GetCateById(id);
            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }

        [HttpGet("SearchCate")]
        public async Task<IActionResult> SearchCategories(string name)
        {
            var categories = await _repository.SearchByName(name);
            return Ok(categories);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] CateManaDTO updateDTO)
        {
            await _repository.UpdateCate(updateDTO);
            return Ok("Category updated successfully");
        }

        //Pet Category
        [HttpPost("AddPetCategory")]
        public async Task<IActionResult> CreatePetType([FromBody] PetTypeManaDTO dto)
        {
            await _repository.CreatePetType(dto);
            return Ok("Category added successfully");
        }

        [HttpGet("GetAllPetCategories")]
        public async Task<IActionResult> GetAllPetCategories(string clinicId)
        {
            var categories = await _repository.GetAllPetCateAsync(clinicId);
            return Ok(categories);
        }

        [HttpGet("GetPetCategoryById/{clinicId}/{id}")]
        public async Task<IActionResult> GetPetCategoryById(string clinicId, string id)
        {
            var category = await _repository.GetCateByIdAsync(clinicId, id);
            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }


        [HttpPut("UpdatePetCategory")]
        public async Task<IActionResult> UpdatePetCategory([FromBody] PetTypeManaDTO updateDTO)
        {
            await _repository.UpdatePetCateAsync(updateDTO);
            return Ok("Category updated successfully");
        }

        //Degree
        [HttpPost("AddDegree")]
        public async Task<IActionResult> AddDeegree([FromBody] DoctorDegree dto)
        {
            await _repository.AddDegree(dto);
            return Ok("Category added successfully");
        }
        [HttpGet("GetAllDegree")]
        public async Task<IActionResult> GetAllDegrees()
        {
            var categories = await _repository.GetDoctorDegrees();
            return Ok(categories);
        }
    }
}
