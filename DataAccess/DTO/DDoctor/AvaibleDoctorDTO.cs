﻿using DataAccess.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.DDoctor
{
    public class AvaibleDoctorDTO
    {
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialized { get; set; }
        public bool DoctorStatus { get; set; }
        public string DegreeId { get; set; }
        public string ClinicName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
