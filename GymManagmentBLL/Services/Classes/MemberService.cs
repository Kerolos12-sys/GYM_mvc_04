using AutoMapper;
using AutoMapper.Execution;
using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels.MemberViewModels;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityMember = GymManagmentDAL.Entities.Member;

namespace GymManagmentBLL.Services.Classes
{
    public class MemberService : IMemberService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MemberService( IUnitOfWork unitOfWork, IMapper mapper)
        {  
         
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        




        //1
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var   Members=_unitOfWork.GetRepository<EntityMember>().GetAll();
            if (Members == null) 
                return Enumerable.Empty<MemberViewModel>();


            return _mapper.Map<IEnumerable<MemberViewModel>>(Members);
        }

        //2
        public bool CreateMember(CreatMemberViewModel creatMember)
        {
            try {
            var emailExists = _unitOfWork.GetRepository<EntityMember>().GetAll(x => x.Email == creatMember.Email).Any();
            var phoneExists = _unitOfWork.GetRepository<EntityMember>().GetAll(x=>x.Phone == creatMember.Phone).Any();
            if (phoneExists||emailExists) { return false; }

                var member = _mapper.Map<EntityMember>(creatMember);
                _unitOfWork.GetRepository<EntityMember>().Add(member);
                return _unitOfWork.SaveChanges() > 0;

                }

            catch (Exception ex) { return false; }
        
        }
      
        //3
        public MemberViewModel GetMemberDetails(int MemberId)
        {
           
            var Member=_unitOfWork.GetRepository<EntityMember>().GetById(MemberId);
            if (Member == null) { return null; }
           var viewmodel= _mapper.Map<MemberViewModel>(Member);

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
           var x=_mapper.Map<HealthRecordViewModel>(memberhealthrecord);
            return x;
        }
        //5   عرض البيانات القديمة الي هيتم التعديل عليها 
        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberID)
        {
            
            var member=_unitOfWork.GetRepository<EntityMember>().GetById(MemberID);
            if(member == null) {return null;}
            var x=_mapper.Map<MemberToUpdateViewModel>(member);
            return x;
        }
        //6 التعديل علي البيانات 
        public bool UpdateMemberDetails(int MemberId, MemberToUpdateViewModel UpdatedMember)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<EntityMember>();


                var emailexist= Repo.GetAll(x=>x.Email == UpdatedMember.Email && x.Id!=MemberId).Any();
                var phoneexist = Repo.GetAll(x => x.Phone == UpdatedMember.Phone && x.Id != MemberId).Any();
                if (emailexist && phoneexist) { return false; }
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


            var MemberRepo = _unitOfWork.GetRepository<EntityMember>();

            var member=MemberRepo.GetById(memberId);
            if( member == null ) { return false; }




            var sessionIds = _unitOfWork.GetRepository<MemberSession>()
            .GetAll(condition: b => b.MemberId == memberId)
            .Select(x => x.SessionId);

            var hasFutureSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(condition: x => sessionIds.Contains(x.Id) && x.StartDate > DateTime.Now)
                .Any();

            if (hasFutureSessions)
                return false;

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
