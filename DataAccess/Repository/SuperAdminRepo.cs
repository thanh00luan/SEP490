using DataAccess.DAO;
using DataAccess.DTO.SuperAD;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class SuperAdminRepo : ISuperAdminRepo
    {
        private readonly AdminDAO _adminDAO;
        public SuperAdminRepo(AdminDAO adminDAO)
        {
            _adminDAO = adminDAO;
        }
        public Task<AppointmentStatisticReponse> appointmentStatistics(DateTime start, DateTime end, string clinicId)
            => _adminDAO.appointmentStatistics(start, end,clinicId);

        public Task<int> countCustomer(string clinicId)
            => _adminDAO.countCustomer(clinicId);

        public Task<double> moneyStatisticByClinic(DateTime start, DateTime end, string clinicId)
            => _adminDAO.moneyStatistic(start, end,clinicId);
    }
}
