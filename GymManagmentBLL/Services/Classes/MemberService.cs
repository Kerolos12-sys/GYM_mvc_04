using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels.MemberViewModels;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Classes
{
    public class MemberService : IMemberService
    {

        private readonly IGenericRepository<Member> _memberRepository;
        public MemberService(IGenericRepository<Member> memberRepository) {
         
            
            _memberRepository = memberRepository;
        
        }
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var   Members=_memberRepository.GetAll();
            if (Members == null) 
                return Enumerable.Empty<MemberViewModel>();
            var MembersViewModels = Members.Select(x => new MemberViewModel
            {

                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Photo = x.Photo,
                Gender=x.Gender.ToString(),
            });
           
            return MembersViewModels;
        }
    }
}
