using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Employee { 
    [Key]
    public string EmployeeId { get; set; }
    public bool EmployeeStatus { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; }
    public User User { get; set; }
    [ForeignKey("Clinic")]
    public string ClinicId { get; set; }
    public Clinic Clinic { get; set; }

    }
}
