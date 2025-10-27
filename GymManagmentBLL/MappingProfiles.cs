using AutoMapper;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymManagmentBLL.ViewModels.MemberViewModels;
using GymManagmentBLL.ViewModels.SessionViewModel;
using GymManagmentBLL.ViewModels.TrainerViewModels;
using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Session, SessionViewModel>()
               .ForMember(dest => dest.CategoryName, options => options.MapFrom(src => src.SessionCategory.CategoryName))
               .ForMember(dest => dest.TrainerName, options => options.MapFrom(src => src.Trainer.Name))
               .ForMember(dest => dest.AvailableSlots, options => options.Ignore());
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();


           ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            CreateMap<Member, MemberViewModel>()
               .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
               .ForMember(dest => dest.Address,
               opt => opt.MapFrom(src => $"{src.Address.BuildingNumber}-{src.Address.Street}-{src.Address.City}"))
               .ReverseMap();

            CreateMap<CreatMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => new HealthRecord
                {
                    Height = src.HealthRecordViewModel.Height,
                    Width = src.HealthRecordViewModel.Weight,
                    BloodType = src.HealthRecordViewModel.BloodType,
                    Note = src.HealthRecordViewModel.Note
                }));

            CreateMap<Member, MemberToUpdateViewModel>()
               .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
               .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
               .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
               .ReverseMap()
               .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
               .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
               .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber));


            CreateMap<HealthRecord, HealthRecordViewModel>()
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Width))
                .ReverseMap();
            /////////////////////////////////////////////////////////////////////////////////////////////////////////


            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialties.ToString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber}-{src.Address.Street}-{src.Address.City}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString()));

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                  BuildingNumber = src.BuildingNumber,
                  Street = src.Street,
                  City = src.City
                 }));

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ReverseMap();




        }
    }
}
