using Microsoft.AspNetCore.Mvc;
using ShowUp2Move_BLL;

namespace ShowUp2Move.Controllers
{
    public class EventController : Controller
    {
        // All Events
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            List<clsEvent_BLL> events = clsEvent_BLL.GetAll();
            return View(events);
        }

        // Event Detail
        public IActionResult Detail(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            clsEvent_BLL? ev = clsEvent_BLL.Find(id);

            if (ev == null)
                return RedirectToAction("Index");

            return View(ev);
        }

        //  GET Create 

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Sports = clsSport_BLL.GetAll();
            return View();
        }

        //POST Create 
        [HttpPost]
        public IActionResult Create(int sportID, string location, DateTime eventDate, string description)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            int newEventID = -1;

            bool success = clsEvent_BLL.Add(userID, sportID, location, eventDate, description, true, ref newEventID);

            if (success)
                return RedirectToAction("Detail", new { id = newEventID });

            TempData["Error"] = "Failed to create event.";
            ViewBag.Sports = clsSport_BLL.GetAll();
            return View();
        }
    }
}