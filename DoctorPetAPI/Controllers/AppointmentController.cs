using DataAccess.DTO.Appointment;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using DataAccess.DTO.DPet;

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
        public async Task<IActionResult> GetAllClinics([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var clinics = await _repository.GetAllClinic();
                return Ok(clinics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("getList")]
        public async Task<IActionResult> GetAllAppointments([FromHeader(Name = "Authorization")] string authorizationHeader, int limit, int offset)
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

                var appointments = await _repository.GetAll(userId, limit, offset);

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAppointments: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("book")]
        public IActionResult BookAppointment([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] DoctorClinicDTO appointment)
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
                _repository.BookAppointment(userId,appointment);
                return Ok("Appointment booked successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("slots/{clinicId}")]
        public async Task<IActionResult> GetAvailableSlotsInRange([FromHeader(Name = "Authorization")] string authorizationHeader, string clinicId,DateTime startDate, DateTime endDate)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var slots = await _repository.GetAvailableSlotsInRange(clinicId, startDate, endDate);
                return Ok(slots);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("availableSlots/{clinicId}/{date}")]
        public async Task<IActionResult> GetAvailableSlots([FromHeader(Name = "Authorization")] string authorizationHeader, string clinicId, DateTime date)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
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

        [HttpGet("pet/{clinicId}")]
        public async Task<ActionResult<List<PetManaDTO>>> GetPetByUserId([FromHeader(Name = "Authorization")] string authorizationHeader, string clinicId)
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
                var pets = await _repository.GetPetCategoryByUserId(userId, clinicId);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
