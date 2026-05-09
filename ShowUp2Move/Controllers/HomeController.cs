using Microsoft.AspNetCore.Mvc;
using ShowUp2Move.BLL;

namespace ShowUp2Move.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsUser? user = clsUser.Find(userID);

            if (user == null)
                return RedirectToAction("Login", "Account");

            ViewBag.UnreadCount = clsNotification.GetByUser(userID, unreadOnly: true).Count;

            return View(user);
        }

        [HttpPost]
        public IActionResult ToggleAvailability(bool isAvailable)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            int userID = HttpContext.Session.GetInt32("UserID")!.Value;
            clsUser.UpdateAvailability(userID, isAvailable);

            return RedirectToAction("Dashboard");
        }
    }
}