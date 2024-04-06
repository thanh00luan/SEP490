using System.ComponentModel.DataAnnotations;

namespace DataAccess.DTO.User
{
    public class LoginDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}