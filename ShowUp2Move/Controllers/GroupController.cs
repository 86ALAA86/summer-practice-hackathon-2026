using Microsoft.AspNetCore.Mvc;
using ShowUp2Move.BLL;

namespace ShowUp2Move.Controllers
{
    public class GroupController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            return View(clsGroup.GetGroupsByUser(userID));
        }

        public IActionResult Detail(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.GroupID = id;
            ViewBag.Members = clsGroup.GetGroupMembers(id);
            ViewBag.Venues = clsVenue.GetByGroup(id);
            ViewBag.Polls = clsPoll.GetByGroup(id);

            return View();
        }

        public IActionResult RunMatching()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            return View(clsSport.GetAll());
        }

        [HttpPost]
        public IActionResult RunMatching(int sportID)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            clsSport? sport = clsSport.GetAll().FirstOrDefault(s => s.SportID == sportID);

            if (sport == null)
            {
                TempData["Error"] = "Sport not found.";
                return RedirectToAction("RunMatching");
            }

            bool success = clsGroup.RunMatching(sportID, sport.MinGroupSize, sport.MaxGroupSize);

            TempData[success ? "Success" : "Error"] = success
                ? $"Group created for {sport.SportName}!"
                : $"Not enough available players for {sport.SportName}. Need at least {sport.MinGroupSize}.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Confirm(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsGroup.ConfirmParticipation(id, userID);

            return RedirectToAction("Index");
        }
    }
}