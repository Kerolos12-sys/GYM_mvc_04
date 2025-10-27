using GymManagmentBLL.Services.Classes;
using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels.PLanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _pLanService;

        public PlanController(IPlanService pLanService)
        {
            _pLanService = pLanService;
        }
        public IActionResult Index()
        {

            var plans = _pLanService.GetALlPlans();
            return View(plans);
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }

            var plan = _pLanService.GetPlanByID(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }

            var plan = _pLanService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(model: plan);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel updatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(model: updatedPlan);
            }

            var result = _pLanService.UpdatePlan(id, updatedPlan);

            if (result)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed To Update";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult Activate(int id)
        {
            var result = _pLanService.ToggleStatus(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Plan Status Changed Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Change Plan Status";
            }

            return RedirectToAction(nameof(Index));
        }




    }
}
