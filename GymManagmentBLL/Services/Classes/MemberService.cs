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

        public bool CreateMember(CreatMemberViewModel creatMember)
        {
            try {
                var emailExists = _memberRepository.GetAll(x => x.Email == creatMember.Email).Any();
            var phoneExists =_memberRepository.GetAll(x=>x.Phone == creatMember.Phone).Any();
            if (phoneExists||emailExists) { return false; }

            var member = new Member()
            {
                Phone = creatMember.Phone,
                Email = creatMember.Email,
                Name = creatMember.Name,
                Gender = creatMember.Gender,
                DateOfBirth = creatMember.DateOfBirth,

                Address = new Address()
                {
                    BuildingNumber = creatMember.BuildingNumber,
                    City = creatMember.City,
                    Street = creatMember.Street

                },
                HealthRecord = new HealthRecord() {

                    Height=creatMember.HealthRecordViewModel.Height,
                    Width=creatMember.HealthRecordViewModel.Weight,
                    BloodType=creatMember.HealthRecordViewModel.BloodType,
                    Note=creatMember.HealthRecordViewModel.Note,


                }


            };
            
          return   _memberRepository.Add(member)>0;
           
            }

            catch (Exception ex) { return false; }
        
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
