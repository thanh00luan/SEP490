using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.DTO.User
{
    public class RegisterDTO
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string RePassword { get; set; }


        public string FullName { get; set; }


        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public DateTime Birthday { get; set; }
    }
}