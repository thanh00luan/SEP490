using System;

namespace DataAccess.DTO.User
{
    public class UserManaDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public int UserRole { get; set; }
    }
}