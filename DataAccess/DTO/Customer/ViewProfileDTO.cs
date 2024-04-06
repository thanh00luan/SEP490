using System;
using DataAccess.DTO.User;

namespace DataAccess.DTO.Customer
{
    public class ViewProfileDTO
    {
       public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
    }
}