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
        //public readonly IGenericRepository<Trainer> _TrainerRepository;
        //public TrainerService(IGenericRepository<Trainer> TrainerRepository)
        //{ 

        //    _TrainerRepository = TrainerRepository;



        //}

        private readonly IUnitOfWork _unitOfWork;
        public TrainerService(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

        }
        

   




        //1
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers=_unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainers == null) { return null; }
            var TrainersViewModels = Trainers.Select(x => new TrainerViewModel
            {

                Id = x.Id,
                Email = x.Email,
                Phone = x.Phone,
                Specialization = x.Specialties.ToString(),
                Name = x.Name,



            });
            return TrainersViewModels;
        }
        //2
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var emailExists = _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == createTrainer.Email).Any();
                var phoneExists = _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Phone == createTrainer.Phone).Any();
                if (phoneExists || emailExists) { return false; }
                var Trainer = new Trainer()
                {
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    Name = createTrainer.Name,
                    DateOfBirth = createTrainer.DateOfBirth,
                    Gender = createTrainer.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createTrainer.BuildingNumber,
                        City = createTrainer.City,
                        Street = createTrainer.Street

                    },
                    Specialties=createTrainer.specialties

                };
                _unitOfWork.GetRepository<Trainer>().Add(Trainer);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex) { return false;   }



        }
        //3
        public TrainerViewModel GetTrainerDetails(int TrainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (Trainer == null) { return null; }
            var viewmodel = new TrainerViewModel()
            {
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                DateOfBirth = Trainer.DateOfBirth.ToString(),
                Address = $"{Trainer.Address.BuildingNumber}-{Trainer.Address.Street}-{Trainer.Address.City}",
                Specialization=Trainer.Specialties.ToString(),

            };

         
            return viewmodel;
        }
        //4 عرض البيانات القديمة 
        public TrainerToUpdateViewModel? GetMemberToUpdate(int TrainerID)
        {

            var trainer= _unitOfWork.GetRepository<Trainer>().GetById(TrainerID);
            if (trainer == null) { return null; }
            return new TrainerToUpdateViewModel()
            {
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Street = trainer.Address.Street,
                specialties=trainer.Specialties,

            };
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
