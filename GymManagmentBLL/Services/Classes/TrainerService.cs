using AutoMapper;
using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels.MemberViewModels;
using GymManagmentBLL.ViewModels.TrainerViewModels;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Classes
{
    public class TrainerService : ITrainerervice
    {
    

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        

   




        //1
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers=_unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainers == null) { return null; }
            return _mapper.Map<IEnumerable<TrainerViewModel>>(Trainers);
        }
        //2
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var emailExists = _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == createTrainer.Email).Any();
                var phoneExists = _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Phone == createTrainer.Phone).Any();
                if (phoneExists || emailExists) { return false; }
                var trainer = _mapper.Map<Trainer>(createTrainer);
                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex) { return false;   }



        }
        //3
        public TrainerViewModel GetTrainerDetails(int TrainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (Trainer == null) { return null; }
            return _mapper.Map<TrainerViewModel>(Trainer);
        }
        //4 عرض البيانات القديمة 
        public TrainerToUpdateViewModel? GetMemberToUpdate(int TrainerID)
        {

            var trainer= _unitOfWork.GetRepository<Trainer>().GetById(TrainerID);
            if (trainer == null) { return null; }
            return _mapper.Map<TrainerToUpdateViewModel>(trainer);
        }
        //5  التعديل علي البيانات
        public bool UpdateTrainerDetails(int TrainerId, TrainerToUpdateViewModel UpdatedTrainer)
        {
            try
            {
                var emailexist = _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == UpdatedTrainer.Email).Any();
                var phoneexist = _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Phone == UpdatedTrainer.Phone).Any();
                if (emailexist && phoneexist) { return false; }
                var trainertoupdate = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
                if (trainertoupdate == null) { return false; }
                trainertoupdate.Email = UpdatedTrainer.Email;
                trainertoupdate.Phone = UpdatedTrainer.Phone;
                trainertoupdate.Address.BuildingNumber = UpdatedTrainer.BuildingNumber;
                trainertoupdate.Address.City = UpdatedTrainer.City;
                trainertoupdate.Specialties = UpdatedTrainer.specialties;
                trainertoupdate.UpdatedAT = DateTime.Now;
                _unitOfWork.GetRepository<Trainer>().Update(trainertoupdate);
                return _unitOfWork.SaveChanges()>0;


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //6
        public bool RemoveTrainer(int TrainerId)
        {
            try
            {
                var trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
                if (trainer == null)
                    return false;

                // Check if trainer has any future sessions
                bool hasFutureSessions = trainer.TrainerSessions
                    .Any(s => s.StartDate > DateTime.Now);

                if (hasFutureSessions)
                    return false; // Can't delete trainer with future sessions

                // Safe to delete
                _unitOfWork.GetRepository<Trainer>().Delete(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }


        }
    }
}
