using DataAccess.DTO.Appointment;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace DoctorPetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _repository;
        public AppointmentController(IAppointmentRepository repository)
        {
            _repository = repository;
        }



        [HttpGet("clinics")]
        public async Task<IActionResult> GetAllClinics()
        {
            try
            {
                var clinics = await _repository.GetAllClinic();
                return Ok(clinics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("getList")]
        public async Task<IActionResult> GetAllAppointments(DateTime date, int limit, int offset)
        {
            try
            {
                var appointments = await _repository.GetAll(date, limit, offset);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAppointments: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("book")]
        public IActionResult BookAppointment([FromBody] DoctorClinicDTO appointment)
        {
            try
            {
                _repository.BookAppointment(appointment);
                return Ok("Appointment booked successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("slots/{clinicId}")]
        public async Task<IActionResult> GetAvailableSlotsInRange(string clinicId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var slots = await _repository.GetAvailableSlotsInRange(clinicId, startDate, endDate);
                return Ok(slots);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("availableSlots/{clinicId}/{date}")]
        public async Task<IActionResult> GetAvailableSlots(string clinicId, DateTime date)
        {
            try
            {
                var availableSlotsResponse = await _repository.GetAvailableSlots(clinicId, date);

                if (availableSlotsResponse != null)
                {
                    return Ok(availableSlotsResponse);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPost("doctorSlots")]
        //public async Task<IActionResult> SetDoctorAvailableSlots([FromBody] AvailableSlotsDTO request)
        //{
        //    try
        //    {
        //        bool result = await _repository.SetDoctorAvailableSlots(request.DoctorId, request.ClinicId, request.Date, request.AvailableSlots);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred: {ex.Message}");
        //    }
        //}

    }
}
