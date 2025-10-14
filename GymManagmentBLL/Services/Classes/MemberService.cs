﻿using GymManagmentBLL.Services.Interfaces;
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

        private readonly IUnitOfWork _unitOfWork;
        
        //private readonly IGenericRepository<Member> _memberRepository;
        //private readonly IGenericRepository<MemberShip> _membershipRepository;
        //private readonly IPlanRepository _planRepository;
        //private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        //private readonly IGenericRepository<MemberSession> _memberSessionRepository;
        public MemberService( IUnitOfWork unitOfWork)
        {  
            //_memberRepository = memberRepository;
            //_membershipRepository = membershipRepository;
            //_planRepository = planRepository;
            //_healthRecordRepository = healthRecordRepository;
            //_memberSessionRepository = memberSessionRepository;
            _unitOfWork = unitOfWork;

        }




        //1
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var   Members=_unitOfWork.GetRepository<Member>().GetAll();
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
        //2
        public bool CreateMember(CreatMemberViewModel creatMember)
        {
            try {
            var emailExists = _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == creatMember.Email).Any();
            var phoneExists = _unitOfWork.GetRepository<Member>().GetAll(x=>x.Phone == creatMember.Phone).Any();
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
            
               _unitOfWork.GetRepository<Member>().Add(member); //added locally
                return _unitOfWork.SaveChanges()>0;
           
            }

            catch (Exception ex) { return false; }
        
        }
      
        //3
        public MemberViewModel GetMemberDetails(int MemberId)
        {
           
            var Member=_unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member == null) { return null; }
            var viewmodel = new MemberViewModel()
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Photo = Member.Photo,
                Gender = Member.Gender.ToString(),
                DateOfBirth=Member.DateOfBirth.ToString(),
                Address=$"{Member.Address.BuildingNumber}-{Member.Address.Street}-{Member.Address.City}",


            };

            var ActiveMemberShip= _unitOfWork.GetRepository<MemberShip>().GetAll(x=>x.MemberId==MemberId&&x.Status=="Active").FirstOrDefault();
            if (ActiveMemberShip is not null) 
            {

                viewmodel.MemberShipStartDate=ActiveMemberShip.CreatedAt.ToShortDateString();
                viewmodel.MemberShipEndDate=ActiveMemberShip.EndDate.ToShortDateString();
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                viewmodel.PlanName = plan?.Name;
            }

            return viewmodel;
        }
        //4
        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
           var memberhealthrecord=_unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
           if(memberhealthrecord == null) {return null; }
            return new HealthRecordViewModel()
            {
                BloodType = memberhealthrecord.BloodType,
                Note = memberhealthrecord.Note,
                Weight = memberhealthrecord.Width,
                Height = memberhealthrecord.Height,


            };
        }
        //5   عرض البيانات القديمة الي هيتم التعديل عليها 
        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberID)
        {
            
            var member=_unitOfWork.GetRepository<Member>().GetById(MemberID);
            if(member == null) {return null;}
            return new MemberToUpdateViewModel() 
            {
            Email=member.Email,
            Phone=member.Phone,
            Photo=member.Photo,
            BuildingNumber=member.Address.BuildingNumber,
            City=member.Address.City,
            Street=member.Address.Street,
            
            };
        }
        //6 التعديل علي البيانات 
        public bool UpdateMemberDetails(int MemberId, MemberToUpdateViewModel UpdatedMember)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Member>();


                var emailexist= Repo.GetAll(x=>x.Email == UpdatedMember.Email).Any();
                var phoneexist = Repo.GetAll(x => x.Phone == UpdatedMember.Phone).Any();
                if(emailexist && phoneexist) { return false; }
                var membertoupdate = Repo.GetById(MemberId);
                if(membertoupdate == null) { return false; }
                membertoupdate.Email = UpdatedMember.Email;
                membertoupdate.Phone = UpdatedMember.Phone;
                membertoupdate.Address.BuildingNumber = UpdatedMember.BuildingNumber;
                membertoupdate.Address.City= UpdatedMember.City;
                membertoupdate.UpdatedAT=DateTime.Now;
                Repo.Update(membertoupdate);
                return _unitOfWork.SaveChanges()>0;


            }
            catch (Exception ex) 
            { return false;
            }
        }          
        //7
        public bool RemoveMember(int memberId)
        {


            var MemberRepo = _unitOfWork.GetRepository<Member>();

            var member=MemberRepo.GetById(memberId);
            if( member == null ) { return false; }
            var activemembersession = _unitOfWork.GetRepository<MemberSession>().GetAll(x => x.MemberId == memberId&& x.Session.StartDate>DateTime.Now).Any();
            if(activemembersession) { return false; };

            var MemberShipRepo= _unitOfWork.GetRepository<MemberShip>();


            var memberships = MemberShipRepo.GetAll(x => x.MemberId == memberId);
            try 
            {
                if (memberships.Any())
                {
                    foreach (var membership in memberships) 
                    {
                        MemberShipRepo.Delete(membership);
                        
                    }

                }
                MemberRepo.Delete(member);
               return _unitOfWork.SaveChanges()>0;
            
            }
            catch (Exception ex) { return false; }

            
        }


    }
}
