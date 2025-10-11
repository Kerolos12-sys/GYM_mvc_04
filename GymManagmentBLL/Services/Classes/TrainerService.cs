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
        public readonly IGenericRepository<Trainer> _TrainerRepository;
        public TrainerService(IGenericRepository<Trainer> TrainerRepository)
        { 
             
            _TrainerRepository = TrainerRepository;
            
        
        
        }



        //1
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers=_TrainerRepository.GetAll();
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
                var emailExists = _TrainerRepository.GetAll(x => x.Email == createTrainer.Email).Any();
                var phoneExists = _TrainerRepository.GetAll(x => x.Phone == createTrainer.Phone).Any();
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
                return _TrainerRepository.Add(Trainer) > 0;

            }
            catch (Exception ex) { return false;   }



        }
        //3
        public TrainerViewModel GetTrainerDetails(int TrainerId)
        {
            var Trainer = _TrainerRepository.GetById(TrainerId);
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

            var trainer= _TrainerRepository.GetById(TrainerID);
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
                var emailexist = _TrainerRepository.GetAll(x => x.Email == UpdatedTrainer.Email).Any();
                var phoneexist = _TrainerRepository.GetAll(x => x.Phone == UpdatedTrainer.Phone).Any();
                if (emailexist && phoneexist) { return false; }
                var trainertoupdate = _TrainerRepository.GetById(TrainerId);
                if (trainertoupdate == null) { return false; }
                trainertoupdate.Email = UpdatedTrainer.Email;
                trainertoupdate.Phone = UpdatedTrainer.Phone;
                trainertoupdate.Address.BuildingNumber = UpdatedTrainer.BuildingNumber;
                trainertoupdate.Address.City = UpdatedTrainer.City;
                trainertoupdate.Specialties = UpdatedTrainer.specialties;
                trainertoupdate.UpdatedAT = DateTime.Now;
                return _TrainerRepository.Update(trainertoupdate)>0;


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
                var trainer = _TrainerRepository.GetById(TrainerId);
                if (trainer == null)
                    return false;

                // Check if trainer has any future sessions
                bool hasFutureSessions = trainer.TrainerSessions
                    .Any(s => s.StartDate > DateTime.Now);

                if (hasFutureSessions)
                    return false; // Can't delete trainer with future sessions

                // Safe to delete
                return _TrainerRepository.Delete(trainer) > 0;
            }
            catch (Exception)
            {
                return false;
            }


        }
    }
}
