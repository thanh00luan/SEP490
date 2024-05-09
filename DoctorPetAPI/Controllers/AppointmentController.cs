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
        public async Task<IActionResult> GetAllAppointments([FromHeader(Name = "Authorization")] string authorizationHeader, int limit, int offset)
        {
            try
            {
                // Xác thực và lấy thông tin từ token
                var userId = GetUserIdFromToken(authorizationHeader);

                // Sử dụng userId để lấy danh sách cuộc hẹn từ repository
                var appointments = await _repository.GetAll(userId, limit, offset);

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAppointments: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private string GetUserIdFromToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                if (token.StartsWith("Bearer "))
                {
                    token = token.Replace("Bearer ", "");

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var decodedToken = tokenHandler.ReadJwtToken(token);

                    var userIdClaim = decodedToken.Claims.FirstOrDefault(claim => claim.Type == "UserID");

                    if (userIdClaim != null)
                    {
                        return userIdClaim.Value;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
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
        public async Task<IActionResult> GetAvailableSlotsInRange(string clinicId,DateTime startDate, DateTime endDate)
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

        [HttpGet("pet/{userId}/{clinicId}")]
        public async Task<ActionResult<List<PetManaDTO>>> GetPetCategoryByUserId(string userId, string clinicId)
        {
            try
            {
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
