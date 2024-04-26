using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

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
    }
}
