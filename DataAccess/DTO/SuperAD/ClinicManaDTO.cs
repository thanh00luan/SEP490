using DataAccess.DTO.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.SuperAD
{
    public class ClinicManaDTO
    {
        public string ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string Address { get; set; }
        public string ClinicPhoneNumber { get; set; }
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

        public StaffManaDTO Staff { get; set; }
    }
}
