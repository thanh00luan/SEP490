using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using DataAccess.DTO.Admin;
using DataAccess.DTO.SuperAD;

namespace DoctorPetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly ISuperAdminRepo _superAdminRepo;
        public SuperAdminController(ISuperAdminRepo repository)
        {
            _superAdminRepo = repository;
        }

        [HttpGet("appointmentStatistics")]
        public async Task<IActionResult> GetAppointmentStatistics([FromHeader(Name = "Authorization")] string authorizationHeader, DateTime start, DateTime end, string clinicId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var appointmentStatistics = await _superAdminRepo.appointmentStatistics(start, end, clinicId);
                return Ok(appointmentStatistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("countCustomer")]
        public async Task<IActionResult> GetCustomerCount([FromHeader(Name = "Authorization")] string authorizationHeader, string clinicId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var customerCount = await _superAdminRepo.countCustomer(clinicId);
                return Ok(customerCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("moneyStatisticByClinic")]
        public async Task<IActionResult> GetMoneyStatistic([FromHeader(Name = "Authorization")] string authorizationHeader, DateTime start, DateTime end, string clinicId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var moneyStatistic = await _superAdminRepo.moneyStatisticByClinic(start, end, clinicId);
                return Ok(moneyStatistic);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GenerateReport")]
        public async Task<ActionResult> GenerateMedicineSalesReport([FromHeader(Name = "Authorization")] string authorizationHeader, DateTime startDate, DateTime endDate, string clinicId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var report = await _superAdminRepo.GenerateMedicineSalesReport(startDate, endDate, clinicId);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Clinic

        [HttpPost("AddClinic")]
        public async Task<IActionResult> AddClinic([FromHeader(Name = "Authorization")] string authorizationHeader, ClinicManaDTO dto)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _superAdminRepo.AddClinic(dto);
                return Ok("Clinic added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("DeleteClinic/{clinicId}")]
        public async Task<IActionResult> DeleteClinic([FromHeader(Name = "Authorization")] string authorizationHeader, string clinicId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _superAdminRepo.DeleteClinic(clinicId);
                return Ok("Clinic deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllClinics")]
        public async Task<IActionResult> GetAllClinic([FromHeader(Name = "Authorization")] string authorizationHeader, int limit, int offset)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var Clinics = await _superAdminRepo.GetAllClinic(limit, offset);
                return Ok(Clinics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetClinicById/{clinicId}")]
        public async Task<IActionResult> GetClinicById([FromHeader(Name = "Authorization")] string authorizationHeader, string clinicId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var Clinic = await _superAdminRepo.GetClinicById(clinicId);
                if (Clinic != null)
                    return Ok(Clinic);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateClinic")]
        public async Task<IActionResult> UpdateClinic([FromHeader(Name = "Authorization")] string authorizationHeader, ClinicManaDTO Clinic)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _superAdminRepo.UpdateClinic(Clinic);
                return Ok("Clinic updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Pet Category
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] PetCateManaDTO dto)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            await _superAdminRepo.AddPetCategory(dto);
            return Ok("Category added successfully");
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory([FromHeader(Name = "Authorization")] string authorizationHeader, string id)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            await _superAdminRepo.DeletePetCategory(id);
            return Ok("Category deleted successfully");
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            var categories = await _superAdminRepo.GetAllPetCategories();
            return Ok(categories);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById([FromHeader(Name = "Authorization")] string authorizationHeader, string id)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            var category = await _superAdminRepo.GetPetCateById(id);
            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] PetCateManaDTO updateDTO)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            await _superAdminRepo.UpdatePetCate(updateDTO);
            return Ok("Category updated successfully");
        }

        //Doctor
        [HttpGet("GetDoctors")]
        public async Task<IActionResult> GetAllDoctor([FromHeader(Name = "Authorization")] string authorizationHeader, string clinicId, int limit, int offset)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var doctors = await _superAdminRepo.GetAllDoctors(clinicId, limit, offset);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllStaff")]
        public async Task<IActionResult> GetAllStaff([FromHeader(Name = "Authorization")] string authorizationHeader, string clinicId,  int limit, int offset)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var staff = await _superAdminRepo.GetAllStaff(clinicId, limit, offset);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
