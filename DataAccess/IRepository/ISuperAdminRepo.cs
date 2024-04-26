using DataAccess.DTO.SuperAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface ISuperAdminRepo
    {
        //Statistic
        Task<AppointmentStatisticReponse> appointmentStatistics(DateTime start, DateTime end, string clinicId);

        Task<double> moneyStatisticByClinic(DateTime start, DateTime end, string clinicId);

        Task<int> countCustomer(string clinicId);

    }
}
