using GymManagmentBLL.ViewModels.PLanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Interfaces
{
    public interface IPlanService 
    {

        IEnumerable<PlanViewModel> GetALlPlans();
        PlanViewModel? GetPlanByID(int planid);

        UpdatePlanViewModel? GetPlanToUpdate(int PlanId);
        bool UpdatePlan(int PlanId, UpdatePlanViewModel updatePlan);

        bool ToggleStatus(int PlanId);
    }
}
