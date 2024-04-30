using DataAccess.DAO;
using DataAccess.DTO.Admin;
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
        //statis
        public Task<AppointmentStatisticReponse> appointmentStatistics(DateTime start, DateTime end, string clinicId)
            => _adminDAO.appointmentStatistics(start, end, clinicId);

        public Task<int> countCustomer(string clinicId)
            => _adminDAO.countCustomer(clinicId);
        public Task<double> moneyStatisticByClinic(DateTime start, DateTime end, string clinicId)
            => _adminDAO.moneyStatistic(start, end, clinicId);



        //clinic
        public Task AddClinic(ClinicManaDTO dto)
            => _adminDAO.CreateClinic(dto);
        public Task DeleteClinic(string id)
            =>_adminDAO.DeleteClinic(id);
        public Task<ClinicListDTO> GetAllClinic(int limit, int offset)
            =>_adminDAO.GetAllClinic(limit, offset);
        public Task<ClinicManaDTO> GetClinicById(string id)
            =>_adminDAO.GetClinicByID(id);
        public Task<IEnumerable<ClinicManaDTO>> SortClinicByName()
            =>_adminDAO.SortClinicByName();
        public Task UpdateClinic(ClinicManaDTO updateDTO)
            =>_adminDAO.UpdateClinic(updateDTO);
        //PetCategory
        public Task AddPetCategory(PetCateManaDTO dto)
            =>_adminDAO.CreateCategoryAsync(dto);

        public Task DeletePetCategory(string id)
            =>_adminDAO.DeleteCateAsync(id);

        public Task<IEnumerable<PetCateManaDTO>> GetAllPetCategories()
            =>_adminDAO.GetAllCateAsync();

        public Task<PetCateManaDTO> GetPetCateById(string id)
            =>_adminDAO.GetCateByIdAsync(id);

        public Task UpdatePetCate(PetCateManaDTO updateDTO)
            =>_adminDAO.UpdateCateAsync(updateDTO); 

        

    }
}
