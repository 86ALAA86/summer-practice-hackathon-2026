using Microsoft.AspNetCore.Mvc;
using ShowUp2Move.BLL;

namespace ShowUp2Move.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            return View(clsEvent.GetAll());
        }

        public IActionResult Detail(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            clsEvent? ev = clsEvent.Find(id);
            if (ev == null) return RedirectToAction("Index");

            return View(ev);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Sports = clsSport.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(int sportID, string location, DateTime eventDate, string description)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            int newEventID = -1;

            bool success = clsEvent.Add(userID, sportID, location, eventDate, description, true, ref newEventID);

            if (success)
                return RedirectToAction("Detail", new { id = newEventID });

            TempData["Error"] = "Failed to create event.";
            ViewBag.Sports = clsSport.GetAll();
            return View();
        }
    }
}