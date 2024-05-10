using DataAccess.DTO.DDoctor;
using DataAccess.DTO.Precscription;
using DataAccess.IRepository;
using DataAccess.RequestDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DataAccess.DTO.Admin;

namespace DoctorPetAPI.Controllers
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
        public IActionResult SetDoctorAvailability([FromBody] List<SetDoctorRequest> requests)
        {
            try
            {
                _repository.SetDoctorAvailability(requests);
                return Ok("Doctor availability set successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error setting doctor availability: {ex.Message}");
            }
        }

        [HttpGet("getSlot/{doctorId}/{start}/{end}")]
        public IActionResult GetDoctorAvailability([FromHeader(Name = "Authorization")] string authorizationHeader, DateTime start, DateTime end)
        {
            try
            {
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
                var availabilitySlots = _repository.GetDoctorAvailability(userId, start, end);
                return Ok(availabilitySlots);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting doctor availability: {ex.Message}");
            }
        }

        [HttpGet("getAppointments")]
        public async Task<IActionResult> GetDoctorAppointments(int limit, int offset, [FromHeader(Name = "Authorization")] string authorizationHeader, DateTime date)
        {
            try
            {
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
                var appointments = await _repository.GetDoctorAppointList(limit, offset, userId, date);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting doctor appointments: {ex.Message}");
            }
        }

        [HttpPost("confirmAppointment")]
        public IActionResult ConfirmAppointment([FromHeader(Name = "Authorization")] string authorizationHeader, string appontmentId)
        {
            try
            {
                Authen authen = new Authen();
                var userId = authen.GetIdFromToken(authorizationHeader);
                _repository.ConfirmAppointment(userId, appontmentId);
                return Ok("Appointment confirmed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error confirming appointment: {ex.Message}");
            }
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePrescription([FromBody] GeneratePrescriptionDTO dto)
        {
            try
            {
                var presDTO = await _repository.GeneratePres(dto);

                return Ok(presDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getMedicine/{clinicId}/{cateId}")]
        public async Task<IActionResult> getMedicineByCate(string clinicId,string cateId, int limit, int offset)
        {
            try
            {
                var appointments = await _repository.getMedicineByCate(clinicId, cateId, limit, offset);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting medicine : {ex.Message}");
            }
        }

        [HttpGet("search/{name}")]
        public async Task<IActionResult> searchMedicineByName(string name)
        {
            try
            {
                var medicine = await _repository.SearchMedicineByName(name);
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

        [HttpGet("getInvoice/{appointmentId}")]
        public async Task<IActionResult> getInvoice(string appointmentId)
        {
            try
            {
                var press = await _repository.GetPrescription(appointmentId);
                if (press != null)
                    return Ok(press);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<MedicineManaDTO>>> SearchMedicineByCategoryAndKeyword([FromQuery] string clinicId, [FromQuery] string categoryId, [FromQuery] string keyword,int limit,int offset)
        {
            try
            {
                var medicines = await _repository.SearchMedicineByCategoryAndKeyword(clinicId, categoryId, keyword, limit, offset);
                return Ok(medicines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
