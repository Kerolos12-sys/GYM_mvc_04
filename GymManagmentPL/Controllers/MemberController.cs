using GymManagmentBLL.Services.Classes;
using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public ActionResult Index()
        {

            var members = _memberService.GetAllMembers();
            return View(members);
        }


        public ActionResult MemberDetails(int id)
        {
            var member = _memberService.GetMemberDetails(id);


            if (id <= 0)
            {

                RedirectToAction(nameof(Index));
            }

            if (member == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
                return RedirectToAction(actionName: nameof(Index));

            var HealthRecord = _memberService.GetMemberHealthRecordDetails(id);

            if (HealthRecord is null)
                return RedirectToAction(actionName: nameof(Index));

            return View(model: HealthRecord);
        }


        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public ActionResult CreateMember(CreatMemberViewModel CreatedMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(key: "DataInvalid", errorMessage: "Check Data And Missing Fields");
                return View(viewName: nameof(Create), model: CreatedMember);
            }

            bool Result = _memberService.CreateMember(CreatedMember);

            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Create, Check Phone And Email";
            }

            return RedirectToAction(actionName: nameof(Index));
        }


        [HttpGet]
        public ActionResult MemberEdit( int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            
            return View(member); 
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel MemberToEdit)
        {
            if (!ModelState.IsValid)
                return View(model: MemberToEdit);

            var result = _memberService.UpdateMemberDetails(id, MemberToEdit);

            if (result)
                TempData["SuccessMessage"] = "Member Updated Successfully";
            else
                TempData["ErrorMessage"] = "Member Failed To Update";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member cannot be 0 or a negative number";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MemberId = id;
            return View(member);
        }


        [HttpPost]
        public ActionResult DeleteConfirmed( int id)
        {
            var result = _memberService.RemoveMember(id);

            if (result)
                TempData["SuccessMessage"] = "Member deleted successfully";
            else
                TempData["ErrorMessage"] = "Failed to delete member";

            return RedirectToAction(nameof(Index));
        }



    }
}
