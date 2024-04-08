using DataAccess.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using DataAccess.DTO.DDoctor;
using DataAccess.IRepository;
using DataAccess.DTO.Precscription;

namespace DoctorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _repository;
        public DoctorController(IDoctorRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("SetSlot")]
        public IActionResult SetDoctorAvailability([FromBody] DoctorAvailabilityRequest request)
        {
            try
            {
                _repository.SetDoctorAvailability(request.DoctorId, request.AvailabilitySlots);
                return Ok("Doctor availability set successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error setting doctor availability: {ex.Message}");
            }
        }

        [HttpGet("getSlot/{doctorId}")]
        public IActionResult GetDoctorAvailability(string doctorId)
        {
            try
            {
                var availabilitySlots = _repository.GetDoctorAvailability(doctorId);
                return Ok(availabilitySlots);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting doctor availability: {ex.Message}");
            }
        }

        [HttpGet("getAppointments")]
        public async Task<IActionResult> GetDoctorAppointments(int limit, int offset, string doctorId)
        {
            try
            {
                var appointments = await _repository.GetDoctorAppointList(limit, offset, doctorId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting doctor appointments: {ex.Message}");
            }
        }

        [HttpPost("confirmAppointment")]
        public IActionResult ConfirmAppointment([FromBody] AppointmentConfirmationRequest request)
        {
            try
            {
                _repository.ConfirmAppointment(request.DoctorId, request.AppointmentId);
                return Ok("Appointment confirmed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error confirming appointment: {ex.Message}");
            }
        }

        [HttpPost("generateInvoice")]
        public async Task<IActionResult> GeneratePrescription([FromBody] CreateDTO dto)
        {
            try
            {
                var presDTO = await _repository.GeneratePres(dto);

                return Ok(presDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating prescription: {ex.Message}");
            }
        }

    }
}
