using System;

namespace DataAccess.DTO.User
{
    public class ViewUserDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
    }
}