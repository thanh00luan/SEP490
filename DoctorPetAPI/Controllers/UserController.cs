using DataAccess.DTO.User;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using DataAccess.DTO.Admin;
using DataAccess.DTO.DPet;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Data;
using System.Security.Cryptography;
using DataAccess.RequestDTO;

namespace DoctorPetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPetRepository _petRepository;

        private ApplicationDbContext _context;
        public UserController(IUserRepository userRepository, IPetRepository petRepository,ApplicationDbContext dbcontext)
        {
            _userRepository = userRepository;
            _petRepository = petRepository;
            _context = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("GetUserInfo")]
        public async Task<IActionResult> GetUserById([FromHeader(Name = "Authorization")] string authorizationHeader)
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
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginDTO>> LoginAsync([FromBody] LoginDTO userDto)
        {

            var check = await _userRepository.LoginAsync(userDto);
            var user = await _userRepository.GetUserByUsernameAsync(userDto.UserName);
            if (user == null)
            {
                return BadRequest(new { Message = "User not found" });
            }
            TokenReponse tokenReponse = new TokenReponse();
            if (check != null)
            {
                var token = GenerateToken(user);
                tokenReponse.User = user;
                tokenReponse.AccessToken = token;
                return Ok(tokenReponse);
            }
            else
            {
                return BadRequest(new { Message = "Invalid login credentials" });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterDTO>> RegisterUserAsync([FromBody] RegisterDTO newUser)
        {
            var registeredUser = await _userRepository.RegisterUserAsync(newUser);

            if (registeredUser == null)
            {
                return Conflict("User already exists.");
            }

            return Ok(registeredUser);
        }

        [HttpPost("changepassword")]
        public async Task<ActionResult> ChangePasswordAsync([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] ChangePassDTO dto)
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
                await _userRepository.ChangePasswordAsync(userId, dto);
                return Ok("Password changed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private string GenerateToken(UserDTO acc)
        {
            var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet."));
            var credentials = new SigningCredentials(secureKey, SecurityAlgorithms.HmacSha256);
            var claim = new[]
            {
                new Claim ("UserName", acc.UserName),
                new Claim ("UserID", acc.UserId),
                new Claim ("UserRole", acc.UserRole.ToString())
            };
            var token = new JwtSecurityToken
                (
                issuer: "issuer",
                audience: "audience",
                claim,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
                );
            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }

        [HttpGet("ByUsername/{username}")]
        public async Task<ActionResult<UserDTO>> GetUserByUserName([FromHeader(Name = "Authorization")] string authorizationHeader, string username)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("ByRole/{userRole}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersByRoleAsync([FromHeader(Name = "Authorization")] string authorizationHeader, int userRole)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var users = await _userRepository.GetUsersByRoleAsync(userRole);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddPet")]
        public async Task<IActionResult> AddPet([FromHeader(Name = "Authorization")] string authorizationHeader, PetManaDTO dto)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                await _petRepository.AddPet(dto);
                return Ok("Pet added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllPet")]
        public async Task<IActionResult> GetAllPet([FromHeader(Name = "Authorization")] string authorizationHeader)
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
                var Pets = await _petRepository.GetAllPet(userId);
                return Ok(Pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetPetById/{petId}")]
        public async Task<IActionResult> GetPetById([FromHeader(Name = "Authorization")] string authorizationHeader,string petId)
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
                var Pet = await _petRepository.GetPetById(petId);
                if (Pet != null)
                    return Ok(Pet);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdatePet")]
        public async Task<IActionResult> UpdatePet([FromHeader(Name = "Authorization")] string authorizationHeader, PetManaDTO dto)
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
                await _petRepository.UpdatePet(userId,dto);
                return Ok("Pet updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("DeletePet/{petId}")]
        public async Task<IActionResult> DeletePetAsync([FromHeader(Name = "Authorization")] string authorizationHeader, string petId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }

                await _petRepository.DeletePet(petId);

                return Ok("Pet deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getUserInvoice/{appointmentId}")]
        public async Task<IActionResult> GetUserInvoice([FromHeader(Name = "Authorization")] string authorizationHeader, string appointmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Authorization header is missing.");
                }
                var prescriptionDTO = await _userRepository.GetPrescription(appointmentId);

                if (prescriptionDTO == null)
                {
                    return NotFound();
                }

                return Ok(prescriptionDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user invoice: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPasswordAsync(string userName)
        {
            try
            {
                var result = await _userRepository.ForgotPasswordAsync(userName);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Failed to send reset password email.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send reset password email: {ex.Message}");
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyOTPAsync(string userId, string OTP)
        {
            try
            {
                bool result = await _userRepository.VerifyOTPAsync(userId, OTP);

                if (result)
                {
                    return Ok("OTP verified successfully.");
                }
                else
                {
                    return BadRequest("Invalid OTP or expired.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to verify OTP: {ex.Message}");
            }
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPasswordAsync(string userId, string newPass)
        {
            try
            {
                bool result = await _userRepository.ResetPasswordAsync(userId, newPass);

                if (result)
                {
                    return Ok("Password reset successfully.");
                }
                else
                {
                    return BadRequest("Failed to reset password.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to reset password: {ex.Message}");
            }
        }


    }
}
