using DataAccess.DTO.Admin;
using DataAccess.DTO.Precscription;
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
        Task<List<AppointmentStatisticReponse>> appointmentStatistics(DateTime start, DateTime end, string clinicId);

        Task<double> moneyStatisticByClinic(DateTime start, DateTime end, string clinicId);

        Task<int> countCustomer(string clinicId);

        //clinic 
        Task AddClinic(ClinicManaDTO dto);
        Task DeleteClinic(string id);
        Task<ClinicListDTO> GetAllClinic(int limit, int offset);
        Task<ClinicManaDTO> GetClinicById(string id);
        Task<IEnumerable<ClinicManaDTO>> SortClinicByName();
        Task UpdateClinic(ClinicManaDTO updateDTO);

        //Pet Category
        Task AddPetCategory(PetCateManaDTO dto);
        Task DeletePetCategory(string id);
        Task<IEnumerable<PetCateManaDTO>> GetAllPetCategories();
        Task<PetCateManaDTO> GetPetCateById(string id);
        Task UpdatePetCate(PetCateManaDTO updateDTO);

    }
}
