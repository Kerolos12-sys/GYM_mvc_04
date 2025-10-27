using GymManagmentBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class SessionController : Controller
    {

        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #region Get All Sessions
        public ActionResult Index()
        {
            var sessions = _sessionService.GetAllSession();
            return View(model: sessions);
        }
        #endregion


        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);

            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(model: session);
        }


    }
}
