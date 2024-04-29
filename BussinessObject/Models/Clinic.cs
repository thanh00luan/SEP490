using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Clinic
    {
        [Key]
        public string ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string Address { get; set; }
        public string ClinicPhoneNumber { get; set; }
        public string Email { get; set; }

        private double Lat;
        private double Lng;

        public double Latitude
        {
            get { return Lat; }
            set { Lat = Math.Round(value, 4); }
        }

        public double Longitude
        {
            get { return Lng; }
            set { Lng = Math.Round(value, 4); }
        }
        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public List<Appointment> Appointments { get; set; }

        public List<Doctor> Doctors { get; set; }

        public List<Medicine> Medicines { get; set; }
    }
}
