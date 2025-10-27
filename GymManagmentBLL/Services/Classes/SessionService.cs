using AutoMapper;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels.SessionViewModel;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Classes;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymManagmentBLL.Services.Classes
{
    public class SessionService : ISessionService
    {

        public readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork  ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            try
            {
                // Check if Trainer exists
                if (!IsTrainerExists(createdSession.TrainerId))
                    return false;

                // Check if Category exists
                if (!IsCategoryExists(createdSession.CategoryId))
                    return false;

                // Check if StartDate is before EndDate
                if (!IsDateTimeValid(createdSession.StartDate, createdSession.EndDate))
                    return false;

                // Check if Capacity is within valid range (1–25)
                if (createdSession.Capacity > 25 || createdSession.Capacity < 1)
                    return false;

                // Map from ViewModel to Entity
                var sessionEntity = _mapper.Map<Session>(createdSession);

                // Add to database
                _unitOfWork.GetRepository<Session>().Add(sessionEntity);

                // Save changes
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create Session Failed: {ex}");
                return false;
            }

        }

        public IEnumerable<SessionViewModel> GetAllSession()
        {
            var sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (!sessions.Any())
                return [];

            //return sessions.Select(s => new SessionViewModel
            //{
            //    Id = s.Id,
            //    Description = s.Description,
            //    StartDate = s.StartDate,
            //    EndDate = s.EndDate,
            //    Capacity = s.Capacity,
            //    TrainerName = s.Trainer.Name,       
            //    CategoryName = s.SessionCategory.CategoryName,     
            //    AvailableSlots =s.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(s.Id),            
            //});
            var mappedsessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedsessions)
            { session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            }
            return mappedsessions;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
         
            var Session =_unitOfWork.SessionRepository.GetSessionsWithTrainerAndCategory(sessionId);
            var mappedsession=_mapper.Map<Session, SessionViewModel>(Session);
            mappedsession.AvailableSlots = mappedsession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(mappedsession.Id);
            return mappedsession;
        }

        #region Helper Methods


        private bool IsSessionAvailableForUpdating(Session session)
        {
            if (session is null)
                return false;

            // If Session Completed → No Updates Allowed
            if (session.EndDate < DateTime.Now)
                return false;

            // If Session Already Started → No Updates Allowed
            if (session.StartDate <= DateTime.Now)
                return false;

            // If Session Has Active Bookings → No Updates Allowed
            var hasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (hasActiveBooking)
                return false;

            return true;
        }
        private bool IsSessionAvailableForRemoving(Session session)
        {
            if (session == null)
                return false;

            // If Session Started → No Delete Allowed
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now)
                return false;

            // If Session Is Upcoming → No Delete Allowed
            if (session.StartDate > DateTime.Now)
                return false;

            // If Session Has Active Bookings → No Delete Allowed
            var hasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (hasActiveBooking)
                return false;

            return true;
        }
        private bool IsTrainerExists(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
        }

        private bool IsCategoryExists(int categoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;
        }

        private bool IsDateTimeValid(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate;
        }
           #endregion
        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (!IsSessionAvailableForUpdating(session))
                return null;

            return _mapper.Map<UpdateSessionViewModel>(session);
        }

        public bool UpdateSession(UpdateSessionViewModel updatedSession, int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);

               
                if (!IsSessionAvailableForUpdating(session))
                    return false;

               
                if (!IsTrainerExists(updatedSession.TrainerId))
                    return false;

                
                if (!IsDateTimeValid(updatedSession.StartDate, updatedSession.EndDate))
                    return false;

                
                _mapper.Map(updatedSession, session);

                // تحديث تاريخ التعديل
                session.UpdatedAT = DateTime.Now;

                // حفظ التغييرات
                _unitOfWork.SessionRepository.Update(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Session Failed: {ex}");
                return false;
            }
        }

        public bool RemoveSession(int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForRemoving(session))
                    return false;

                _unitOfWork.SessionRepository.Delete(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove Session Failed: {ex}");
                return false;
            }
        }
    }
}
