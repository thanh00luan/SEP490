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
        public async Task<IActionResult> GetAppointmentStatistics(DateTime start, DateTime end, string clinicId)
        {
            try
            {
                var appointmentStatistics = await _superAdminRepo.appointmentStatistics(start, end, clinicId);
                return Ok(appointmentStatistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("countCustomer")]
        public async Task<IActionResult> GetCustomerCount(string clinicId)
        {
            try
            {
                var customerCount = await _superAdminRepo.countCustomer(clinicId);
                return Ok(customerCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("moneyStatisticByClinic")]
        public async Task<IActionResult> GetMoneyStatistic(DateTime start, DateTime end, string clinicId)
        {
            try
            {
                var moneyStatistic = await _superAdminRepo.moneyStatisticByClinic(start, end, clinicId);
                return Ok(moneyStatistic);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Clinic

        [HttpPost("AddClinic")]
        public async Task<IActionResult> AddClinic(ClinicManaDTO dto)
        {
            try
            {
                await _superAdminRepo.AddClinic(dto);
                return Ok("Clinic added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("DeleteClinic/{clinicId}")]
        public async Task<IActionResult> DeleteClinic(string clinicId)
        {
            try
            {
                await _superAdminRepo.DeleteClinic(clinicId);
                return Ok("Clinic deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllClinics")]
        public async Task<IActionResult> GetAllClinic(int limit, int offset)
        {
            try
            {
                var Clinics = await _superAdminRepo.GetAllClinic(limit, offset);
                return Ok(Clinics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetClinicById/{clinicId}")]
        public async Task<IActionResult> GetClinicById(string clinicId)
        {
            try
            {
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
        public async Task<IActionResult> UpdateClinic(ClinicManaDTO Clinic)
        {
            try
            {
                await _superAdminRepo.UpdateClinic(Clinic);
                return Ok("Clinic updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
