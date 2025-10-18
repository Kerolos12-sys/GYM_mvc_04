using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels.PLanViewModels;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Classes
{
    public class PLanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PLanService(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }



    
        public IEnumerable<PlanViewModel> GetALlPlans()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (Plans is null || !Plans.Any()) return [];

            return Plans.Select(p => new PlanViewModel
            {
                ID = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                IsActive = p.IsActive,
                Price = p.Price
            });
        }


        public PlanViewModel? GetPlanByID(int planid)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(planid);
            if (Plan is null) return null;

            return new PlanViewModel
            {
                ID = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                IsActive = Plan.IsActive,
                Price = Plan.Price
            };




        }



        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || plan.IsActive == false ||
                HasActiveMemberships(PlanId))
                return null;

            return new UpdatePlanViewModel
            {
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                PlanName = plan.Name,
                Price = plan.Price
            };
        }

       

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatePlan)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || HasActiveMemberships(PlanId))
                return false;

            try
            {
                // تحديث الخصائص المطلوبة
                plan.Description = updatePlan.Description;
                plan.Price = updatePlan.Price;
                plan.DurationDays = updatePlan.DurationDays;
                plan.UpdatedAT = DateTime.Now;

                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }





        }

        public bool ToggleStatus(int PlanId)
        {
            var repo = _unitOfWork.GetRepository<Plan>();
            var plan = repo.GetById(PlanId);
            if (plan == null || HasActiveMemberships(PlanId))
                return false;

            plan.IsActive = plan.IsActive == true ? false : true;
            plan.UpdatedAT = DateTime.Now;

            try
            {
                repo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }



        #region Helper
        private bool HasActiveMemberships(int planId)
        {
            var activeMemberships = _unitOfWork.GetRepository<MemberShip>()
                .GetAll(x => x.PlanId == planId && x.Status == "Active");

            return activeMemberships.Any();
        }
        #endregion

    }
}
