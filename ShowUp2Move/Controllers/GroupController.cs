using Microsoft.AspNetCore.Mvc;
using ShowUp2Move_BLL;

namespace ShowUp2Move.Controllers
{
    public class GroupController : Controller
    {
        // My Groups
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            List<clsGroup_BLL> groups = clsGroup_BLL.GetGroupsByUser(userID);

            return View(groups);
        }

        // Group Detail 
        public IActionResult Detail(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            List<clsGroupMember> members = clsGroup_BLL.GetGroupMembers(id);
            ViewBag.GroupID = id;

            return View(members);
        }

        //Run Matching
        public IActionResult RunMatching()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            List<clsSport_BLL> sports = clsSport_BLL.GetAll();
            return View(sports);
        }

        [HttpPost]
        public IActionResult RunMatching(int sportID)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            // get sport info for group size
            clsSport_BLL? sport = clsSport_BLL.GetAll().FirstOrDefault(s => s.SportID == sportID);

            if (sport == null)
            {
                TempData["Error"] = "Sport not found.";
                return RedirectToAction("RunMatching");
            }

            bool success = clsGroup_BLL.RunMatching(sportID, sport.MinGroupSize, sport.MaxGroupSize);

            if (success)
                TempData["Success"] = $"Group created successfully for {sport.SportName}!";
            else
                TempData["Error"] = $"Not enough available players for {sport.SportName}. Need at least {sport.MinGroupSize}.";

            return RedirectToAction("Index");
        }

        //Confirm Participation
        [HttpPost]
        public IActionResult Confirm(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsGroup_BLL.ConfirmParticipation(id, userID);

            return RedirectToAction("Index");
        }
    }
}