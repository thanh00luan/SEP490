using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class UserOTP
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string UserId { get; set; } 
        public string OTP { get; set; } 
        public string Token { get; set; } 
        public DateTimeOffset ExpiryTime { get; set; } 

    }
}
