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

namespace DoctorPetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPetRepository _petRepository;
        public UserController(IUserRepository userRepository, IPetRepository petRepository)
        {
            _userRepository = userRepository;
            _petRepository = petRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

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

        [HttpPost("changepassword/{userId}")]
        public async Task<ActionResult> ChangePasswordAsync(string userId, [FromBody] ChangePassDTO dto)
        {
            try
            {
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
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials
                );
            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePet(string id)
        {
            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpGet("ByUsername/{username}")]
        public async Task<ActionResult<UserDTO>> GetUserByUserName(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("ByRole/{userRole}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersByRoleAsync(int userRole)
        {
            try
            {
                var users = await _userRepository.GetUsersByRoleAsync(userRole);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddPet")]
        public async Task<IActionResult> AddPet(PetManaDTO dto)
        {
            try
            {
                await _petRepository.AddPet(dto);
                return Ok("Pet added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllPet")]
        public async Task<IActionResult> GetAllPet(string userId)
        {
            try
            {
                var Pets = await _petRepository.GetAllPet(userId);
                return Ok(Pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetPetById/{id}")]
        public async Task<IActionResult> GetPetById( string id)
        {
            try
            {
                var Pet = await _petRepository.GetPetById(id);
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
        public async Task<IActionResult> UpdatePet(PetManaDTO dto)
        {
            try
            {
                await _petRepository.UpdatePet(dto);
                return Ok("Pet updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("{petId}")]
        public async Task<IActionResult> DeletePetAsync(string petId)
        {
            await _petRepository.DeletePet(petId);
            return Ok("Pet deleted successfully.");
        }


    }
}
