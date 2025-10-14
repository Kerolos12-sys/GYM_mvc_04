using AutoMapper;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymManagmentBLL.ViewModels.SessionViewModel;
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

        }
    }
}
