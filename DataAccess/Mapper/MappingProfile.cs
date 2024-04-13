using AutoMapper;
using BussinessObject.Models;
using DataAccess.DTO.Admin;
using DataAccess.DTO.Appointment;
using DataAccess.DTO.Clinic;
using DataAccess.DTO.DDoctor;
using DataAccess.DTO.DPet;
using DataAccess.DTO.User;

namespace DataAccess.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CreateMap<Pet, PetDTO>().ReverseMap();
            // CreateMap<Prescription, PrescriptionDTO>().ReverseMap();
            // CreateMap<Customer, CreateNewDTO>().ReverseMap();
            // CreateMap<Customer, UpdateDTO>().ReverseMap();
            // CreateMap<Customer, CustomerDTO>().ReverseMap();
            // CreateMap<Appointment, AppointmentDTO>().ReverseMap();
            // CreateMap<Bill, BillDTO>().ReverseMap();
            // CreateMap<BillMedicine, BillMedicineDTO>().ReverseMap();
            // CreateMap<Clinic, ClinicDTO>().ReverseMap();
            // CreateMap<Doctor, DoctorDTO>().ReverseMap();
            // CreateMap<Employee, EmployeeDTO>().ReverseMap();
            // CreateMap<EnterStorage, EnterStorageDTO>().ReverseMap();
            // CreateMap<MedicineCategory, MedicineCategoryDTO>().ReverseMap();
            // CreateMap<Medicine, MedicineDTO>().ReverseMap();
            // CreateMap<PetType, PetTypeDTO>().ReverseMap();
            // CreateMap<Storage, StorageDTO>().ReverseMap();
            // CreateMap<StorageStatus, StorageStatusDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<LoginDTO, User>().ReverseMap();
            CreateMap<RegisterDTO, User>().ReverseMap();
            CreateMap<ChangePassDTO, User>().ReverseMap();
            // CreateMap<ClinicReadDTO,Clinic>().ReverseMap();
            // CreateMap<CRUDTO, Clinic>().ReverseMap();
            // CreateMap<AddNewDoctorDTO, Doctor>().ReverseMap();

            CreateMap<PetDTO, Pet>().ReverseMap();
            CreateMap<Appointment, DoctorClinicDTO>().ReverseMap();

            CreateMap<CreatePetDTO, Pet>().ReverseMap();

            CreateMap<EditProfileDTO, User>().ReverseMap();

            CreateMap<GetAllDTO, Appointment>().ReverseMap();

            CreateMap<DoctorClinicDTO, Appointment>().ReverseMap();

            CreateMap<ClinicDTO, Clinic>().ReverseMap();

            CreateMap<DoctorDTO, Doctor>().ReverseMap();

            CreateMap<UserReadDTO, User>().ReverseMap();

            CreateMap<UserManaDTO, User>().ReverseMap();

            CreateMap<DoctorManaDTO, Doctor>().ReverseMap();

        }

    }
}